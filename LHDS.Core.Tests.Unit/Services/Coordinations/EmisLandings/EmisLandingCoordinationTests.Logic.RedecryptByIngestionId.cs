// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.EmisLandings
{
    public partial class EmisLandingCoordinationServiceTests
    {
        [Fact]
        public async Task ShouldRedecryptDocumentAndLogAsync()
        {
            // Given
            Guid ingestionTrackingId = Guid.NewGuid();

            // When
            await this.emisLandingCoordinationService.RedecryptDocumentByIngestionIdAsync(ingestionTrackingId);

            // Then
            this.emisLandingOrchestrationServiceMock.Verify(service =>
                service.RedecryptDocumentByIngestionIdAsync(ingestionTrackingId),
                    Times.Once);

            this.emisLandingOrchestrationServiceMock.VerifyNoOtherCalls();
            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

