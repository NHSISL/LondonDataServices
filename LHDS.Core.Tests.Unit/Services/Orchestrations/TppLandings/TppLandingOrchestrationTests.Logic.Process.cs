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
                service.RetrieveAllIngestionTrackings())
                    .Returns(randomIngestionTrackings.AsQueryable());

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                   .Returns(randomDateTime);

            this.hashBrokerMock.Setup(broker => broker.GenerateSha256Hash(inputData))
                .Returns(randomHash);

            // when
            ValueTask<Guid> returnedGuid = this.tppOrchestrationService.ProcessAsync(
                input: inputData,
                fileName: inputFileName,
                supplierId: inputSupplierId);

            // then
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackings(),
                    Times.Once);

            this.hashBrokerMock.Verify(broker =>
                broker.GenerateSha256Hash(inputData),
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

            List<string> randomFileNames = GetRandomStrings();
            List<string> inputFileNames = randomFileNames;

            byte[] randomData = CreateRandomData();
            byte[] inputData = randomData;
            Stream inputDataStream = new MemoryStream(inputData);

            List<IngestionTracking> randomIngestionTrackings =
                CreateRandomIngestionTrackings(
                    dateTimeOffset: randomDateTime,
                    fileNames: inputFileNames,
                    supplierId: randomSupplierId);

            IQueryable<DataSetSpecification> randomDataSetSpecificationList =
               CreateRandomDataSetSpecifications(dataSet: randomDataSet);

            DataSetSpecification randomDataSetSpecification = randomDataSetSpecificationList.First();
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking(randomDateTime);
            string inputFileName = randomIngestionTracking.FileName;
            IngestionTracking storageIngestionTracking = randomIngestionTracking;
            IngestionTracking updatedIngestionTracking = storageIngestionTracking.DeepClone();

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackings())
                    .Returns(randomIngestionTrackings.AsQueryable());

            this.hashBrokerMock.Setup(broker =>
                broker.GenerateSha256Hash(inputDataStream))
                    .Returns(randomHash);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            this.dataSetSpecificationProcessingServiceMock.Setup(service =>
               service.GetActiveDataSetSpecification(randomSupplierId))
                   .Returns(ValueTask.FromResult(randomDataSetSpecificationList.FirstOrDefault()));

            var filename = inputFileName.StartsWith('/')
                ? inputFileName
                : "/" + inputFileName;

            IngestionTracking newIngestionTracking =
                new IngestionTracking
                {
                    Id = randomGuid,
                    FileName = filename,
                    SupplierId = randomSupplierId,
                    EncryptedFileName = null,

                    DecryptedFileName =
                        $"/{landingConfiguration.DecryptedFolder}"
                        + $"/{randomDataSet.DataSetName}"
                        + $"/{randomDataSetSpecification.Id}"
                        + $"{filename}",

                    Decrypted = false,
                    LastSeen = randomDateTime,
                    FileDeleted = false,
                    RecordCount = 0,
                    EncryptedFileSize = inputData.Length,
                    EncryptedFileSha256Hash = randomHash,
                    DecryptedFileSize = 0,
                    DecryptedFileSha256Hash = string.Empty,
                    CreatedBy = "System",
                    CreatedDate = randomDateTime,
                    UpdatedBy = "System",
                    UpdatedDate = randomDateTime
                };

            IngestionTracking savedIngestionTracking = newIngestionTracking.DeepClone();

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                    service.AddIngestionTrackingAsync(newIngestionTracking))
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
            ValueTask<Guid> returnedGuid = this.tppOrchestrationService.ProcessAsync(
                input: inputDataStream,
                fileName: inputFileName,
                supplierId: inputSupplierId);

            // then

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
               service.RetrieveAllIngestionTrackings(),
                   Times.Once);

            this.hashBrokerMock.Verify(broker =>
                broker.GenerateSha256Hash(inputDataStream),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.dataSetSpecificationProcessingServiceMock.Verify(service =>
                service.GetActiveDataSetSpecification(randomSupplierId),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.AddIngestionTrackingAsync(It.IsAny<IngestionTracking>()),
                    Times.Once);

            this.documentProcessingServiceMock.Verify(service =>
                service.AddDocumentAsync(
                    inputDataStream,
                    newIngestionTracking.DecryptedFileName,
                    blobContainers.Ingress),
                        Times.Once);

            this.ingestionTrackingProcessingAuditServiceMock.Verify(service =>
                service.AddIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
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
                service.RetrieveAllIngestionTrackings())
                    .Returns(randomIngestionTrackings.AsQueryable());

            this.hashBrokerMock.Setup(broker =>
               broker.GenerateSha256Hash(inputStream))
                   .Returns(randomHash);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

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
            ValueTask<Guid> returnedGuid = this.tppOrchestrationService
                .ProcessAsync(input: inputStream, fileName: inputFileName, supplierId: randomSupplierId);

            // then
            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackings(),
                    Times.Once);

            this.hashBrokerMock.Verify(broker =>
                broker.GenerateSha256Hash(inputStream),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.documentProcessingServiceMock.Verify(service =>
                service.AddDocumentAsync(inputStream, randomIngestionTracking.DecryptedFileName, inputContainer),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.ModifyIngestionTrackingAsync(It.IsAny<IngestionTracking>()),
                    Times.Once);

            this.ingestionTrackingProcessingAuditServiceMock.Verify(service =>
                service.AddIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
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