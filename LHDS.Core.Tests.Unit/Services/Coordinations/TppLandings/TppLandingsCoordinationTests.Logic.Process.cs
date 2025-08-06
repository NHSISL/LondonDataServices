// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.TppLandings
{
    public partial class TppLandingsCoordinationTests
    {
        [Fact]
        public async Task ShouldProcessAndLogAsync()
        {
            // given
            Stream randomData = new MemoryStream(Encoding.UTF8.GetBytes(GetRandomString()));
            Stream inputData = randomData;
            string inputFileName = GetRandomString();
            Guid inputSupplierId = Guid.NewGuid();
            Guid ingestionTrackingId = Guid.NewGuid();
            Guid expectedIngestionTrackingId = ingestionTrackingId;

            this.tppLandingOrchestrationServiceMock.Setup(service =>
                service.ProcessAsync(inputFileName, inputSupplierId))
                    .ReturnsAsync(ingestionTrackingId);

            // when
            Guid actualIngestionTrackingId = await this.tppLandingCoordinationService.ProcessAsync(
                fileName: inputFileName,
                supplierId: inputSupplierId);

            // then
            actualIngestionTrackingId.ToString().Should().BeEquivalentTo(expectedIngestionTrackingId.ToString());

            this.tppLandingOrchestrationServiceMock.Verify(service =>
                service.ProcessAsync(inputFileName, inputSupplierId),
                Times.Once);

            this.tppLandingOrchestrationServiceMock.VerifyNoOtherCalls();
            this.ingressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
