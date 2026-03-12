// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.IngestionTrackings
{
    public partial class IngestionTrackingsControllerTests
    {
        [Fact]
        public async Task ShouldReturnBadRequestOnGetByIdIfValidationErrorOccurredAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            var IngestionTrackingValidationException =
                new IngestionTrackingValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(IngestionTrackingValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<IngestionTracking>(expectedBadRequestObjectResult);

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingByIdAsync(someId))
                    .ThrowsAsync(IngestionTrackingValidationException);

            // when
            ActionResult<IngestionTracking> actualActionResult =
                await this.ingestionTrackingsController.GetIngestionTrackingByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByIdAsync(someId),
                    Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnGetByIdIfServerErrorOccurredAsync(Xeption serverException)
        {
            // given 
            Guid someId = Guid.NewGuid();

            InternalServerErrorObjectResult expectedInternalServerErrorObjectResult =
                InternalServerError(serverException);

            var expectedActionResult =
                new ActionResult<IngestionTracking>(expectedInternalServerErrorObjectResult);

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serverException);

            // when
            ActionResult<IngestionTracking> actualActionResult =
                await this.ingestionTrackingsController.GetIngestionTrackingByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnGetByIdIfItemDoesNotExistAsync()
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
                service.RetrieveIngestionTrackingByIdAsync(someId))
                    .ThrowsAsync(IngestionTrackingValidationException);

            // when
            ActionResult<IngestionTracking> actualActionResult =
                await this.ingestionTrackingsController.GetIngestionTrackingByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByIdAsync(someId),
                    Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
        }
    }
}
