// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Text;
using System.Threading.Tasks;
using LHDS.Landings.Client.Models.Audits;
using LHDS.Landings.Client.Models.Foundations.Documents;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings;
using Moq;

namespace LHDS.Landings.Client.Tests.Unit.Services.Orchestrations.Decryptions
{

    public partial class DecryptionOrchestrationTests
    {
        [Fact]
        public async Task ShouldProcessDecryptedDocumentAsync()
        {
            // given
            string randomFileName = GetRandomMessage();
            byte[] randomEncryptedString = Encoding.ASCII.GetBytes(GetRandomMessage());
            byte[] randomDecryptedString = Encoding.ASCII.GetBytes(GetRandomMessage());
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            var isDecrypted = false;
            Document randomDocument = new Document { FileName=randomFileName, DocumentData=randomEncryptedString};

            //setup documentservice mock
            //

            IngestionTracking externalIngestionTrackingFound = null;

            this.documentServiceMock.Setup(service =>
                service.RetrieveDocumentByFileNameAsync(randomFileName, false))
                    .ReturnsAsync(randomDocument);

            this.decryptionServiceMock.Setup(service =>
                service.DecryptAsync(randomDocument.DocumentData))
                    .ReturnsAsync(randomDecryptedString);

            IngestionTracking ingestionTracking = new IngestionTracking
            {
                Id = randomDocument.FileName,
                FileName = randomDocument.FileName,
                Decrypted = true,
                CreatedDate = randomDateTime,
            };

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingByFileNameAsync(randomDocument.FileName))
                    .ReturnsAsync(ingestionTracking);

            // when
            await this.decryptionOrchestrationService.DecryptAsync(randomFileName);

            // then
            this.documentServiceMock.Verify(service =>
                service.RetrieveDocumentByFileNameAsync(randomFileName, false),
                    Times.Once);

            this.decryptionServiceMock.Verify(service =>
                service.DecryptAsync(randomDocument.DocumentData),
                    Times.Once);

            this.documentServiceMock.Verify(service =>
                service.AddDocumentAsync(randomDocument, true),
                    Times.Once);

            //this.ingestionTrackingServiceMock.Verify(service =>
            //    service.RetrieveIngestionTrackingByFileNameAsync(randomDocument.FileName),
            //        Times.Once);
            
            this.auditServiceMock.Verify(service =>
                service.AddAuditAsync(It.IsAny<Audit>()),
                    Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
