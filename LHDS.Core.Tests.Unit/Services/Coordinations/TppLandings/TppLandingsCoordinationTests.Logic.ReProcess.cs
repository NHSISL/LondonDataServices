// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.TppLandings
{
    public partial class TppLandingsCoordinationTests
    {
        [Fact]
        public async Task ShouldReProcessAndLogAsync()
        {
            // given
            Guid inputSupplierId = Guid.NewGuid();
            Guid ingestionTrackingId = Guid.NewGuid();

            this.tppLandingOrchestrationServiceMock.Setup(service =>
                service.ReProcessAsync(inputSupplierId))
                    .ReturnsAsync(new List<Guid> { ingestionTrackingId });

            // when
            await this.tppLandingCoordinationService.ReProcessAsync(
                supplierId: inputSupplierId);

            // then
            this.tppLandingOrchestrationServiceMock.Verify(service =>
                service.ReProcessAsync(inputSupplierId),
                    Times.Once);

            this.tppLandingOrchestrationServiceMock.VerifyNoOtherCalls();
            this.ingressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
