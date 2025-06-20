// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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

            this.tppLandingOrchestrationServiceMock.Setup(service =>
                service.ReProcessAsync(inputSupplierId))
                    .Returns(ValueTask.CompletedTask);

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
