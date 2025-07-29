// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Services.Orchestrations.Downloads;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.EmisLandings
{
    public partial class EmisLandingOrchestrationTests
    {
        [Fact]
        public async Task ShouldMarkAsDeleteIfFileWereRemovedAsync()
        {
            // given
            Guid inputSupplierId = landingConfiguration.LandingSupplierId;
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            DateTimeOffset lastSeenDateTime = randomDateTime.AddMinutes(-this.landingConfiguration.LastSeenMinutes);
            SubscriberCredential someSubscriberCredential = CreateRandomSubscriberCredential();

            List<IngestionTracking> storageIngestionTrackings = new List<IngestionTracking>
            {
                new IngestionTracking
                {
                    Id = Guid.NewGuid(),
                    SupplierId = inputSupplierId,
                    FileName = "test.txt",
                    EncryptedFileName = "/encrypted/test.txt",
                    DecryptedFileName = "/decrypted/test.txt",
                    Decrypted = true,
                    LastSeen = lastSeenDateTime,
                    FileDeleted = false,
                    SubscriberAgreementId = someSubscriberCredential.Id
                }
            };

            var emisLandingOrchestrationServiceMock = new Mock<EmisLandingOrchestrationService>(
                documentProcessingServiceMock.Object,
                downloadProcessingServiceMock.Object,
                ingestionTrackingProcessingServiceMock.Object,
                ingestionTrackingAuditProcessingServiceMock.Object,
                dataSetSpecificationProcessingServiceMock.Object,
                this.blobContainers,
                loggingBrokerMock.Object,
                dateTimeBrokerMock.Object,
                identifierBrokerMock.Object,
                hashBrokerMock.Object,
                fileBrokerMock.Object,
                landingConfiguration)
            {
                CallBase = true
            };

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTime);

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackingsAsync())
                    .ReturnsAsync(storageIngestionTrackings.AsQueryable());

            List<IngestionTracking> unavailableIngestionTrackings = storageIngestionTrackings
              .Where(ingestionTracking =>
                  ingestionTracking.LastSeen <= lastSeenDateTime
                  && !ingestionTracking.FileDeleted
                  && ingestionTracking.SubscriberAgreementId == someSubscriberCredential.Id)
              .ToList();

            List<IngestionTracking> notSeenIngestionTrackings = storageIngestionTrackings.DeepClone();

            foreach (var item in notSeenIngestionTrackings)
            {
                item.FileDeleted = true;

                this.ingestionTrackingProcessingServiceMock.Setup(service =>
                    service.ModifyIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(item))))
                        .ReturnsAsync(item);
            }

            // when
            await emisLandingOrchestrationServiceMock.Object.MarkItemsAsDeleteThatHasNotBeenSeen(someSubscriberCredential.Id);

            // then
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackingsAsync(),
                    Times.Once);

            foreach (var item in notSeenIngestionTrackings)
            {
                this.ingestionTrackingProcessingServiceMock.Verify(service =>
                    service.ModifyIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(item))),
                        Times.Once);
            }

            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingAuditProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}