// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Mesh;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Orchestrations.OptOuts;
using LHDS.Core.Services.Processings.CsvMappers;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.Mesh;
using LHDS.Core.Services.Processings.OptOuts;

namespace LHDS.Core.Services.Orchestrations.OptOuts
{
    public partial class OptOutOrchestrationService : IOptOutOrchestrationService
    {
        private readonly IOptOutProcessingService optOutProcessingService;
        private readonly IDocumentProcessingService documentProcessingService;
        private readonly IMeshProcessingService meshProcessingService;
        private readonly ICsvMapperProcessingService csvMapperProcessingService;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIdentifierBroker identifierBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly OptOutConfiguration optOutConfiguration;
        private readonly MeshConfiguration meshConfiguration;

        public OptOutOrchestrationService(
            IOptOutProcessingService optOutProcessingService,
            IDocumentProcessingService documentProcessingService,
            IMeshProcessingService meshProcessingService,
            ICsvMapperProcessingService csvMapperProcessingService,
            IDateTimeBroker dateTimeBroker,
            IIdentifierBroker identifierBroker,
            ILoggingBroker loggingBroker,
            OptOutConfiguration optOutConfiguration,
            MeshConfiguration meshConfiguration)
        {
            this.optOutProcessingService = optOutProcessingService;
            this.documentProcessingService = documentProcessingService;
            this.meshProcessingService = meshProcessingService;
            this.csvMapperProcessingService = csvMapperProcessingService;
            this.dateTimeBroker = dateTimeBroker;
            this.identifierBroker = identifierBroker;
            this.loggingBroker = loggingBroker;
            this.optOutConfiguration = optOutConfiguration;
            this.meshConfiguration = meshConfiguration;
        }

        public ValueTask RetrieveOptOutStatusAsync(byte[] optOutFile, string fileName) =>
            TryCatch(async () =>
            {
                ValidateConfigurationSettings();
                ValidateOptOutFileIsNotNull(optOutFile);
                ValidateRequestIdIsNotNull(fileName);

                bool withHeader =
                    optOutConfiguration.OptOutFileHasHeader;

                bool shouldAddTrailingComma =
                    optOutConfiguration.OptOutFileRequireTrailingComma;

                var inputString = Encoding.ASCII.GetString(optOutFile);

                List<OptOutIdentifier> mappedOptOuts =
                await this.csvMapperProcessingService.MapCsvToObjectAsync<OptOutIdentifier>(inputString, false);

                List<OptOut> processedOptOuts = new List<OptOut>();

                foreach (var optOut in mappedOptOuts)
                {
                    DateTimeOffset timeStamp = this.dateTimeBroker.GetCurrentDateTimeOffset();

                    OptOut item = await this.optOutProcessingService
                        .RetrieveOrAddOptOutAsync(
                            new OptOut
                            {
                                Id = this.identifierBroker.GetIdentifier(),
                                NhsNumber = optOut.NhsNumber,
                                OptOutStatus = "Unknown",
                                CreatedDate = timeStamp,
                                UpdatedDate = timeStamp,
                                CreatedBy = "System",
                                UpdatedBy = "System"
                            });

                    processedOptOuts.Add(item);
                }

                var processedData = await this.csvMapperProcessingService
                    .MapObjectToCsvAsync(processedOptOuts, withHeader, shouldAddTrailingComma);

                var processedBytes = Encoding.ASCII.GetBytes(processedData);

                Document document = new Document
                {
                    FileName = $"{optOutConfiguration.OutputFolder}/{fileName}_Response_{dateTimeBroker
                        .GetCurrentDateTimeOffset().ToString("yyyyMMddHHmmss")}.csv",
                    DocumentData = processedBytes
                };

                await this.documentProcessingService.AddDocumentAsync(document);
            });

        public ValueTask<MeshMessage> PushExpiredOptOutsToMeshForRenewalAsync() =>
            TryCatch(async () =>
            {
                ValidateConfigurationSettings();
                bool withHeader = this.optOutConfiguration.OptOutFileHasHeader;
                bool shouldAddTrailingComma = this.optOutConfiguration.OptOutFileRequireTrailingComma;

                List<OptOut> mappedOptOuts = await
                    this.optOutProcessingService
                        .RetrieveAllExpiredOptOutsAsync(optOutConfiguration.ExpiredAfterDays);

                List<OptOutIdentifier> mappedOptOutIdentifiers =
                    mappedOptOuts.Select(optout => new OptOutIdentifier { NhsNumber = optout.NhsNumber }).ToList();

                var processedOutputString = await this.csvMapperProcessingService
                       .MapObjectToCsvAsync(mappedOptOutIdentifiers, withHeader, shouldAddTrailingComma);

                string batchReference = this.dateTimeBroker.GetCurrentDateTimeOffset().ToString("yyyyMMddHHmmss");

                MeshMessage message = new MeshMessage
                {
                    StringContent = processedOutputString,
                    Headers = new Dictionary<string, List<string>>()
                };

                message.Headers.Add("Content-Type", new List<string> { "text/plain" });
                message.Headers.Add("Mex-FileName", new List<string> { batchReference });
                message.Headers.Add("Mex-From", new List<string> { this.meshConfiguration.MailboxId });
                message.Headers.Add("Mex-To", new List<string> { this.optOutConfiguration.To });
                message.Headers.Add("Mex-WorkflowID", new List<string> { this.optOutConfiguration.WorkflowId });

                message = await this.meshProcessingService.SendMessageAsync(message);

                foreach (var optOut in mappedOptOuts)
                {
                    var dateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();
                    optOut.LastSentToMesh = dateTime;
                    optOut.UpdatedDate = dateTime;
                    optOut.BatchReference = batchReference;

                    await this.optOutProcessingService.ModifyOptOutAsync(optOut);
                }

                return message;
            });

        public ValueTask<List<MeshMessage>> RetrieveUpdatedMeshConsentStatusesChangesAsync() =>
            TryCatch(async () =>
            {
                ValidateConfigurationSettings();
                bool withHeader = this.optOutConfiguration.OptOutFileHasHeader;

                List<string> messageIds = await
                    this.meshProcessingService.RetrieveMessageIdsFromInboxAsync();

                List<MeshMessage> meshMessageList = new List<MeshMessage>();

                foreach (string messageId in messageIds)
                {
                    MeshMessage message = await meshProcessingService.RetrieveAndAcknowledgeMessageByIdAsync(messageId);
                    meshMessageList.Add(message);
                    string batchReference = message.Headers["Mex-LocalID"].FirstOrDefault();

                    List<OptOutIdentifier> consentedIdentifierList =
                        await csvMapperProcessingService.MapCsvToObjectAsync<OptOutIdentifier>(
                            message.StringContent,
                            withHeader);

                    List<OptOut> originalBatch = await this.optOutProcessingService
                        .RetrieveAllOptOutsByBatchReferenceAsync(batchReference);

                    List<string> consentedIdentifiers = consentedIdentifierList
                        .Select(optOut => optOut.NhsNumber).ToList();

                    List<OptOut> consentedList = originalBatch
                        .Where(optOut => consentedIdentifiers.Contains(optOut.NhsNumber)).ToList();

                    List<OptOut> nonConsentedList = originalBatch.Except(consentedList).ToList();

                    List<OptOut> delta = new List<OptOut>();

                    foreach (var item in consentedList)
                    {
                        if (item.OptOutStatus != "Opt-In")
                        {
                            delta.Add(item);
                        }

                        var dateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();
                        item.UpdatedDate = dateTime;
                        item.CacheTime = dateTime;
                        item.LastSentToMesh = dateTime;
                        item.OptOutStatus = "Opt-In";

                        await this.optOutProcessingService.ModifyOptOutAsync(item);
                    }

                    foreach (var item in nonConsentedList)
                    {
                        if (item.OptOutStatus != "Opt-Out")
                        {
                            delta.Add(item);
                        }

                        var dateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();
                        item.UpdatedDate = dateTime;
                        item.CacheTime = dateTime;
                        item.LastSentToMesh = dateTime;
                        item.OptOutStatus = "Opt-Out";

                        await this.optOutProcessingService.ModifyOptOutAsync(item);
                    }

                    List<OptOutIdentifier> differentIdentifiers = delta
                        .Select(identifier => new OptOutIdentifier { NhsNumber = identifier.NhsNumber }).ToList();

                    string csvDifferences = await this.csvMapperProcessingService
                        .MapObjectToCsvAsync(
                            @object: differentIdentifiers,
                            addHeaderRecord: this.optOutConfiguration.OptOutFileHasHeader,
                            shouldAddTrailingComma: this.optOutConfiguration.OptOutFileRequireTrailingComma);

                    Document document = new Document
                    {
                        DocumentData = Encoding.ASCII.GetBytes(csvDifferences),
                        FileName = $"{optOutConfiguration.OutputFolder}/{batchReference}_received_" +
                            $"{this.dateTimeBroker.GetCurrentDateTimeOffset().ToString("yyyyMMddHHmmss")}.csv",
                    };

                    await this.documentProcessingService.AddDocumentAsync(document);
                }

                return meshMessageList;
            });
    }
}
