// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Audits;
using LHDS.Core.Brokers.CsvHelpers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.AddressToUprnFileLogs;
using LHDS.Core.Models.Foundations.AssignAddresses;
using LHDS.Core.Models.Orchestrations.AddressToUprns;
using LHDS.Core.Services.Foundations.AddressToUprnFileLogs;
using LHDS.Core.Services.Foundations.Assigns;
using LHDS.Core.Services.Foundations.Documents;

namespace LHDS.Core.Services.Orchestrations.AddressToUprns
{
    public partial class AddressToUprnOrchestrationService : IAddressToUprnOrchestrationService
    {
        private readonly IAssignService assignService;
        private readonly IDocumentService documentService;
        private readonly IAddressToUprnFileLogService addressToUprnFileLogService;
        private readonly ICsvHelperBroker csvHelperBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IAuditBroker auditBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IIdentifierBroker identifierBroker;
        private readonly AddressToUprnConfiguration addressToUprnConfiguration;
        private readonly BlobContainers blobContainers;

        public AddressToUprnOrchestrationService(
            IAssignService assignService,
            IDocumentService documentService,
            IAddressToUprnFileLogService addressToUprnFileLogService,
            ICsvHelperBroker csvHelperBroker,
            IDateTimeBroker dateTimeBroker,
            IAuditBroker auditBroker,
            ILoggingBroker loggingBroker,
            IIdentifierBroker identifierBroker,
            AddressToUprnConfiguration addressToUprnConfiguration,
            BlobContainers blobContainers)
        {
            this.assignService = assignService;
            this.documentService = documentService;
            this.addressToUprnFileLogService = addressToUprnFileLogService;
            this.csvHelperBroker = csvHelperBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.auditBroker = auditBroker;
            this.loggingBroker = loggingBroker;
            this.identifierBroker = identifierBroker;
            this.addressToUprnConfiguration = addressToUprnConfiguration;
            this.blobContainers = blobContainers;
        }

        public ValueTask MatchAddressToUprnAsync(
            Stream input,
            string fileName,
            Guid correlationId,
            CancellationToken cancellationToken = default) =>
        TryCatch(async () =>
        {
            ValidateMatchAddressToUprnArguments(input, fileName, correlationId);

            DateTimeOffset startTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            Guid fileLogId = await this.identifierBroker.GetIdentifierAsync();

            var addressToUprnFileLog = new AddressToUprnFileLog
            {
                Id = fileLogId,
                FileName = fileName,
                DateReceived = startTime,
                SuccessStatus = SuccessStatus.Success
            };

            long fileSizeLimitBytes = (long)this.addressToUprnConfiguration.MaxFileSizeLimitMb * 1024 * 1024;

            if (!input.CanSeek)
            {
                addressToUprnFileLog.SuccessStatus = SuccessStatus.Failed;
                addressToUprnFileLog.Message = "Stream is not seekable.";
                addressToUprnFileLog.DateProcessed = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                addressToUprnFileLog.Duration = addressToUprnFileLog.DateProcessed - startTime;
                await this.addressToUprnFileLogService.AddAddressToUprnFileLogAsync(addressToUprnFileLog);

                await WriteErrorFileAsync(
                    container: this.blobContainers.AddressToUprn,
                    errorFolder: this.addressToUprnConfiguration.ErrorFolder,
                    fileName: fileName,
                    errorLine: "Stream is not seekable.",
                    cancellationToken: cancellationToken);

                return;
            }

            if (input.Length > fileSizeLimitBytes)
            {
                addressToUprnFileLog.SuccessStatus = SuccessStatus.Failed;

                addressToUprnFileLog.Message =
                    $"File too large, {correlationId}, " +
                    $"File exceeds the maximum allowed size of " +
                    $"{this.addressToUprnConfiguration.MaxFileSizeLimitMb} MB.";

                addressToUprnFileLog.DateProcessed = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                addressToUprnFileLog.Duration = addressToUprnFileLog.DateProcessed - startTime;
                await this.addressToUprnFileLogService.AddAddressToUprnFileLogAsync(addressToUprnFileLog);

                await WriteErrorFileAsync(
                    container: this.blobContainers.AddressToUprn,
                    errorFolder: this.addressToUprnConfiguration.ErrorFolder,
                    fileName: fileName,
                    errorLine: $"File too large,\"{correlationId}\",\"{addressToUprnFileLog.Message}\"",
                    cancellationToken: cancellationToken);

                return;
            }

            string outboxContainer = this.blobContainers.AddressToUprn;
            string outboxPath = $"{this.addressToUprnConfiguration.OutboxFolder}/{fileName}";
            string errorPath = $"{this.addressToUprnConfiguration.ErrorFolder}/{fileName}";

            Pipe outputPipe = new Pipe();
            bool hasErrors = false;
            int entryCount = 0;
            int entriesMatched = 0;
            int errorRowCount = 0;
            List<string> errorLines = new List<string>();

            async Task WriteOutputCsvAsync()
            {
                try
                {
                    await using Stream writerStream = outputPipe.Writer.AsStream();

                    await this.csvHelperBroker.MapObjectToCsvAsync<AddressToUprnCsv>(
                        @object: ProcessAddressesAsync(
                            input: input,
                            correlationId: correlationId,
                            fileName: fileName,
                            onEntryProcessed: (matched) =>
                            {
                                entryCount++;

                                if (matched)
                                {
                                    entriesMatched++;
                                }
                            },
                            onError: (errorLine) =>
                            {
                                hasErrors = true;
                                errorRowCount++;
                                errorLines.Add(errorLine);
                            },
                            cancellationToken: cancellationToken),
                        outputStream: writerStream,
                        addHeaderRecord: true,
                        cancellationToken: cancellationToken);
                }
                catch (Exception ex)
                {
                    await outputPipe.Writer.CompleteAsync(ex);

                    return;
                }

                await outputPipe.Writer.CompleteAsync();
            }

            await Task.WhenAll(
                WriteOutputCsvAsync(),
                this.documentService.AddDocumentAsync(
                    input: outputPipe.Reader.AsStream(),
                    fileName: outboxPath,
                    container: outboxContainer).AsTask());

            if (errorLines.Count > 0)
            {
                hasErrors = true;
                StringBuilder errorCsvBuilder = new StringBuilder();
                errorCsvBuilder.AppendLine("Error,CorrelationId,Message");

                foreach (string errorLine in errorLines)
                {
                    errorCsvBuilder.AppendLine(errorLine);
                }

                byte[] errorBytes = Encoding.UTF8.GetBytes(errorCsvBuilder.ToString());
                using Stream errorStream = new MemoryStream(errorBytes);

                await this.documentService.AddDocumentAsync(
                    input: errorStream,
                    fileName: errorPath,
                    container: outboxContainer);
            }

            int entriesUnmatched = entryCount - entriesMatched - errorRowCount;
            addressToUprnFileLog.EntryCount = entryCount;
            addressToUprnFileLog.EntriesMatched = entriesMatched;
            addressToUprnFileLog.EntriesUnmatched = entriesUnmatched < 0 ? 0 : entriesUnmatched;
            addressToUprnFileLog.ErrorRowCount = errorRowCount;
            addressToUprnFileLog.SuccessStatus = hasErrors ? SuccessStatus.PartialSuccess : SuccessStatus.Success;
            addressToUprnFileLog.DateProcessed = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            addressToUprnFileLog.Duration = addressToUprnFileLog.DateProcessed - startTime;

            await this.addressToUprnFileLogService.AddAddressToUprnFileLogAsync(addressToUprnFileLog);
        });

        private async IAsyncEnumerable<AddressToUprnCsv> ProcessAddressesAsync(
            Stream input,
            Guid correlationId,
            string fileName,
            Action<bool> onEntryProcessed,
            Action<string> onError,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            IAsyncEnumerable<AddressToUprnInputCsv> inputRows =
                this.csvHelperBroker.MapCsvToObjectAsync<AddressToUprnInputCsv>(
                    data: input,
                    hasHeaderRecord: true,
                    cancellationToken: cancellationToken);

            await foreach (AddressToUprnInputCsv inputRow in inputRows.WithCancellation(cancellationToken))
            {
                string unstructuredAddress = inputRow.UnstructuredAddress;

                AssignAddress assignAddress;

                try
                {
                    assignAddress = await this.assignService.MatchAddressAsync(unstructuredAddress);
                }
                catch (Exception exception)
                {
                    string safeAddress = EscapeCsvField(unstructuredAddress);
                    string safeMessage = EscapeCsvField(exception.Message);

                    onError(
                        $"TimeOut,\"{correlationId}\"," +
                        $"\"Failed to process `{safeAddress}`. {safeMessage}\"");

                    continue;
                }

                bool isMatched = assignAddress?.Matched ?? false;
                onEntryProcessed(isMatched);

                yield return new AddressToUprnCsv
                {
                    AddressFormat = assignAddress?.AddressFormat,
                    PostcodeQuality = assignAddress?.PostcodeQuality,
                    IsMatched = isMatched.ToString(),
                    UPRN = assignAddress?.BestMatch?.UPRN,
                    Qualifier = assignAddress?.BestMatch?.Qualifier,
                    Classification = assignAddress?.BestMatch?.Classification,
                    Algorithm = assignAddress?.BestMatch?.Algorithm,
                    AddressNumber = assignAddress?.BestMatch?.ABPAddress?.Number,
                    AddressStreet = assignAddress?.BestMatch?.ABPAddress?.Street,
                    AddressTown = assignAddress?.BestMatch?.ABPAddress?.Town,
                    AddressPostcode = assignAddress?.BestMatch?.ABPAddress?.Postcode,
                    AddressOrganization = assignAddress?.BestMatch?.ABPAddress?.Organisation,
                    MatchPatternFlat = assignAddress?.BestMatch?.MatchPattern?.Flat,
                    MatchPatternBuilding = assignAddress?.BestMatch?.MatchPattern?.Building,
                    MatchPatternNumber = assignAddress?.BestMatch?.MatchPattern?.Number,
                    MatchPatternStreet = assignAddress?.BestMatch?.MatchPattern?.Street,
                    MatchPatternPostCode = assignAddress?.BestMatch?.MatchPattern?.Postcode,
                    CorrelationId = correlationId.ToString(),
                    UnstructuredAddress = unstructuredAddress
                };
            }
        }

        private async ValueTask WriteErrorFileAsync(
            string container,
            string errorFolder,
            string fileName,
            string errorLine,
            CancellationToken cancellationToken = default)
        {
            string errorPath = $"{errorFolder}/{fileName}";
            string errorContent = $"Error,CorrelationId,Message\n{errorLine}\n";
            byte[] errorBytes = Encoding.UTF8.GetBytes(errorContent);
            using Stream errorStream = new MemoryStream(errorBytes);

            await this.documentService.AddDocumentAsync(
                input: errorStream,
                fileName: errorPath,
                container: container);
        }

        private static string EscapeCsvField(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return value.Replace("\"", "\"\"");
        }
    }
}
