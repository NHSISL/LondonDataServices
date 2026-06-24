// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Services.Orchestrations.Tpp;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.TppLandings
{
    public partial class TppLandingOrchestrationTests
    {
        [Fact]
        public async Task ShouldNotProcessExistingDocumentOnProcessFileAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            string randomHash = GetRandomString(64);
            int randomNumber = GetRandomNumber();
            Guid randomSupplierId = Guid.NewGuid();
            Guid inputSupplierId = randomSupplierId;
            List<string> randomFileNames = GetRandomStrings();
            randomFileNames = randomFileNames.Select(name => name.StartsWith("/") ? name : "/" + name).ToList();
            string randomFileName = randomFileNames.Last();
            string inputFileName = randomFileName;
            Stream randomData = new MemoryStream(Encoding.UTF8.GetBytes(GetRandomString()));
            Stream inputData = randomData;

            List<IngestionTracking> randomIngestionTrackings =
                CreateRandomIngestionTrackings(
                    dateTimeOffset: randomDateTime,
                    fileNames: randomFileNames,
                    supplierId: randomSupplierId);

            IngestionTracking randomIngestionTracking = randomIngestionTrackings.Last();
            randomIngestionTracking.DecryptedFileSha256Hash = randomHash;
            randomIngestionTracking.FileName = inputFileName;

            var tppOrchestrationServiceMock = new Mock<TppLandingOrchestrationService>(
                documentProcessingServiceMock.Object,
                ingestionTrackingProcessingServiceMock.Object,
                ingestionTrackingProcessingAuditServiceMock.Object,
                dataSetSpecificationProcessingServiceMock.Object,
                subscriberAgreementProcessingServiceMock.Object,
                blobContainers,
                loggingBrokerMock.Object,
                dateTimeBrokerMock.Object,
                identifierBrokerMock.Object,
                landingConfiguration)
            {
                CallBase = true
            };

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackingsAsync())
                    .ReturnsAsync(randomIngestionTrackings.AsQueryable());

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                   .ReturnsAsync(randomDateTime);

            // when
            ValueTask<Guid> returnedGuid = tppOrchestrationServiceMock.Object.ProcessFileAsync(
                fileName: inputFileName,
                supplierId: inputSupplierId);

            // then
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackingsAsync(),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingAuditServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }

        [Fact]
        public async Task ShouldProcessNewDocumentAndAddOnProcessFileAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Guid randomGuid = Guid.NewGuid();
            Guid randomSupplierId = Guid.NewGuid();
            Guid inputSupplierId = randomSupplierId;
            DataSet randomDataSet = CreateRandomDataSet(supplierId: randomSupplierId);
            int randomNumber = GetRandomNumber();
            string resourceGroup = GetRandomString();
            string batch = randomDateTime.ToString("yyyyMMdd_HHmm");
            string extractTime = DateTime.ParseExact(batch, "yyyyMMdd_HHmm", null).ToString("yyyyMMddHHmmss");
            string objectName = GetRandomString();
            string inputFileName = $"/{resourceGroup}/{batch}/{objectName}.csv";
            string sourceFolderPath = $"/{resourceGroup}/{batch}";

            SubscriberAgreement randomSubscriberAgreement = new SubscriberAgreement
            {
                Id = randomGuid,
                SupplierId = randomSupplierId,
                SupplierSharingAgreementShortName = resourceGroup,
                IsActive = true,
            };

            SubscriberAgreement outputSubscriberAgreement = randomSubscriberAgreement.DeepClone();

            var tppOrchestrationServiceMock = new Mock<TppLandingOrchestrationService>(
                documentProcessingServiceMock.Object,
                ingestionTrackingProcessingServiceMock.Object,
                ingestionTrackingProcessingAuditServiceMock.Object,
                dataSetSpecificationProcessingServiceMock.Object,
                subscriberAgreementProcessingServiceMock.Object,
                blobContainers,
                loggingBrokerMock.Object,
                dateTimeBrokerMock.Object,
                identifierBrokerMock.Object,
                landingConfiguration)
            {
                CallBase = true
            };

            tppOrchestrationServiceMock.Setup(service =>
                service.LogAudit(
                    It.IsAny<IngestionTracking>(), It.IsAny<string>()))
                        .Returns(ValueTask.CompletedTask);

            List<string> randomFileNames =
                GetRandomTppFileNames(resourceGroup, batch, count: randomNumber);

            List<string> inputFileNames = randomFileNames;
            byte[] randomData = CreateRandomData();
            byte[] inputData = randomData;

            DataSetSpecification randomDataSetSpecification =
               CreateRandomDataSetSpecification(dataSet: randomDataSet);

            randomDataSetSpecification.DataSetId = randomDataSet.Id;

            string basePath =
                $"/{landingConfiguration.DecryptedFolder}"
                + $"/{randomDataSet.DataSetName}"
                + $"/{randomDataSetSpecification.OurSpecificationVersion}"
                + $"/{resourceGroup}"
                + $"/{extractTime}";

            string decryptedFileName =
                $"{basePath}"
                + $"/{objectName}.csv";

            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking(randomDateTime);
            randomIngestionTracking.DataSetSpecificationId = randomDataSetSpecification.Id;
            randomIngestionTracking.ObjectName = objectName;
            randomIngestionTracking.BatchReadyFolderPath = basePath;
            randomIngestionTracking.SubscriberAgreementId = null;

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackingsAsync())
                    .ReturnsAsync(new List<IngestionTracking>().AsQueryable());

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTime);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ReturnsAsync(randomGuid);

            this.dataSetSpecificationProcessingServiceMock.Setup(service =>
               service.GetActiveDataSetSpecification(randomSupplierId))
                   .Returns(ValueTask.FromResult(randomDataSetSpecification));

            this.subscriberAgreementProcessingServiceMock.Setup(service =>
                service.RetrieveOrAddSubscriberAgreementByNameAsync(It.Is(SameSubscriberAgreementAs(randomSubscriberAgreement))))
                    .ReturnsAsync(outputSubscriberAgreement);

            IngestionTracking newIngestionTracking =
                new IngestionTracking
                {
                    Id = randomGuid,
                    SupplierId = randomSupplierId,
                    SubscriberAgreementId = randomSubscriberAgreement.Id,
                    FileName = inputFileName,
                    SourceFolderPath = sourceFolderPath,
                    BatchReadyFolderPath = basePath,
                    Batch = batch,
                    IsBatchComplete = false,
                    ObjectName = objectName,
                    DataSetSpecificationId = randomDataSetSpecification.Id,
                    EncryptedFileName = "Not Encrypted",
                    EncryptedFileSize = 0,
                    EncryptedFileSha256Hash = string.Empty,
                    DecryptedFileName = decryptedFileName,
                    Decrypted = true,
                    DecryptedFileSize = 0,
                    DecryptedFileSha256Hash = string.Empty,
                    LastSeen = randomDateTime,
                    LastAttempt = randomDateTime,
                    FileDeleted = false,
                    IsDownloaded = false,
                    RetryCount = 0,
                };

            IngestionTracking storageIngestionTracking = newIngestionTracking.DeepClone();

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                    service.AddIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(newIngestionTracking))))
                       .ReturnsAsync(storageIngestionTracking);

            IngestionTracking modifiedIngestionTracking = storageIngestionTracking.DeepClone();
            modifiedIngestionTracking.RetryCount += 1;

            string batchReadyFileName =
                $"{modifiedIngestionTracking.BatchReadyFolderPath}/{landingConfiguration.BatchReadyFile}"
                    .Replace("\\", "/");

            this.documentProcessingServiceMock.Setup(service =>
                service.RemoveDocumentByFileNameAsync(batchReadyFileName, this.blobContainers.Ingress))
                   .Returns(ValueTask.CompletedTask);

            modifiedIngestionTracking.IsDownloaded = false;
            modifiedIngestionTracking.IsBatchComplete = false;
            modifiedIngestionTracking.FileDeleted = false;
            modifiedIngestionTracking.LastSeen = randomDateTime;

            string expectedSha256Hash =
                Convert.ToHexString(SHA256.HashData(inputData)).ToLowerInvariant();

            modifiedIngestionTracking.DecryptedFileSha256Hash = expectedSha256Hash;
            modifiedIngestionTracking.DecryptedFileSize = inputData.Length;
            Stream blobStream = new MemoryStream(inputData);

            documentProcessingServiceMock.Setup(service =>
                service.RetrieveDocumentStreamByFileNameAsync(
                    modifiedIngestionTracking.FileName,
                    blobContainers.TppLanding))
                        .ReturnsAsync(blobStream);

            documentProcessingServiceMock
                .Setup(service =>
                    service.AddDocumentAsync(
                        It.IsAny<Stream>(),
                        modifiedIngestionTracking.DecryptedFileName,
                        blobContainers.Ingress))
                .Callback<Stream, string, string>((input, _, _) =>
                {
                    byte[] buffer = new byte[81920];
                    while (input.Read(buffer, 0, buffer.Length) > 0) { }
                })
                .Returns(ValueTask.CompletedTask);

            modifiedIngestionTracking.IsDownloaded = true;

            // when
            Guid returnedGuid = await tppOrchestrationServiceMock.Object.ProcessFileAsync(
                fileName: inputFileName,
                supplierId: inputSupplierId);

            // then
            this.ingestionTrackingProcessingServiceMock.Verify(service =>
               service.RetrieveAllIngestionTrackingsAsync(),
                   Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(2));

            this.dataSetSpecificationProcessingServiceMock.Verify(service =>
                service.GetActiveDataSetSpecification(randomSupplierId),
                    Times.Once);

            this.subscriberAgreementProcessingServiceMock.Verify(service =>
                service.RetrieveOrAddSubscriberAgreementByNameAsync(
                    It.Is(SameSubscriberAgreementAs(randomSubscriberAgreement))),
                        Times.Once);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Exactly(2));

            tppOrchestrationServiceMock.Verify(service =>
                service.LogAudit(
                    It.IsAny<IngestionTracking>(),
                    $"New file found '{storageIngestionTracking.FileName}',  " +
                        $"created item with Id: {storageIngestionTracking.Id}"),
                            Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.AddIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(newIngestionTracking))),
                    Times.Once);

            tppOrchestrationServiceMock.Verify(service =>
                service.LogAudit(
                    It.Is(SameIngestionTrackingAs(modifiedIngestionTracking)),
                    $"Processing file '{modifiedIngestionTracking.FileName}' " +
                        $"associated with Id: {modifiedIngestionTracking.Id}." + Environment.NewLine +
                            $"Downloading: {modifiedIngestionTracking.FileName} " + Environment.NewLine +
                                $"RetryCount: {modifiedIngestionTracking.RetryCount}"),
                                    Times.Once);

            tppOrchestrationServiceMock.Verify(service =>
                service.LogAudit(
                    It.Is(SameIngestionTrackingAs(modifiedIngestionTracking)),
                    $"Removing batch ready file '{batchReadyFileName}' as this file will invalidate the " +
                        $"ready status for batch: {modifiedIngestionTracking.Batch}."),
                            Times.Once);

            this.documentProcessingServiceMock.Verify(service =>
                service.RemoveDocumentByFileNameAsync(batchReadyFileName, this.blobContainers.Ingress),
                   Times.Once);

            tppOrchestrationServiceMock.Verify(service =>
                service.LogAudit(
                    It.Is(SameIngestionTrackingAs(modifiedIngestionTracking)),
                    $"Moving file '{modifiedIngestionTracking.FileName}' to " +
                        $"'{modifiedIngestionTracking.DecryptedFileName}'." +
                            Environment.NewLine + $"RetryCount: {modifiedIngestionTracking.RetryCount}"),
                                Times.Once);

            documentProcessingServiceMock.Verify(service =>
                service.RetrieveDocumentStreamByFileNameAsync(
                    modifiedIngestionTracking.FileName,
                    blobContainers.TppLanding),
                        Times.Once);

            documentProcessingServiceMock.Verify(service =>
                service.AddDocumentAsync(
                    It.IsAny<Stream>(),
                    modifiedIngestionTracking.DecryptedFileName,
                    blobContainers.Ingress),
                        Times.Once);

            tppOrchestrationServiceMock.Verify(service =>
                service.LogAudit(
                    It.Is(SameIngestionTrackingAs(modifiedIngestionTracking)),
                    $"Received and updated file from TPP which has now been uploaded " +
                        $"to the blob storage '{modifiedIngestionTracking.DecryptedFileName}'"),
                            Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.ModifyIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(modifiedIngestionTracking))),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldProcessExisitingDocumentIfUpdatedUpdateHashOnProcessFileAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            int randomNumber = GetRandomNumber();
            Guid randomSupplierId = Guid.NewGuid();
            List<string> randomFileNames = GetRandomStrings();
            randomFileNames = randomFileNames.Select(name => name.StartsWith("/") ? name : "/" + name).ToList();
            string randomFileName = randomFileNames.Last();
            string inputFileName = randomFileName;
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputFileName);

            var tppOrchestrationServiceMock = new Mock<TppLandingOrchestrationService>(
                documentProcessingServiceMock.Object,
                ingestionTrackingProcessingServiceMock.Object,
                ingestionTrackingProcessingAuditServiceMock.Object,
                dataSetSpecificationProcessingServiceMock.Object,
                subscriberAgreementProcessingServiceMock.Object,
                blobContainers,
                loggingBrokerMock.Object,
                dateTimeBrokerMock.Object,
                identifierBrokerMock.Object,
                landingConfiguration)
            {
                CallBase = true
            };

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTime);

            tppOrchestrationServiceMock.Setup(service =>
                service.LogAudit(
                    It.IsAny<IngestionTracking>(), It.IsAny<string>()))
                        .Returns(ValueTask.CompletedTask);

            List<IngestionTracking> randomIngestionTrackings =
                CreateRandomIngestionTrackings(
                    dateTimeOffset: randomDateTime,
                    fileNames: randomFileNames,
                    supplierId: randomSupplierId);

            IngestionTracking randomIngestionTracking = randomIngestionTrackings.Last();
            randomIngestionTracking.FileName = inputFileName;
            randomIngestionTracking.RetryCount = 1;
            randomIngestionTracking.IsDownloaded = false;
            randomIngestionTracking.DecryptedFileSize = inputBytes.Length;
            IngestionTracking storageIngestionTracking = randomIngestionTracking;
            IngestionTracking modifiedIngestionTracking = storageIngestionTracking.DeepClone();
            modifiedIngestionTracking.RetryCount += 1;

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackingsAsync())
                    .ReturnsAsync(randomIngestionTrackings.AsQueryable());

            string batchReadyFileName =
                $"{modifiedIngestionTracking.BatchReadyFolderPath}/{landingConfiguration.BatchReadyFile}"
                    .Replace("\\", "/");

            this.documentProcessingServiceMock.Setup(service =>
                service.RemoveDocumentByFileNameAsync(batchReadyFileName, this.blobContainers.Ingress))
                   .Returns(ValueTask.CompletedTask);

            modifiedIngestionTracking.IsDownloaded = false;
            modifiedIngestionTracking.IsBatchComplete = false;
            modifiedIngestionTracking.FileDeleted = false;
            modifiedIngestionTracking.LastSeen = randomDateTime;

            string expectedSha256Hash =
                Convert.ToHexString(SHA256.HashData(inputBytes)).ToLowerInvariant();

            modifiedIngestionTracking.DecryptedFileSha256Hash = expectedSha256Hash;
            modifiedIngestionTracking.DecryptedFileSize = inputBytes.Length;
            Stream blobStream = new MemoryStream(inputBytes);

            documentProcessingServiceMock.Setup(service =>
                service.RetrieveDocumentStreamByFileNameAsync(
                    modifiedIngestionTracking.FileName,
                    blobContainers.TppLanding))
                        .ReturnsAsync(blobStream);

            documentProcessingServiceMock
                .Setup(service =>
                    service.AddDocumentAsync(
                        It.IsAny<Stream>(),
                        modifiedIngestionTracking.DecryptedFileName,
                        blobContainers.Ingress))
                .Callback<Stream, string, string>((input, _, _) =>
                {
                    byte[] buffer = new byte[81920];
                    while (input.Read(buffer, 0, buffer.Length) > 0) { }
                })
                .Returns(ValueTask.CompletedTask);

            modifiedIngestionTracking.IsDownloaded = true;
            modifiedIngestionTracking.Decrypted = true;

            // when
            ValueTask<Guid> returnedGuid = tppOrchestrationServiceMock.Object
                .ProcessFileAsync(fileName: inputFileName, supplierId: randomSupplierId);

            // then
            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackingsAsync(),
                    Times.Once);

            tppOrchestrationServiceMock.Verify(service =>
                service.LogAudit(
                    It.IsAny<IngestionTracking>(),
                    $"Processing file '{modifiedIngestionTracking.FileName}' " +
                        $"associated with Id: {modifiedIngestionTracking.Id}." + Environment.NewLine +
                            $"Downloading: {modifiedIngestionTracking.FileName} " + Environment.NewLine +
                                $"RetryCount: {modifiedIngestionTracking.RetryCount}"),
                                    Times.Once);

            tppOrchestrationServiceMock.Verify(service =>
                service.LogAudit(
                    It.IsAny<IngestionTracking>(),
                    $"Removing batch ready file '{batchReadyFileName}' as this file will invalidate the " +
                        $"ready status for batch: {modifiedIngestionTracking.Batch}."),
                            Times.Once);

            this.documentProcessingServiceMock.Verify(service =>
                service.RemoveDocumentByFileNameAsync(batchReadyFileName, this.blobContainers.Ingress),
                   Times.Once);

            tppOrchestrationServiceMock.Verify(service =>
                service.LogAudit(
                    It.IsAny<IngestionTracking>(),
                    $"Moving file '{modifiedIngestionTracking.FileName}' to " +
                        $"'{modifiedIngestionTracking.DecryptedFileName}'." +
                            Environment.NewLine + $"RetryCount: {modifiedIngestionTracking.RetryCount}"),
                                Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            documentProcessingServiceMock.Verify(service =>
                service.RetrieveDocumentStreamByFileNameAsync(
                    modifiedIngestionTracking.FileName,
                    blobContainers.TppLanding),
                        Times.Once);

            documentProcessingServiceMock.Verify(service =>
                service.AddDocumentAsync(
                    It.IsAny<Stream>(),
                    modifiedIngestionTracking.DecryptedFileName,
                    blobContainers.Ingress),
                        Times.Once);

            tppOrchestrationServiceMock.Verify(service =>
                service.LogAudit(
                    It.IsAny<IngestionTracking>(),
                    $"Received and updated file from TPP which has now been uploaded " +
                        $"to the blob storage '{modifiedIngestionTracking.DecryptedFileName}'"),
                            Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.ModifyIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(modifiedIngestionTracking))),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}