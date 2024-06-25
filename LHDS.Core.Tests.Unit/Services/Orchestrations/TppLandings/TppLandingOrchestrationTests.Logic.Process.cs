// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Stream randomData = new MemoryStream(Encoding.ASCII.GetBytes(GetRandomString()));
            Stream inputData = randomData;

            List<IngestionTracking> randomIngestionTrackings =
                CreateRandomIngestionTrackings(
                    dateTimeOffset: randomDateTime,
                    fileNames: randomFileNames,
                    supplierId: randomSupplierId);

            IngestionTracking randomIngestionTracking = randomIngestionTrackings[randomNumber - 1];
            randomIngestionTracking.DecryptedFileSha256Hash = randomHash;

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

        [Fact(Skip = "Conversion to stream")]
        public async Task ShouldProcessNewDocumentAndAddAsync()
        {
            //// given
            //DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            //Guid randomGuid = Guid.NewGuid();
            //Guid randomSupplierId = Guid.NewGuid();
            //DataSet randomDataSet = CreateRandomDataSet(supplierId: randomSupplierId);
            //Document randomDocument = CreateRandomDocument();
            //string randomHash = GetRandomString(64);
            //randomDocument.SHA256Hash = randomHash;
            //int randomNumber = GetRandomNumber();

            //List<Document> randomDocuments = CreateRandomDocuments(count: randomNumber);

            //List<IngestionTracking> randomIngestionTrackings =
            //    CreateRandomIngestionTrackings(
            //        dateTimeOffset: randomDateTime,
            //        documents: randomDocuments,
            //        supplierId: randomSupplierId);

            //IQueryable<DataSetSpecification> randomDataSetSpecificationList =
            //   CreateRandomDataSetSpecifications(dataSet: randomDataSet);

            //DataSetSpecification randomDataSetSpecification = randomDataSetSpecificationList.First();
            //IngestionTracking randomIngestionTracking = randomIngestionTrackings[randomNumber - 1];
            //IngestionTracking storageIngestionTracking = randomIngestionTracking;
            //IngestionTracking updatedIngestionTracking = storageIngestionTracking.DeepClone();

            //this.ingestionTrackingProcessingServiceMock.Setup(service =>
            //    service.RetrieveAllIngestionTrackings())
            //        .Returns(randomIngestionTrackings.AsQueryable());

            //this.hashBrokerMock.Setup(broker =>
            //    broker.GenerateSha256Hash(randomDocument.DocumentData))
            //        .Returns(randomHash);

            //this.dateTimeBrokerMock.Setup(broker =>
            //    broker.GetCurrentDateTimeOffset())
            //        .Returns(randomDateTime);

            //this.dataSetSpecificationProcessingServiceMock.Setup(service =>
            //   service.GetActiveDataSetSpecification(randomSupplierId))
            //       .Returns(ValueTask.FromResult(randomDataSetSpecificationList.FirstOrDefault()));

            //var filename = randomDocument.FileName.StartsWith('/')
            //       ? randomDocument.FileName
            //       : "/" + randomDocument.FileName;

            //IngestionTracking newIngestionTracking =
            //       new IngestionTracking
            //       {
            //           Id = randomGuid,
            //           FileName = randomDocument.FileName,
            //           SupplierId = randomSupplierId,
            //           EncryptedFileName = null,

            //           DecryptedFileName =
            //                    $"/{landingConfiguration.DecryptedFolder}"
            //                    + $"/{randomDataSet.DataSetName}"
            //                    + $"/{randomDataSetSpecification.Id}"
            //                    + $"{filename}",

            //           Decrypted = false,
            //           LastSeen = randomDateTime,
            //           FileDeleted = false,
            //           RecordCount = 0,
            //           EncryptedFileSize = randomDocument.DocumentData.Length,
            //           EncryptedFileSha256Hash = randomHash,
            //           DecryptedFileSize = 0,
            //           DecryptedFileSha256Hash = string.Empty,
            //           CreatedBy = "System",
            //           CreatedDate = randomDateTime,
            //           UpdatedBy = "System",
            //           UpdatedDate = randomDateTime
            //       };

            //IngestionTracking savedIngestionTracking = newIngestionTracking.DeepClone();

            //this.ingestionTrackingProcessingServiceMock.Setup(service =>
            //        service.AddIngestionTrackingAsync(newIngestionTracking))
            //           .ReturnsAsync(storageIngestionTracking);

            //Document newBlobDocument = new Document
            //{
            //    DocumentData = randomDocument.DocumentData,
            //    FileName = savedIngestionTracking.DecryptedFileName
            //};

            //this.documentProcessingServiceMock.Setup(service =>
            //    service.AddDocumentAsync(It.Is(SameDocumentAs(newBlobDocument)), blobContainers.Versioner));

            //IngestionTrackingAudit ingestionTrackingAudit = new IngestionTrackingAudit();
            //ingestionTrackingAudit.Id = Guid.NewGuid();
            //ingestionTrackingAudit.IngestionTrackingId = updatedIngestionTracking.Id;

            //ingestionTrackingAudit.Message =
            //    "Received and updated file from TPP which has now been uploaded to the blob store";

            //this.ingestionTrackingProcessingAuditServiceMock.Setup(service =>
            //    service.AddIngestionTrackingAuditAsync(ingestionTrackingAudit))
            //        .ReturnsAsync(value: ingestionTrackingAudit);

            //// when
            //ValueTask<Guid> returnedGuid = this.tppOrchestrationService
            //    .ProcessAsync(document: randomDocument, supplierId: randomIngestionTracking.SupplierId);

            //// then

            //this.ingestionTrackingProcessingServiceMock.Verify(service =>
            //   service.RetrieveAllIngestionTrackings(),
            //       Times.Once);

            //this.hashBrokerMock.Verify(broker =>
            //    broker.GenerateSha256Hash(randomDocument.DocumentData),
            //        Times.Once);

            //this.dateTimeBrokerMock.Verify(broker =>
            //    broker.GetCurrentDateTimeOffset(),
            //        Times.Once);

            //this.dataSetSpecificationProcessingServiceMock.Verify(service =>
            //    service.GetActiveDataSetSpecification(randomSupplierId),
            //        Times.Once);

            //this.ingestionTrackingProcessingServiceMock.Verify(service =>
            //    service.AddIngestionTrackingAsync(It.IsAny<IngestionTracking>()),
            //        Times.Once);

            //this.documentProcessingServiceMock.Verify(service =>
            //    service.AddDocumentAsync(It.Is(SameDocumentAs(newBlobDocument)), blobContainers.Versioner),
            //        Times.Once);

            //this.ingestionTrackingProcessingAuditServiceMock.Verify(service =>
            //    service.AddIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
            //        Times.Once);

            //this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            //this.hashBrokerMock.VerifyNoOtherCalls();
            //this.dateTimeBrokerMock.VerifyNoOtherCalls();
            //this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            //this.documentProcessingServiceMock.VerifyNoOtherCalls();
            //this.ingestionTrackingProcessingAuditServiceMock.VerifyNoOtherCalls();
            //this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact(Skip = "Conversion to stream")]
        public async Task ShouldProcessExisitingDocumentIfUpdatedUpdateHashAsync()
        {
            //// given
            //DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            //Document randomDocument = CreateRandomDocument();
            //string randomHash = GetRandomString(64);
            //randomDocument.SHA256Hash = randomHash;
            //int randomNumber = GetRandomNumber();
            //Guid randomSupplierId = Guid.NewGuid();

            //List<Document> randomDocuments = CreateRandomDocuments(randomNumber);
            //randomDocuments[randomNumber - 1].FileName = randomDocument.FileName;

            //List<IngestionTracking> randomIngestionTrackings =
            //    CreateRandomIngestionTrackings(
            //        dateTimeOffset: randomDateTime,
            //        documents: randomDocuments,
            //        supplierId: randomSupplierId);

            //IngestionTracking randomIngestionTracking = randomIngestionTrackings[randomNumber - 1];
            //IngestionTracking storageIngestionTracking = randomIngestionTracking;
            //IngestionTracking updatedIngestionTracking = storageIngestionTracking.DeepClone();
            //updatedIngestionTracking.DecryptedFileSha256Hash = randomDocument.SHA256Hash;

            //this.ingestionTrackingProcessingServiceMock.Setup(service =>
            //    service.RetrieveAllIngestionTrackings())
            //        .Returns(randomIngestionTrackings.AsQueryable());

            //this.hashBrokerMock.Setup(broker =>
            //   broker.GenerateSha256Hash(randomDocument.DocumentData))
            //       .Returns(randomHash);

            //this.dateTimeBrokerMock.Setup(broker =>
            //    broker.GetCurrentDateTimeOffset())
            //        .Returns(randomDateTime);

            //this.documentProcessingServiceMock.Setup(service =>
            //    service.AddDocumentAsync(
            //        It.Is(SameDocumentAs(randomDocument)),
            //            landingConfiguration.DecryptedFolder))
            //                .ReturnsAsync(randomDocument.FileName);

            //this.ingestionTrackingProcessingServiceMock.Setup(service =>
            //    service.ModifyIngestionTrackingAsync(updatedIngestionTracking))
            //        .ReturnsAsync(updatedIngestionTracking);

            //IngestionTrackingAudit ingestionTrackingAudit = new IngestionTrackingAudit();
            //ingestionTrackingAudit.Id = Guid.NewGuid();
            //ingestionTrackingAudit.IngestionTrackingId = updatedIngestionTracking.Id;
            //ingestionTrackingAudit.Message = "Updated TPP Hash";

            //this.ingestionTrackingProcessingAuditServiceMock.Setup(service =>
            //    service.AddIngestionTrackingAuditAsync(ingestionTrackingAudit))
            //        .ReturnsAsync(value: ingestionTrackingAudit);

            //// when
            //ValueTask<Guid> returnedGuid = this.tppOrchestrationService
            //    .ProcessAsync(document: randomDocument, supplierId: randomSupplierId);

            //// then
            //this.ingestionTrackingProcessingServiceMock.Verify(service =>
            //    service.RetrieveAllIngestionTrackings(),
            //        Times.Once);

            //this.hashBrokerMock.Verify(broker =>
            //    broker.GenerateSha256Hash(randomDocument.DocumentData),
            //        Times.Once);

            //this.dateTimeBrokerMock.Verify(broker =>
            //    broker.GetCurrentDateTimeOffset(),
            //        Times.Once);

            //this.documentProcessingServiceMock.Verify(service =>
            //    service.AddDocumentAsync(
            //        It.Is(SameDocumentAs(randomDocument)),
            //        It.IsAny<string>()),
            //        Times.Once);

            //this.ingestionTrackingProcessingServiceMock.Verify(service =>
            //    service.ModifyIngestionTrackingAsync(It.IsAny<IngestionTracking>()),
            //        Times.Once);

            //this.ingestionTrackingProcessingAuditServiceMock.Verify(service =>
            //    service.AddIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
            //        Times.Once);

            //this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            //this.hashBrokerMock.VerifyNoOtherCalls();
            //this.dateTimeBrokerMock.VerifyNoOtherCalls();
            //this.documentProcessingServiceMock.VerifyNoOtherCalls();
            //this.ingestionTrackingProcessingAuditServiceMock.VerifyNoOtherCalls();
            //this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            //this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}