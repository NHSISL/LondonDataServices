// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Services.Orchestrations.Tpp;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.TppLandings
{
    public partial class TppLandingOrchestrationTests
    {
        [Fact]
        public async Task ShouldNotProcessExistingDocumentAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            string randomHash = GetRandomString(64);
            int randomNumber = GetRandomNumber();
            Guid randomSupplierId = Guid.NewGuid();
            Guid inputSupplierId = randomSupplierId;
            List<string> randomFileNames = GetRandomStrings();
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

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackingsAsync())
                    .ReturnsAsync(randomIngestionTrackings.AsQueryable());

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                   .ReturnsAsync(randomDateTime);

            this.hashBrokerMock.Setup(broker => broker.GenerateSha256HashAsync(inputData))
                .ReturnsAsync(randomHash);

            // when
            ValueTask<Guid> returnedGuid = this.tppOrchestrationService.ProcessAsync(
                input: inputData,
                fileName: inputFileName,
                supplierId: inputSupplierId);

            // then
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackingsAsync(),
                    Times.Once);

            this.hashBrokerMock.Verify(broker =>
                broker.GenerateSha256HashAsync(inputData),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingAuditServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }

        [Fact]
        public async Task ShouldProcessNewDocumentAndAddAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Guid randomGuid = Guid.NewGuid();
            Guid randomSupplierId = Guid.NewGuid();
            Guid inputSupplierId = randomSupplierId;
            DataSet randomDataSet = CreateRandomDataSet(supplierId: randomSupplierId);
            string randomHash = GetRandomString(64);
            int randomNumber = GetRandomNumber();
            string resourceGroup = GetRandomString();
            string batch = randomDateTime.ToString("yyyyMMdd_HHmm");
            string extractTime = DateTime.ParseExact(batch, "yyyyMMdd_HHmm", null).ToString("yyyyMMddHHmmss");
            string objectName = GetRandomString();
            string inputFileName = $"/{resourceGroup}/{batch}/{objectName}.csv";
            string sourceFolderPath = $"/{resourceGroup}/{batch}";

            var tppOrchestrationServiceMock = new Mock<TppLandingOrchestrationService>(
                documentProcessingServiceMock.Object,
                ingestionTrackingProcessingServiceMock.Object,
                ingestionTrackingProcessingAuditServiceMock.Object,
                dataSetSpecificationProcessingServiceMock.Object,
                blobContainers,
                loggingBrokerMock.Object,
                dateTimeBrokerMock.Object,
                identifierBrokerMock.Object,
                hashBrokerMock.Object,
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
            Stream inputDataStream = new MemoryStream(inputData);

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
            IngestionTracking storageIngestionTracking = randomIngestionTracking;
            IngestionTracking updatedIngestionTracking = storageIngestionTracking.DeepClone();

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackingsAsync())
                    .ReturnsAsync(new List<IngestionTracking>().AsQueryable());

            this.hashBrokerMock.Setup(broker =>
                broker.GenerateSha256HashAsync(inputDataStream))
                    .ReturnsAsync(randomHash);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTime);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ReturnsAsync(randomGuid);

            this.dataSetSpecificationProcessingServiceMock.Setup(service =>
               service.GetActiveDataSetSpecification(randomSupplierId))
                   .Returns(ValueTask.FromResult(randomDataSetSpecification));

            IngestionTracking newIngestionTracking =
                new IngestionTracking
                {
                    Id = randomGuid,
                    SupplierId = randomSupplierId,
                    FileName = inputFileName,
                    SourceFolderPath = sourceFolderPath,
                    BatchReadyFolderPath = basePath,
                    Batch = batch,
                    ObjectName = objectName,
                    DataSetSpecificationId = randomDataSetSpecification.Id,
                    EncryptedFileName = "Not Encrypted",
                    EncryptedFileSize = 0,
                    EncryptedFileSha256Hash = string.Empty,
                    DecryptedFileName = decryptedFileName,
                    Decrypted = true,
                    DecryptedFileSize = inputData.Length,
                    DecryptedFileSha256Hash = randomHash,
                    LastSeen = randomDateTime,
                    FileDeleted = false
                };

            IngestionTracking savedIngestionTracking = newIngestionTracking.DeepClone();

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                    service.AddIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(newIngestionTracking))))
                       .ReturnsAsync(storageIngestionTracking);

            IngestionTrackingAudit ingestionTrackingAudit = new IngestionTrackingAudit();
            ingestionTrackingAudit.Id = Guid.NewGuid();
            ingestionTrackingAudit.IngestionTrackingId = updatedIngestionTracking.Id;

            ingestionTrackingAudit.Message =
                "Received and updated file from TPP which has now been uploaded to the blob store";

            this.ingestionTrackingProcessingAuditServiceMock.Setup(service =>
                service.AddIngestionTrackingAuditAsync(ingestionTrackingAudit))
                    .ReturnsAsync(value: ingestionTrackingAudit);

            // when
            Guid returnedGuid = await tppOrchestrationServiceMock.Object.ProcessAsync(
                input: inputDataStream,
                fileName: inputFileName,
                supplierId: inputSupplierId);

            // then

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
               service.RetrieveAllIngestionTrackingsAsync(),
                   Times.Once);

            this.hashBrokerMock.Verify(broker =>
                broker.GenerateSha256HashAsync(inputDataStream),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Once);

            this.dataSetSpecificationProcessingServiceMock.Verify(service =>
                service.GetActiveDataSetSpecification(randomSupplierId),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.AddIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(newIngestionTracking))),
                    Times.Once);

            tppOrchestrationServiceMock.Verify(service =>
                service.LogAudit(
                    It.Is(SameIngestionTrackingAs(newIngestionTracking)),
                    $"New file found '{newIngestionTracking.FileName}',  " +
                        $"created item with Id: {newIngestionTracking.Id}"),
                            Times.Once);

            this.documentProcessingServiceMock.Verify(service =>
                service.AddDocumentAsync(
                    inputDataStream,
                    newIngestionTracking.DecryptedFileName,
                    blobContainers.Ingress),
                        Times.Once);

            tppOrchestrationServiceMock.Verify(service =>
                service.LogAudit(
                    It.Is(SameIngestionTrackingAs(newIngestionTracking)),
                    $"Downloaded file '{newIngestionTracking.FileName}' and successfully uploaded " +
                        $"to blob storage '{newIngestionTracking.DecryptedFileName}'"),
                            Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldProcessExisitingDocumentIfUpdatedUpdateHashAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            string randomHash = GetRandomString(64);
            string inputContainer = "ingress";
            int randomNumber = GetRandomNumber();
            Guid randomSupplierId = Guid.NewGuid();
            List<string> randomFileNames = GetRandomStrings();
            string randomFileName = randomFileNames.Last();
            string inputFileName = randomFileName;
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputFileName);
            Stream inputStream = new MemoryStream(inputBytes);

            var tppOrchestrationServiceMock = new Mock<TppLandingOrchestrationService>(
                documentProcessingServiceMock.Object,
                ingestionTrackingProcessingServiceMock.Object,
                ingestionTrackingProcessingAuditServiceMock.Object,
                dataSetSpecificationProcessingServiceMock.Object,
                blobContainers,
                loggingBrokerMock.Object,
                dateTimeBrokerMock.Object,
                identifierBrokerMock.Object,
                hashBrokerMock.Object,
                landingConfiguration)
            {
                CallBase = true
            };

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
            IngestionTracking storageIngestionTracking = randomIngestionTracking;
            IngestionTracking updatedIngestionTracking = storageIngestionTracking.DeepClone();
            updatedIngestionTracking.DecryptedFileSha256Hash = randomHash;

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackingsAsync())
                    .ReturnsAsync(randomIngestionTrackings.AsQueryable());

            this.hashBrokerMock.Setup(broker =>
               broker.GenerateSha256HashAsync(inputStream))
                   .ReturnsAsync(randomHash);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTime);

            this.documentProcessingServiceMock
                .Setup(service =>
                    service.AddDocumentAsync(inputStream, randomIngestionTracking.DecryptedFileName, inputContainer))
                .Returns(ValueTask.CompletedTask);

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.ModifyIngestionTrackingAsync(updatedIngestionTracking))
                    .ReturnsAsync(updatedIngestionTracking);

            IngestionTrackingAudit ingestionTrackingAudit = new IngestionTrackingAudit();
            ingestionTrackingAudit.Id = Guid.NewGuid();
            ingestionTrackingAudit.IngestionTrackingId = updatedIngestionTracking.Id;
            ingestionTrackingAudit.Message = "Updated TPP Hash";

            this.ingestionTrackingProcessingAuditServiceMock.Setup(service =>
                service.AddIngestionTrackingAuditAsync(ingestionTrackingAudit))
                    .ReturnsAsync(value: ingestionTrackingAudit);

            // when
            ValueTask<Guid> returnedGuid = tppOrchestrationServiceMock.Object
                .ProcessAsync(input: inputStream, fileName: inputFileName, supplierId: randomSupplierId);

            // then
            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackingsAsync(),
                    Times.Once);

            this.hashBrokerMock.Verify(broker =>
                broker.GenerateSha256HashAsync(inputStream),
                    Times.Once);

            this.documentProcessingServiceMock.Verify(service =>
                service.AddDocumentAsync(inputStream, randomIngestionTracking.DecryptedFileName, inputContainer),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.ModifyIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(updatedIngestionTracking))),
                    Times.Once);

            tppOrchestrationServiceMock.Verify(service =>
                service.LogAudit(
                    It.Is(SameIngestionTrackingAs(updatedIngestionTracking)),
                    $"Received and updated file from TPP which has now been uploaded " +
                        $"to the blob storage '{updatedIngestionTracking.DecryptedFileName}'"),
                            Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingAuditServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}