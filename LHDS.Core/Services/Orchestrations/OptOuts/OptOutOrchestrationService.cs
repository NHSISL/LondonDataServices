// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Brokers.CsvHelpers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Mesh;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.Documents;
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
            this.loggingBroker = loggingBroker;
            this.optOutConfiguration = optOutConfiguration;
            this.meshConfiguration = meshConfiguration;
        }

        public ValueTask<bool> ValidateMailboxAccessAsync() =>
              TryCatch(async () =>
              {
                  return await meshProcessingService.ValidateMailboxAccessAsync();
              });

        public ValueTask<string> RetrieveOptOutStatusAsync(byte[] optOutFile, string fileName) =>
            TryCatch(async () =>
            {
                ValidateConfigurationSettings();
                ValidateOptOutFileIsNotNull(optOutFile);
                ValidateRequestIdIsNotNull(fileName);

                bool withHeader =
                    optOutConfiguration.OptOutFileHasHeader;

                Dictionary<string, int>? fieldMappings = null;

                bool shouldAddTrailingComma =
                    optOutConfiguration.OptOutFileRequireTrailingComma;

                var inputString = Encoding.ASCII.GetString(optOutFile);

                List<OptOutIdentifier> mappedOptOuts =
                    await this.csvHelperBroker
                        .MapCsvToObjectAsync<OptOutIdentifier>(inputString, withHeader);

                List<OptOut> processedOptOuts = new List<OptOut>();

                foreach (var optOut in mappedOptOuts)
                {
                    DateTimeOffset timeStamp = this.dateTimeBroker.GetCurrentDateTimeOffset();
                    var expirationDate = timeStamp.AddDays(-optOutConfiguration.ExpiredAfterDays);

                    OptOut item = await this.optOutProcessingService
                        .RetrieveOrAddOptOutAsync(
                            new OptOut
                            {
                                Id = this.identifierBroker.GetIdentifier(),
                                NhsNumber = optOut.NhsNumber,
                                Status = string.IsNullOrWhiteSpace(optOut.Status) ? "Unknown" : optOut.Status,
                                UniqueReference = optOut.UniqueReference,
                                CacheTime = expirationDate,
                                CreatedDate = timeStamp,
                                UpdatedDate = timeStamp,
                                CreatedBy = "System",
                                UpdatedBy = "System"
                            });

                    processedOptOuts.Add(item);
                }

                var processedData = await this.csvHelperBroker
                    .MapObjectToCsvAsync(processedOptOuts, withHeader, fieldMappings, shouldAddTrailingComma);

                var processedBytes = Encoding.ASCII.GetBytes(processedData);

                Document document = new Document
                {
                    FileName = $"{optOutConfiguration.OutputFolder}/{fileName}_Response.csv",
                    DocumentData = processedBytes
                };

                string saveDocument = await this.documentProcessingService
                    .AddDocumentAsync(document, blobContainers.OptOut);

                return saveDocument;
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

                string batchReference = this.dateTimeBroker.GetCurrentDateTimeOffset().ToString("yyyyMMddHHmmss");

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
                    var dateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();
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

                while ((messageIds = await this.meshProcessingService.RetrieveMessageIdsFromInboxAsync()).Count > 0)
                {
                    foreach (string messageId in messageIds)
                    {
                        MeshMessage message = await meshProcessingService.RetrieveMessageByIdAsync(messageId);

                        if (GetKeyStringValue("mex-workflowid", message.Headers) != this.optOutConfiguration.WorkflowId)
                        {
                            continue;
                        }

                        meshMessageList.Add(message);
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
                            .ConsolidateOptOutChangesAndReturnChangesOnly(originalBatch, consentedIdentifiers);

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

                            string fileName = $"{optOutConfiguration.OutputFolder}/{batchReference}_deltaresponse.csv";

                            ValidateDocumentRequirements(csvDifferences, fileName);

                            Document document = new Document
                            {
                                DocumentData = Encoding.ASCII.GetBytes(csvDifferences),
                                FileName = fileName
                            };

                            await this.documentProcessingService.AddDocumentAsync(document, blobContainers.OptOut);
                        }

                        await this.meshProcessingService.AcknowledgeMessageByIdAsync(messageId);
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
    }
}
