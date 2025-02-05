// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.EmisLandings
{
    public partial class EmisLandingsControllerTests
    {
        [Fact]
        public async Task ShouldReturnOkOnPutAsync()
        {
            // given
            var ingestionTrackingId = Guid.NewGuid();
            var expectedObjectResult = Ok();
            var expectedActionResult = (ActionResult)expectedObjectResult;

            this.emisLandingCoordinationServiceMock.Setup(service =>
                service.RedecryptDocumentByIngestionIdAsync(ingestionTrackingId))
                    .Returns(ValueTask.CompletedTask);

            // when
            ActionResult actualActionResult =
                await this.emisLandingsController.RedecryptDocumentByIngestionTrackingIdAsync(ingestionTrackingId);

            // then
            actualActionResult.Should().BeOfType<OkResult>();

            this.emisLandingCoordinationServiceMock.Verify(service =>
                service.RedecryptDocumentByIngestionIdAsync(ingestionTrackingId),
                Times.Once);

            this.emisLandingCoordinationServiceMock.VerifyNoOtherCalls();
        }
    }
}
