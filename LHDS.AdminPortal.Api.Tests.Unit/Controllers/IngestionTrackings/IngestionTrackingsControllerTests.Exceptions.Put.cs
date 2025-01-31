// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using Xeptions;
using Xunit;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using RESTFulSense.Models;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.IngestionTrackings
{
    public partial class IngestionTrackingsControllerTests
    {
        [Fact]
        public async Task ShouldReturnBadRequestOnPutIfValidationErrorOccurredAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            IngestionTracking someIngestionTracking = CreateRandomIngestionTracking();

            var IngestionTrackingValidationException =
                new IngestionTrackingValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(IngestionTrackingValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<IngestionTracking>(expectedBadRequestObjectResult);

            this.ingestionTrackingServiceMock.Setup(service =>
                service.ModifyIngestionTrackingAsync(someIngestionTracking))
                    .ThrowsAsync(IngestionTrackingValidationException);

            // when
            ActionResult<IngestionTracking> actualActionResult =
                await this.ingestionTrackingsController.PutIngestionTrackingAsync(someIngestionTracking);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.ModifyIngestionTrackingAsync(someIngestionTracking),
                    Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnPutIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();
            IngestionTracking someIngestionTracking = CreateRandomIngestionTracking();

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
                service.ModifyIngestionTrackingAsync(someIngestionTracking))
                    .ThrowsAsync(IngestionTrackingValidationException);

            // when
            ActionResult<IngestionTracking> actualActionResult =
                await this.ingestionTrackingsController.PutIngestionTrackingAsync(someIngestionTracking);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.ModifyIngestionTrackingAsync(someIngestionTracking),
                    Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnFailedDependencyOnPutIfInvalidIngestionTrackingReferenceAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            IngestionTracking someIngestionTracking = CreateRandomIngestionTracking();

            var alreadyExistsIngestionTrackingException =
                new InvalidIngestionTrackingReferenceException(
                    message: someMessage,
                    innerException: someInnerException);

            var IngestionTrackingDependencyValidationException =
                new IngestionTrackingDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsIngestionTrackingException);

            FailedDependencyObjectResult expectedBadRequestObjectResult =
                FailedDependency(IngestionTrackingDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<IngestionTracking>(expectedBadRequestObjectResult);

            this.ingestionTrackingServiceMock.Setup(service =>
                service.ModifyIngestionTrackingAsync(It.IsAny<IngestionTracking>()))
                    .ThrowsAsync(IngestionTrackingDependencyValidationException);

            // when
            ActionResult<IngestionTracking> actualActionResult =
                await this.ingestionTrackingsController.PutIngestionTrackingAsync(someIngestionTracking);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.ModifyIngestionTrackingAsync(It.IsAny<IngestionTracking>()),
                    Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
        }
    }
}
