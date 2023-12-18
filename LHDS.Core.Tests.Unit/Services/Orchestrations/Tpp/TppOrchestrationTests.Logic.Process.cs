// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Tpp
{
    public partial class TppOrchestrationTests
    {
        [Fact]
        public async Task ShouldProcessExisitingDocumentAndUpdateHashAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Models.Foundations.Documents.Document randomDocument = CreateRandomDocument();
            string randomHash = GetRandomString(64);
            randomDocument.SHA256Hash = randomHash;
            int randomNumber = GetRandomNumber();

            List<Models.Foundations.Documents.Document> randomDocuments = CreateRandomDocuments(randomNumber);
            randomDocuments[randomNumber - 1].FileName = randomDocument.FileName;

            List<IngestionTracking> randomIngestionTrackings =
                CreateRandomIngestionTrackings(randomDateTime, randomDocuments);

            IngestionTracking randomIngestionTracking = randomIngestionTrackings[randomNumber - 1];
            IngestionTracking storageIngestionTracking = randomIngestionTracking;
            IngestionTracking updatedIngestionTracking = storageIngestionTracking.DeepClone();
            updatedIngestionTracking.DecryptedFileSha256Hash = randomDocument.SHA256Hash;

            this.hashBrokerMock.Setup(broker =>
                broker.GenerateSha256Hash(randomDocument.DocumentData))
                    .Returns(randomHash);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackings())
                    .Returns(randomIngestionTrackings.AsQueryable());

            this.documentProcessingServiceMock.Setup(service =>
                service.AddDocumentAsync(
                    It.Is(SameDocumentAs(randomDocument)),
                    landingConfiguration.DecryptedFolder))
                        .ReturnsAsync(randomDocument.FileName);

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
            ValueTask<Guid> returnedGuid = this.tppOrchestrationService.ProcessAsync(randomDocument);

            // then
            this.hashBrokerMock.Verify(broker =>
               broker.GenerateSha256Hash(randomDocument.DocumentData),
                   Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackings(),
                    Times.Once);

            this.documentProcessingServiceMock.Verify(service =>
                service.AddDocumentAsync(
                    It.Is(SameDocumentAs(randomDocument)),
                    It.IsAny<string>()),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
               service.ModifyIngestionTrackingAsync(It.IsAny<IngestionTracking>()),
                   Times.Once);

            this.ingestionTrackingProcessingAuditServiceMock.Verify(service =>
                service.AddIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                    Times.Once);

            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingAuditServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
        }

        //DecryptedFileName =
        //    $"/{randonIngestionTracking..DataSet.DataSetName}" +
        //    $"/{retrievedDataSetSpecification.Id}" +
        //    $"/{filename.Split('_')[3]}" +
        //    $"{filename.Replace(".gpg", "", StringComparison.InvariantCultureIgnoreCase)}",
    }
}