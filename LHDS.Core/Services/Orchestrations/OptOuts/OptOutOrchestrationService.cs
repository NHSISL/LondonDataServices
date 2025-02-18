// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Brokers.CsvHelpers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Mesh;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Orchestrations.OptOuts;
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
        private readonly ICsvHelperBroker csvHelperBroker;
        private readonly BlobContainers blobContainers;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIdentifierBroker identifierBroker;
        //private readonly ISecurityBroker securityBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly OptOutConfiguration optOutConfiguration;
        private readonly MeshConfiguration meshConfiguration;

        public OptOutOrchestrationService(
            IOptOutProcessingService optOutProcessingService,
            IDocumentProcessingService documentProcessingService,
            IMeshProcessingService meshProcessingService,
            BlobContainers blobContainers,
            ICsvHelperBroker csvHelperBroker,
            IDateTimeBroker dateTimeBroker,
            IIdentifierBroker identifierBroker,
            //ISecurityBroker securityBroker,
            ILoggingBroker loggingBroker,
            OptOutConfiguration optOutConfiguration,
            MeshConfiguration meshConfiguration)
        {
            this.optOutProcessingService = optOutProcessingService;
            this.documentProcessingService = documentProcessingService;
            this.meshProcessingService = meshProcessingService;
            this.blobContainers = blobContainers;
            this.csvHelperBroker = csvHelperBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.identifierBroker = identifierBroker;
            //this.securityBroker = securityBroker;
            this.loggingBroker = loggingBroker;
            this.optOutConfiguration = optOutConfiguration;
            this.meshConfiguration = meshConfiguration;
        }

        public ValueTask<bool> ValidateMailboxAccessAsync() =>
              TryCatch(async () =>
              {
                  return await meshProcessingService.ValidateMailboxAccessAsync();
              });

        public ValueTask<string> RetrieveOptOutStatusAsync(Stream input, string fileName) =>
            TryCatch(async () =>
            {
                ValidateConfigurationSettings();
                ValidateArgumentsOnRetrieveOptOutStatus(input, fileName);

                bool withHeader =
                    optOutConfiguration.OptOutFileHasHeader;

                Dictionary<string, int>? fieldMappings = null;

                bool shouldAddTrailingComma =
                    optOutConfiguration.OptOutFileRequireTrailingComma;

                var inputString = Encoding.UTF8.GetString(ReadAllBytesFromStream(input));

                List<OptOutIdentifier> mappedOptOuts =
                    await this.csvHelperBroker
                        .MapCsvToObjectAsync<OptOutIdentifier>(
                            data: inputString,
                            hasHeaderRecord: withHeader,
                            fieldMappings: null,
                            headerValidated: false);

                List<OptOutIdentifier> processedOptOutIdentifiers = new List<OptOutIdentifier>();
                var exceptions = new List<Exception>();

                foreach (var optOut in mappedOptOuts)
                {
                    try
                    {
                        OptOut processedOptOut = await TryCatch(async () =>
                        {
                            DateTimeOffset timeStamp = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                            //EntraUser currentUser = await this.securityBroker.GetCurrentUserAsync();
                            var expirationDate = timeStamp.AddDays(-optOutConfiguration.ExpiredAfterDays);

                            OptOut item = await this.optOutProcessingService
                                .RetrieveOrAddOptOutAsync(
                                    new OptOut
                                    {
                                        Id = await this.identifierBroker.GetIdentifierAsync(),
                                        NhsNumber = optOut.NhsNumber,
                                        Status = string.IsNullOrWhiteSpace(optOut.Status) ? "Unknown" : optOut.Status,
                                        UniqueReference = optOut.UniqueReference,
                                        CacheTime = expirationDate,
                                        CreatedDate = timeStamp,
                                        UpdatedDate = timeStamp,
                                        CreatedBy = "System",
                                        UpdatedBy = "System"
                                    });

                            return item;
                        });

                        OptOutIdentifier processedOptOutIdentifier = new OptOutIdentifier
                        {
                            NhsNumber = processedOptOut.NhsNumber,
                            UniqueReference = processedOptOut.UniqueReference,
                            Status = processedOptOut.Status,
                            StatusChangedDateTime = processedOptOut.CacheTime
                        };

                        processedOptOutIdentifiers.Add(processedOptOutIdentifier);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }
                if (exceptions.Any())
                {
                    throw new AggregateException(
                        $"Unable to retrieve or add opt out for {exceptions.Count} mapped opt outs",
                        exceptions);
                }

                string processedData = await this.csvHelperBroker
                    .MapObjectToCsvAsync(processedOptOutIdentifiers, withHeader, fieldMappings, shouldAddTrailingComma);

                byte[] processedBytes = Encoding.UTF8.GetBytes(processedData);

                DateTimeOffset currentDateTimeOffset =
                    await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

                string timeStamp = currentDateTimeOffset.ToString("yyyyMMddHHmmss");

                string csvFileName = $"{optOutConfiguration.OutputFolder}/" +
                    $"{Path.GetFileNameWithoutExtension(fileName)}_{timeStamp}_Response.csv";

                using (Stream processed = new MemoryStream(processedBytes))
                {
                    await this.documentProcessingService.AddDocumentAsync(
                        input: processed,
                        csvFileName,
                        container: blobContainers.OptOut);
                }

                return csvFileName;
            });

        public ValueTask<MeshMessage?> PushExpiredOptOutsToMeshForRenewalAsync() =>
            TryCatch(async () =>
            {
                ValidateConfigurationSettings();
                bool shouldAddTrailingComma = this.optOutConfiguration.OptOutFileRequireTrailingComma;

                List<OptOut> expiredOptOuts = await
                    this.optOutProcessingService
                        .RetrieveAllExpiredOptOutsAsync(optOutConfiguration.ExpiredAfterDays);

                if (!expiredOptOuts.Any())
                {
                    return null;
                }

                List<string> expiredOptOutIdentifiers =
                    expiredOptOuts.Select(optout => $"{optout.NhsNumber},").ToList();

                StringBuilder csvExpiredOptOutIdentifiers = new StringBuilder();

                foreach (var item in expiredOptOuts)
                {
                    csvExpiredOptOutIdentifiers.AppendLine($"{item.NhsNumber},");
                }

                DateTimeOffset batchReferenceDateTime =
                    await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

                string batchReference = batchReferenceDateTime.ToString("yyyyMMddHHmmss");

                MeshMessage message = await this.meshProcessingService.SendMessageAsync(
                    mexTo: this.optOutConfiguration.To,
                    mexWorkflowId: this.optOutConfiguration.WorkflowId,
                    fileContent: Encoding.UTF8.GetBytes(csvExpiredOptOutIdentifiers.ToString()),
                    mexSubject: string.Empty,
                    mexLocalId: batchReference,
                    mexFileName: $"{batchReference}.txt",
                    mexContentChecksum: string.Empty,
                    contentType: "text/plain",
                    contentEncoding: string.Empty,
                    accept: "application/json");

                foreach (var optOut in expiredOptOuts)
                {
                    var dateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                    optOut.LastSentToMesh = dateTime;
                    optOut.UpdatedDate = dateTime;
                    optOut.BatchReference = batchReference;
                    await this.optOutProcessingService.AddOrModifyOptOutAsync(optOut);
                }

                return message;
            });

        public ValueTask<List<MeshMessage>> RetrieveUpdatedMeshConsentStatusesChangesAsync() =>
                TryCatch(async () =>
                {
                    ValidateConfigurationSettings();
                    bool withHeader = this.optOutConfiguration.OptOutFileHasHeader;
                    List<string> messageIds;
                    List<MeshMessage> meshMessageList = new List<MeshMessage>();
                    var exceptions = new List<Exception>();

                    while ((messageIds = await this.meshProcessingService.RetrieveMessageIdsFromInboxAsync()).Count > 0)
                    {
                        foreach (string messageId in messageIds)
                        {
                            try
                            {
                                MeshMessage returnedMessage = await TryCatch(async () =>
                                {
                                    MeshMessage message = await meshProcessingService
                                        .RetrieveMessageByIdAsync(messageId);

                                    if (GetKeyStringValue("mex-workflowid", message.Headers) !=
                                        this.optOutConfiguration.WorkflowId)
                                    {
                                        return null;
                                    }

                                    string[] delimiters = { "\r\n", "\n" };

                                    List<string> consentedIdentifiers = Encoding.UTF8
                                        .GetString(message.FileContent)
                                            .Replace(",", string.Empty)
                                                .Split(delimiters, StringSplitOptions.RemoveEmptyEntries).ToList();

                                    ValidateLocalIdHeaderExists(message);
                                    string batchReference = GetHeaderValue(message, "mex-localid");
                                    ValidateBacthReferenceExists(batchReference);

                                    List<OptOut> originalBatch = await this.optOutProcessingService
                                        .RetrieveAllOptOutsByBatchReferenceAsync(batchReference);

                                    List<OptOut> delta = await this.optOutProcessingService
                                        .ConsolidateOptOutChangesAndReturnChangesOnly(
                                            originalBatch,
                                            consentedIdentifiers);

                                    if (delta?.Count > 0)
                                    {
                                        List<OptOutIdentifier> differentIdentifiers = delta
                                            .Select(identifier => new OptOutIdentifier
                                            {
                                                NhsNumber = identifier.NhsNumber,
                                                UniqueReference = identifier.UniqueReference,
                                                Status = identifier.Status,
                                                StatusChangedDateTime = identifier.CacheTime
                                            }).ToList();

                                        string csvDifferences = await this.csvHelperBroker
                                            .MapObjectToCsvAsync(
                                                @object: differentIdentifiers,
                                                addHeaderRecord: this.optOutConfiguration.OptOutFileHasHeader,
                                                shouldAddTrailingComma: this.optOutConfiguration.OptOutFileRequireTrailingComma);

                                        string messageFilename = GetHeaderValue(message, "mex-filename");

                                        DateTimeOffset currentDateTimeOffset =
                                            await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

                                        string timeStamp = currentDateTimeOffset.ToString("yyyyMMddHHmmss");

                                        string fileName = $"{optOutConfiguration.OutputFolder}/" +
                                            $"{messageFilename}_{timeStamp}_Response.csv";

                                        ValidateDocumentRequirements(csvDifferences, fileName);
                                        byte[] csvDifferencesBytes = Encoding.UTF8.GetBytes(csvDifferences);

                                        using (Stream input = new MemoryStream(csvDifferencesBytes))
                                        {
                                            await this.documentProcessingService.AddDocumentAsync(
                                            input, fileName, container: blobContainers.OptOut);
                                        }
                                    }

                                    await this.meshProcessingService.AcknowledgeMessageByIdAsync(messageId);

                                    return message;
                                });

                                if (returnedMessage == null)
                                {
                                    continue;
                                }

                                meshMessageList.Add(returnedMessage);
                            }
                            catch (Exception ex)
                            {
                                exceptions.Add(ex);
                            }
                        }

                        if (exceptions.Any())
                        {
                            throw new AggregateException(
                                $"Unable to retrieve message for {exceptions.Count} message IDs",
                                exceptions);
                        }
                    }

                    return meshMessageList;
                });

        private static string GetHeaderValue(MeshMessage message, string keyToFind)
        {
            List<string>? value = new List<string>();

            foreach (var key in message.Headers.Keys)
            {
                if (key.ToLower() == keyToFind.ToLower())
                {
                    message.Headers.TryGetValue(key, out value);

                    break;
                }
            }

            return value?.FirstOrDefault() ?? string.Empty;
        }

        private static string GetKeyStringValue(string key, Dictionary<string, List<string>> dictionary)
        {
            var value = dictionary.ContainsKey(key)
                ? dictionary[key]?.First()
                : string.Empty;

            return value ?? string.Empty;
        }

        static byte[] ReadAllBytesFromStream(Stream stream)
        {
            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
