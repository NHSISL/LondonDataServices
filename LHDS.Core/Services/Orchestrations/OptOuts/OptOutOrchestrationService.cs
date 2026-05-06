// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Brokers.CsvHelpers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Files;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.TempLocations;
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
        private readonly ILoggingBroker loggingBroker;
        private readonly OptOutConfiguration optOutConfiguration;
        private readonly MeshConfiguration meshConfiguration;
        private readonly ITempLocationBroker tempLocationBroker;
        private readonly IFileBroker fileBroker;

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
            MeshConfiguration meshConfiguration,
            ITempLocationBroker tempLocationBroker,
            IFileBroker fileBroker)
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
            this.tempLocationBroker = tempLocationBroker;
            this.fileBroker = fileBroker;
        }

        public ValueTask<bool> ValidateMailboxAccessAsync(CancellationToken cancellationToken) =>
              TryCatch(async () =>
              {
                  cancellationToken.ThrowIfCancellationRequested();

                  return await meshProcessingService.ValidateMailboxAccessAsync(cancellationToken);
              });

        public ValueTask<string> RetrieveOptOutStatusAsync(
            Stream input,
            string fileName,
            CancellationToken cancellationToken) =>
            TryCatch(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                ValidateConfigurationSettings();
                ValidateArgumentsOnRetrieveOptOutStatus(input, fileName);

                bool withHeader = optOutConfiguration.OptOutFileHasHeader;
                Dictionary<string, int>? fieldMappings = null;
                bool shouldAddTrailingComma = optOutConfiguration.OptOutFileRequireTrailingComma;
                string csvTempFilePath = this.tempLocationBroker.GetUniqueHomeFilePath();
                var exceptions = new List<Exception>();
                bool isFirstRecord = true;

                try
                {
                    await using (FileStream writeStream = new FileStream(
                        csvTempFilePath,
                        FileMode.Create,
                        FileAccess.Write,
                        FileShare.None,
                        bufferSize: 4096,
                        useAsync: true))
                    {
                        await foreach (var optOut in this.csvHelperBroker
                            .MapCsvToObjectAsync<OptOutIdentifier>(
                                data: input,
                                hasHeaderRecord: withHeader,
                                fieldMappings: null,
                                headerValidated: false))
                        {
                            try
                            {
                                OptOut processedOptOut = await TryCatch(async () =>
                                {
                                    DateTimeOffset timeStamp =
                                        await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

                                    DateTimeOffset expirationDate =
                                        timeStamp.AddDays(-optOutConfiguration.ExpiredAfterDays);

                                    return await this.optOutProcessingService
                                        .RetrieveOrAddOptOutAsync(
                                            new OptOut
                                            {
                                                Id = await this.identifierBroker.GetIdentifierAsync(),
                                                NhsNumber = optOut.NhsNumber,
                                                Status = string.IsNullOrWhiteSpace(optOut.Status)
                                                    ? "Unknown"
                                                    : optOut.Status,
                                                UniqueReference = optOut.UniqueReference,
                                                CacheTime = expirationDate,
                                                CreatedDate = timeStamp,
                                                UpdatedDate = timeStamp,
                                                CreatedBy = "System",
                                                UpdatedBy = "System"
                                            });
                                });

                                var processedIdentifier = new OptOutIdentifier
                                {
                                    NhsNumber = processedOptOut.NhsNumber,
                                    UniqueReference = processedOptOut.UniqueReference,
                                    Status = processedOptOut.Status,
                                    StatusChangedDateTime = processedOptOut.CacheTime
                                };

                                await this.csvHelperBroker.MapObjectToCsvAsync(
                                    new List<OptOutIdentifier> { processedIdentifier },
                                    writeStream,
                                    addHeaderRecord: isFirstRecord && withHeader,
                                    fieldMappings: fieldMappings,
                                    shouldAddTrailingComma: shouldAddTrailingComma);

                                isFirstRecord = false;
                            }
                            catch (Exception ex)
                            {
                                exceptions.Add(ex);
                            }
                        }
                    }

                    if (exceptions.Any())
                    {
                        throw new AggregateException(
                            $"Unable to retrieve or add opt out for {exceptions.Count} mapped opt outs",
                            exceptions);
                    }

                    DateTimeOffset currentDateTimeOffset =
                        await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

                    string timeStamp = currentDateTimeOffset.ToString("yyyyMMddHHmmss");

                    string csvFileName = $"{optOutConfiguration.OutputFolder}/" +
                        $"{Path.GetFileNameWithoutExtension(fileName)}_{timeStamp}_Response.csv";

                    await using FileStream readStream = new FileStream(
                        csvTempFilePath,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.Read,
                        bufferSize: 4096,
                        useAsync: true);

                    await this.documentProcessingService.AddDocumentAsync(
                        input: readStream,
                        csvFileName,
                        container: blobContainers.OptOut);

                    return csvFileName;
                }
                finally
                {
                    await this.fileBroker.DeleteFileAsync(csvTempFilePath);
                }
            });

        public ValueTask<MeshMessage> PushExpiredOptOutsToMeshForRenewalAsync(CancellationToken cancellationToken) =>
          TryCatch(async () =>
          {
              cancellationToken.ThrowIfCancellationRequested();
              ValidateConfigurationSettings();

              // TODO: Consider implementing paging if the number of expired opt outs
              // is expected to be large to avoid memory issues
              List<string> expiredOptOutIdentifiers = await
                  this.optOutProcessingService
                      .RetrieveAllExpiredOptOutsAsync(optOutConfiguration.ExpiredAfterDays);

              if (!expiredOptOutIdentifiers.Any())
              {
                  return null;
              }

              DateTimeOffset batchReferenceDateTime =
                  await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

              string batchReference = batchReferenceDateTime.ToString("yyyyMMddHHmmss");
              string tempFilePath = this.tempLocationBroker.GetUniqueHomeFilePath();
              MeshMessage message;

              try
              {
                  await using (FileStream writeStream = new FileStream(
                      tempFilePath,
                      FileMode.Create,
                      FileAccess.Write,
                      FileShare.None,
                      bufferSize: 4096,
                      useAsync: true))
                  {
                      await using StreamWriter writer = new StreamWriter(
                          writeStream,
                          Encoding.UTF8,
                          bufferSize: 4096,
                          leaveOpen: true);

                      foreach (string identifier in expiredOptOutIdentifiers)
                      {
                          await writer.WriteLineAsync($"{identifier},");
                      }
                  }

                  await using FileStream readStream = new FileStream(
                      tempFilePath,
                      FileMode.Open,
                      FileAccess.Read,
                      FileShare.Read,
                      bufferSize: 4096,
                      useAsync: true);

                  message = await this.meshProcessingService.SendMessageAsync(
                      mexTo: this.optOutConfiguration.To,
                      mexWorkflowId: this.optOutConfiguration.WorkflowId,
                      content: readStream,
                      mexSubject: string.Empty,
                      mexLocalId: batchReference,
                      mexFileName: $"{batchReference}.txt",
                      mexContentChecksum: string.Empty,
                      contentType: "text/plain",
                      contentEncoding: string.Empty,
                      accept: "application/json",
                      cancellationToken: cancellationToken);
              }
              finally
              {
                  await this.fileBroker.DeleteFileAsync(tempFilePath);
              }

              //TODO:  Consider bulk update for better performance
              foreach (string identifier in expiredOptOutIdentifiers)
              {
                  DateTimeOffset dateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

                  OptOut modifiedOptOut = new OptOut
                  {
                      NhsNumber = identifier,
                      LastSentToMesh = dateTime,
                      UpdatedDate = dateTime,
                      BatchReference = batchReference
                  };

                  await this.optOutProcessingService.AddOrModifyOptOutAsync(modifiedOptOut);
              }

              return message;
          });

        public ValueTask<List<MeshMessage>> RetrieveUpdatedMeshConsentStatusesChangesAsync(
            CancellationToken cancellationToken) =>
            TryCatch(async () =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    ValidateConfigurationSettings();
                    bool withHeader = this.optOutConfiguration.OptOutFileHasHeader;
                    List<string> messageIds;
                    List<MeshMessage> meshMessageList = new List<MeshMessage>();
                    var exceptions = new List<Exception>();

                    while ((messageIds = await this.meshProcessingService
                        .RetrieveMessageIdsFromInboxAsync(cancellationToken)).Count > 0)
                    {
                        foreach (string messageId in messageIds)
                        {
                            try
                            {
                                MeshMessage returnedMessage = await TryCatch(async () =>
                                {
                                    string tempFilePath = this.tempLocationBroker.GetUniqueHomeFilePath();
                                    MeshMessage message;

                                    try
                                    {
                                        await using (FileStream outputStream = new FileStream(
                                            tempFilePath,
                                            FileMode.Create,
                                            FileAccess.Write,
                                            FileShare.None,
                                            bufferSize: 4096,
                                            useAsync: true))
                                        {
                                            message = await meshProcessingService
                                                .RetrieveMessageByIdAsync(
                                                    messageId,
                                                    outputStream,
                                                    cancellationToken);
                                        }

                                        if (GetKeyStringValue("mex-workflowid", message.Headers) !=
                                            this.optOutConfiguration.WorkflowId)
                                        {
                                            return null;
                                        }

                                        string[] delimiters = { "\r\n", "\n" };

                                        // TODO:  Refactor to stream and process line by line if file size is
                                        // expected to be large to avoid memory issues
                                        string fileContent;

                                        await using (FileStream readStream = new FileStream(
                                            tempFilePath,
                                            FileMode.Open,
                                            FileAccess.Read,
                                            FileShare.Read,
                                            bufferSize: 4096,
                                            useAsync: true))
                                        {
                                            using StreamReader reader = new StreamReader(readStream, Encoding.UTF8);
                                            fileContent = await reader.ReadToEndAsync();
                                        }

                                        List<string> consentedIdentifiers = fileContent
                                            .Replace(",", string.Empty)
                                                .Split(delimiters, StringSplitOptions.RemoveEmptyEntries)
                                                    .ToList();

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

                                            string fileName = $"{optOutConfiguration.OutputFolder}/" +
                                                $"{batchReference}_DeltaResponse.csv";

                                            string outputTempPath = this.tempLocationBroker.GetUniqueHomeFilePath();

                                            try
                                            {
                                                await using (FileStream writeStream = new FileStream(
                                                    outputTempPath,
                                                    FileMode.Create,
                                                    FileAccess.Write,
                                                    FileShare.None,
                                                    bufferSize: 4096,
                                                    useAsync: true))
                                                {
                                                    await this.csvHelperBroker.MapObjectToCsvAsync(
                                                        @object: differentIdentifiers,
                                                        outputStream: writeStream,
                                                        addHeaderRecord: this.optOutConfiguration.OptOutFileHasHeader,
                                                        shouldAddTrailingComma: this.optOutConfiguration
                                                            .OptOutFileRequireTrailingComma);
                                                }

                                                ValidateDocumentRequirements(fileName);

                                                await using FileStream csvReadStream = new FileStream(
                                                    outputTempPath,
                                                    FileMode.Open,
                                                    FileAccess.Read,
                                                    FileShare.Read,
                                                    bufferSize: 4096,
                                                    useAsync: true);

                                                await this.documentProcessingService.AddDocumentAsync(
                                                    csvReadStream, fileName, container: blobContainers.OptOut);
                                            }
                                            finally
                                            {
                                                await this.fileBroker.DeleteFileAsync(outputTempPath);
                                            }
                                        }

                                        await this.meshProcessingService
                                            .AcknowledgeMessageByIdAsync(messageId, cancellationToken);

                                        return message;
                                    }
                                    finally
                                    {
                                        await this.fileBroker.DeleteFileAsync(tempFilePath);
                                    }
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
    }
}
