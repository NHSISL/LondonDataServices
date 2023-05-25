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

        public ValueTask<string> RetrieveOptOutStatusAsync(byte[] optOutFile, string fileName) =>
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
                    await this.csvMapperProcessingService
                        .MapCsvToObjectAsync<OptOutIdentifier>(inputString, withHeader);

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
                                Status = string.IsNullOrWhiteSpace(optOut.Status) ? "Unknown" : optOut.Status,
                                UniqueReference = optOut.UniqueReference,
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
                    FileName = $"{optOutConfiguration.OutputFolder}/{fileName}_Response.csv",
                    DocumentData = processedBytes
                };

                string saveDocument = await this.documentProcessingService.AddDocumentAsync(document);

                return saveDocument;
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
                    mappedOptOuts.Select(optout => new OptOutIdentifier
                    {
                        NhsNumber = optout.NhsNumber,
                        UniqueReference = optout.UniqueReference,
                        Status = optout.Status
                    }).ToList();

                var processedString = await this.csvMapperProcessingService
                       .MapObjectToCsvAsync(mappedOptOutIdentifiers, withHeader, shouldAddTrailingComma);

                string batchReference = this.dateTimeBroker.GetCurrentDateTimeOffset().ToString("yyyyMMddHHmmss");

                MeshMessage message = await this.meshProcessingService.SendMessageAsync(
                    mexTo: this.optOutConfiguration.To,
                    mexWorkflowId: this.optOutConfiguration.WorkflowId,
                    fileContent: Encoding.UTF8.GetBytes(processedString),
                    mexSubject: string.Empty,
                    mexLocalId: batchReference,
                    mexFileName: $"{batchReference}.txt",
                    mexContentChecksum: string.Empty,
                    contentType: "text/plain",
                    contentEncoding: string.Empty,
                    accept: "application/json");

                foreach (var optOut in mappedOptOuts)
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

                List<string> messageIds = await
                    this.meshProcessingService.RetrieveMessageIdsFromInboxAsync();

                List<MeshMessage> meshMessageList = new List<MeshMessage>();

                foreach (string messageId in messageIds)
                {
                    MeshMessage message = await meshProcessingService
                        .RetrieveAndAcknowledgeMessageByIdAsync(messageId);

                    meshMessageList.Add(message);

                    List<string> consentedStringList = Encoding.UTF8
                        .GetString(message.FileContent)
                            .Replace(",", string.Empty)
                                .Split("\r\n", StringSplitOptions.RemoveEmptyEntries).ToList();

                    List<OptOutIdentifier> consentedIdentifierList =
                        consentedStringList.Select(item => new OptOutIdentifier { NhsNumber = item }).ToList();

                    ValidateLocalIdHeaderExists(message);

                    string batchReference = GetHeaderValue(message, "Mex-LocalID");

                    ValidateBacthReferenceExists(batchReference);

                    List<OptOut> originalBatch = await this.optOutProcessingService
                        .RetrieveAllOptOutsByBatchReferenceAsync(batchReference);

                    List<string> consentedIdentifiers = consentedIdentifierList
                        .Select(optOut => optOut.NhsNumber)
                        .ToList();

                    List<OptOut> consentedList = originalBatch
                        .Where(optOut => consentedIdentifiers.Contains(optOut.NhsNumber)).ToList();

                    List<OptOut> nonConsentedList = originalBatch.Except(consentedList).ToList();
                    List<OptOut> delta = new List<OptOut>();

                    foreach (var item in consentedList)
                    {
                        if (item.Status != "Opt-In")
                        {
                            delta.Add(item);
                        }

                        var dateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();
                        item.UpdatedDate = dateTime;
                        item.CacheTime = dateTime;
                        item.LastSentToMesh = dateTime;
                        item.Status = "Opt-In";

                        await this.optOutProcessingService.AddOrModifyOptOutAsync(item);
                    }

                    foreach (var nonConsentedListItem in nonConsentedList)
                    {
                        if (nonConsentedListItem.Status != "Opt-Out")
                        {
                            delta.Add(nonConsentedListItem);
                        }

                        var dateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();
                        nonConsentedListItem.UpdatedDate = dateTime;
                        nonConsentedListItem.CacheTime = dateTime;
                        nonConsentedListItem.LastSentToMesh = dateTime;
                        nonConsentedListItem.Status = "Opt-Out";

                        await this.optOutProcessingService.AddOrModifyOptOutAsync(nonConsentedListItem);
                    }

                    if (delta.Count > 0)
                    {
                        List<OptOutIdentifier> differentIdentifiers = delta
                            .Select(identifier => new OptOutIdentifier
                            {
                                NhsNumber = identifier.NhsNumber,
                                UniqueReference = identifier.UniqueReference,
                                Status = identifier.Status,
                                StatusChangedDateTime = identifier.CacheTime
                            }).ToList();

                        string csvDifferences = await this.csvMapperProcessingService
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

                        await this.documentProcessingService.AddDocumentAsync(document);
                    }
                }

                return meshMessageList;
            });

        private static string GetHeaderValue(MeshMessage message, string keyToFind)
        {
            List<string> value = new List<string>();

            foreach (var key in message.Headers.Keys)
            {
                if (key.ToLower() == keyToFind.ToLower())
                {
                    message.Headers.TryGetValue(key, out value);

                    break;
                }
            }

            return value.FirstOrDefault();
        }
    }
}
