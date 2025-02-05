// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataTypes.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.EmisLandings
{
    public partial class EmisLandingsControllerTests
    {
        [Fact]
        public async Task ShouldReturnBadRequestOnPutIfValidationErrorOccurredAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            Guid someIngestionTrackingId = Guid.NewGuid();

            var ingestionTrackingValidationException =
                new IngestionTrackingValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(ingestionTrackingValidationException.InnerException);

            this.emisLandingCoordinationServiceMock.Setup(service =>
                service.RedecryptDocumentByIngestionIdAsync(someIngestionTrackingId))
                    .ThrowsAsync(ingestionTrackingValidationException);

            // when
            ActionResult actualActionResult =
                await this.emisLandingsController.RedecryptDocumentByIngestionTrackingIdAsync(someIngestionTrackingId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedBadRequestObjectResult);

            this.emisLandingCoordinationServiceMock.Verify(service =>
                service.RedecryptDocumentByIngestionIdAsync(someIngestionTrackingId),
                    Times.Once);

            this.emisLandingCoordinationServiceMock.VerifyNoOtherCalls();
        }
    }
}
