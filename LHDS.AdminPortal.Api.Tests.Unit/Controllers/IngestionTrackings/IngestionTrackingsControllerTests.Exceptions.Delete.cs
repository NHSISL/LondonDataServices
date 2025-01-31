// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.IngestionTrackings
{
    public partial class IngestionTrackingsControllerTests
    {
        [Theory]
        [MemberData(nameof(ValidationExceptions))]
        public async Task ShouldReturnBadRequestOnDeleteIfValidationErrorOccurredAsync(Xeption validationException)
        {
            // given
            Guid someId = Guid.NewGuid();

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(validationException.InnerException ?? validationException);

            var expectedActionResult =
                new ActionResult<IngestionTracking>(expectedBadRequestObjectResult);

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RemoveIngestionTrackingByIdAsync(someId))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<IngestionTracking> actualActionResult =
                await this.ingestionTrackingsController.DeleteIngestionTrackingByIdAsync(someId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RemoveIngestionTrackingByIdAsync(someId),
                    Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnDeleteIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();

            var notFoundIngestionTrackingException =
                new NotFoundIngestionTrackingException(
                    ingestionTrackingId: someId);

            var IngestionTrackingValidationException =
                new IngestionTrackingValidationException(
                    message: someMessage,
                    innerException: notFoundIngestionTrackingException);

            NotFoundObjectResult expectedNotFoundObjectResult =
                NotFound(notFoundIngestionTrackingException);

            var expectedActionResult =
                new ActionResult<IngestionTracking>(expectedNotFoundObjectResult);

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RemoveIngestionTrackingByIdAsync(someId))
                    .ThrowsAsync(IngestionTrackingValidationException);

            // when
            ActionResult<IngestionTracking> actualActionResult =
                await this.ingestionTrackingsController.DeleteIngestionTrackingByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RemoveIngestionTrackingByIdAsync(someId),
                    Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
        }
    }
}
