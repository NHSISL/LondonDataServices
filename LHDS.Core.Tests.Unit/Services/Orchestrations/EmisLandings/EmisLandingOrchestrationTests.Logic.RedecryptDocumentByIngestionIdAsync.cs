// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.EmisLandings
{
    public partial class EmisLandingOrchestrationTests
    {
        [Fact]
        public async Task ShouldRedecryptDocumentByIngestionIdAsync()
        {
            // given
            DateTimeOffset dateTimeOffset = GetRandomDateTimeOffset();
            DateTimeOffset currentDateTimeOffset = DateTimeOffset.UtcNow;
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking(dateTimeOffset);
            randomIngestionTracking.Decrypted = true;
            IngestionTracking inputIngestionTracking = randomIngestionTracking;
            IngestionTracking storageIngestionTracking = inputIngestionTracking.DeepClone();
            IngestionTracking expectedIngestionTracking = inputIngestionTracking.DeepClone();
            expectedIngestionTracking.Decrypted = false;
            expectedIngestionTracking.UpdatedDate = currentDateTimeOffset;

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingByIdAsync(inputIngestionTracking.Id))
                    .ReturnsAsync(storageIngestionTracking);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(currentDateTimeOffset);

            // when
            await this.emisLandingOrchestrationService.RedecryptDocumentByIngestionIdAsync(inputIngestionTracking.Id);

            // then
            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByIdAsync(inputIngestionTracking.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTimeOffset(),
                   Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}