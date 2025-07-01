// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Files;
using LHDS.Core.Brokers.Hashing;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Orchestrations.EmisLandings;
using LHDS.Core.Services.Orchestrations.TppLandings;
using LHDS.Core.Services.Processings.DataSetSpecifications;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.IngestionTrackingAudits;
using LHDS.Core.Services.Processings.IngestionTrackings;

namespace LHDS.Core.Services.Orchestrations.Tpp
{
    public partial class TppLandingOrchestrationService : ITppLandingOrchestrationService
    {
        private readonly IDocumentProcessingService documentProcessingService;
        private readonly IIngestionTrackingProcessingService ingestionTrackingProcessingService;
        private readonly IIngestionTrackingAuditProcessingService ingestionTrackingProcessingAuditService;
        private readonly BlobContainers blobContainers;
        private readonly IDataSetSpecificationProcessingService dataSetSpecificationProcessingService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIdentifierBroker identifierBroker;
        private readonly IHashBroker hashBroker;
        private readonly IFileBroker fileBroker;
        private readonly LandingConfiguration landingConfiguration;

        public TppLandingOrchestrationService(
            IDocumentProcessingService documentProcessingService,
            IIngestionTrackingProcessingService ingestionTrackingProcessingService,
            IIngestionTrackingAuditProcessingService ingestionTrackingProcessingAuditService,
            IDataSetSpecificationProcessingService dataSetSpecificationProcessingService,
            BlobContainers blobContainers,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker,
            IIdentifierBroker identifierBroker,
            IHashBroker hashBroker,
            IFileBroker fileBroker,
            LandingConfiguration landingConfiguration)
        {
            this.documentProcessingService = documentProcessingService;
            this.ingestionTrackingProcessingService = ingestionTrackingProcessingService;
            this.ingestionTrackingProcessingAuditService = ingestionTrackingProcessingAuditService;
            this.dataSetSpecificationProcessingService = dataSetSpecificationProcessingService;
            this.blobContainers = blobContainers;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.identifierBroker = identifierBroker;
            this.hashBroker = hashBroker;
            this.fileBroker = fileBroker;
            this.landingConfiguration = landingConfiguration;
        }

        public ValueTask<Guid> ProcessAsync(string fileName, Guid supplierId) =>
            TryCatch(async () =>
            {
                ValidateArgumentsOnProcess(fileName, supplierId);

                return await this.ProcessFileAsync(fileName, supplierId);
            });

        public ValueTask<List<Guid>> ReProcessAsync(Guid supplierId) =>
            TryCatch(async () =>
            {
                ValidateArgumentsOnReProcess(supplierId);

                IQueryable<IngestionTracking> allIngestionTrackings =
                    await this.ingestionTrackingProcessingService.RetrieveAllIngestionTrackingsAsync();

                IQueryable<IngestionTracking> filteredIngestionTrackings = allIngestionTrackings
                    .Where(ingestionTracking =>
                        ingestionTracking.SupplierId == supplierId
                        && ingestionTracking.IsDownloaded == false
                        && ingestionTracking.RetryCount < 4);

                List<IngestionTracking> ingestionTrackingsToProcess = filteredIngestionTrackings.ToList();
                List<Guid> ingestionTrackingsProcessed = new List<Guid>();

                foreach (IngestionTracking ingestionTracking in ingestionTrackingsToProcess)
                {
                    try
                    {
                        await this.ProcessFileAsync(ingestionTracking.FileName, supplierId);
                        ingestionTrackingsProcessed.Add(ingestionTracking.Id);
                    }
                    catch (Exception exception)
                    {
                        await this.loggingBroker.LogErrorAsync(exception);
                    }
                }

                return ingestionTrackingsProcessed;
            });


        virtual internal async ValueTask<Guid> ProcessFileAsync(string fileName, Guid supplierId)
        {
            ValidateArgumentsOnProcess(fileName, supplierId);

            IQueryable<IngestionTracking> allIngestionTrackings =
                await this.ingestionTrackingProcessingService.RetrieveAllIngestionTrackingsAsync();

            IngestionTracking? maybeIngestionTracking = allIngestionTrackings
                    .FirstOrDefault(ingestionTracking => ingestionTracking.FileName == fileName);

            if (maybeIngestionTracking == null)
            {
                var currentDateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

                DataSetSpecification retrievedDataSetSpecification = await
                    this.dataSetSpecificationProcessingService.GetActiveDataSetSpecification(supplierId);

                var filename = fileName.StartsWith('/')
                    ? fileName
                    : "/" + fileName;

                string[] segments = filename.Split('/');

                if (segments.Length < 4)
                {
                    throw new InvalidOperationException(
                        "The input string does not contain enough segments.  " +
                        "Expected to see a path in the format:  " +
                        "/{reporting group}/{manifest time}/{file}, but found " +
                        $"{filename}");
                }

                string objectName = Path.GetFileNameWithoutExtension(filename);
                string sourceFolderPath = Path.GetDirectoryName(filename) ?? string.Empty;
                sourceFolderPath = sourceFolderPath.Replace("\\", "/").Replace("\\", "/");
                string dataSetName = retrievedDataSetSpecification?.DataSet?.DataSetName ?? string.Empty;
                string dataSetVersion = retrievedDataSetSpecification?.OurSpecificationVersion ?? string.Empty;
                string extractGroup = segments[1];
                string batch = segments[2];
                string file = segments[3];

                string extractTime = DateTime.ParseExact(segments[2], "yyyyMMdd_HHmm", null)
                    .ToString("yyyyMMddHHmmss");

                string baseFolder =
                    $"/{landingConfiguration.DecryptedFolder}" +
                    $"/{dataSetName}" +
                    $"/{dataSetVersion}" +
                    $"/{extractGroup}" +
                    $"/{extractTime}";

                var decryptedFileName =
                    $"{baseFolder}"
                    + $"/{file}";

                IngestionTracking newIngestionTracking =
                    new IngestionTracking
                    {
                        Id = await this.identifierBroker.GetIdentifierAsync(),
                        SupplierId = supplierId,
                        FileName = filename,
                        SourceFolderPath = sourceFolderPath,
                        BatchReadyFolderPath = baseFolder,
                        Batch = batch,
                        IsBatchComplete = false,
                        ObjectName = objectName,
                        DataSetSpecificationId = retrievedDataSetSpecification.Id,
                        EncryptedFileName = "Not Encrypted",
                        EncryptedFileSize = 0,
                        EncryptedFileSha256Hash = string.Empty,
                        DecryptedFileName = decryptedFileName,
                        Decrypted = true,
                        DecryptedFileSize = 0,
                        DecryptedFileSha256Hash = string.Empty,
                        LastSeen = currentDateTime,
                        LastAttempt = currentDateTime,
                        FileDeleted = false,
                        IsDownloaded = false,
                        RetryCount = 0,
                    };

                maybeIngestionTracking = await this.ingestionTrackingProcessingService
                    .AddIngestionTrackingAsync(newIngestionTracking);

                await LogAudit(
                    ingestionTracking: newIngestionTracking,
                    message:
                        $"New file found '{newIngestionTracking.FileName}',  " +
                        $"created item with Id: {newIngestionTracking.Id}");
            }

            if (maybeIngestionTracking.IsDownloaded == false && maybeIngestionTracking.RetryCount <= 3)
            {
                maybeIngestionTracking.RetryCount += 1;

                await LogAudit(
                    ingestionTracking: maybeIngestionTracking,
                    message:
                        $"Processing file '{maybeIngestionTracking.FileName}' " +
                        $"associated with Id: {maybeIngestionTracking.Id}." + Environment.NewLine +
                        $"Downloading: {maybeIngestionTracking.FileName} " + Environment.NewLine +
                        $"RetryCount: {maybeIngestionTracking.RetryCount}");

                try
                {
                    string batchReadyFileName =
                        $"{maybeIngestionTracking.BatchReadyFolderPath}/{landingConfiguration.BatchReadyFile}"
                            .Replace("\\", "/");

                    await LogAudit(
                        ingestionTracking: maybeIngestionTracking,
                        message:
                            $"Removing batch ready file '{batchReadyFileName}' as this file will invalidate the " +
                            $"ready status for batch: {maybeIngestionTracking.Batch}.");

                    await this.documentProcessingService.RemoveDocumentByFileNameAsync(
                        batchReadyFileName,
                        this.blobContainers.Ingress);
                }
                catch (Exception)
                { }

                var currentDateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                maybeIngestionTracking.IsDownloaded = false;
                maybeIngestionTracking.IsBatchComplete = false;
                maybeIngestionTracking.FileDeleted = false;
                maybeIngestionTracking.LastSeen = currentDateTime;

                await LogAudit(
                    maybeIngestionTracking,
                        $"Moving file '{maybeIngestionTracking.FileName}' to " +
                            $"'{maybeIngestionTracking.DecryptedFileName}'." +
                                Environment.NewLine + $"RetryCount: {maybeIngestionTracking.RetryCount}");

                string tempFilePath = await this.fileBroker.GetTempFileName();

                try
                {
                    using (FileStream writeFileStream =
                        new FileStream(tempFilePath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        await this.documentProcessingService.RetrieveDocumentByFileNameAsync(
                            output: writeFileStream,
                            fileName: maybeIngestionTracking.FileName,
                            container: blobContainers.TppLanding);
                    }

                    using (FileStream readFileStream =
                        new FileStream(tempFilePath, FileMode.Open, FileAccess.ReadWrite))
                    {
                        string decryptedFileSha256Hash =
                            await this.hashBroker.GenerateSha256HashAsync(data: readFileStream);

                        maybeIngestionTracking.DecryptedFileSize = readFileStream.Length;
                        maybeIngestionTracking.DecryptedFileSha256Hash = decryptedFileSha256Hash;

                        await this.documentProcessingService.AddDocumentAsync(
                            input: readFileStream,
                            fileName: maybeIngestionTracking.DecryptedFileName,
                            container: blobContainers.Ingress);
                    }

                    await LogAudit(
                        maybeIngestionTracking,
                        $"Received and updated file from TPP which has now been uploaded " +
                            $"to the blob storage '{maybeIngestionTracking.DecryptedFileName}'");

                    maybeIngestionTracking.IsDownloaded = true;

                    await this.ingestionTrackingProcessingService.ModifyIngestionTrackingAsync(
                        maybeIngestionTracking);
                }
                catch (Exception)
                {
                    await LogAudit(
                        maybeIngestionTracking,
                        $"Unable to process file. File could not be uploaded " +
                            $"to the blob storage '{maybeIngestionTracking.DecryptedFileName}'");

                    maybeIngestionTracking.IsDownloaded = false;

                    await this.ingestionTrackingProcessingService.ModifyIngestionTrackingAsync(
                        maybeIngestionTracking);

                    throw;
                }
                finally
                {
                    await this.fileBroker.DeleteFileAsync(tempFilePath);
                }
            }

            return maybeIngestionTracking.Id;
        }

        virtual internal async ValueTask LogAudit(
           IngestionTracking ingestionTracking,
           string message)
        {
            var currentDateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

            IngestionTrackingAudit newAudit =
                new IngestionTrackingAudit
                {
                    Id = Guid.NewGuid(),
                    IngestionTrackingId = ingestionTracking.Id,
                    Message = $"{message} document",
                    CreatedBy = "TppOrchestrationService",
                    CreatedDate = currentDateTime,
                    UpdatedBy = "TppOrchestrationService",
                    UpdatedDate = currentDateTime
                };

            await this.ingestionTrackingProcessingAuditService.AddIngestionTrackingAuditAsync(newAudit);
        }
    }
}