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
using RESTFulSense.Models;
using Xeptions;
using Xunit;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.IngestionTrackings
{
    public partial class IngestionTrackingsControllerTests
    {
        [Fact]
        public async Task ShouldReturnBadRequestOnPostIfValidationErrorOccurredAsync()
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
                service.AddIngestionTrackingAsync(someIngestionTracking))
                    .ThrowsAsync(IngestionTrackingValidationException);

            // when
            ActionResult<IngestionTracking> actualActionResult =
                await this.ingestionTrackingsController.PostIngestionTrackingAsync(someIngestionTracking);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.AddIngestionTrackingAsync(someIngestionTracking),
                    Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnPostIfServerErrorOccurredAsync(
            Xeption validationException)
        {
            // given
            IngestionTracking someIngestionTracking = CreateRandomIngestionTracking();

            InternalServerErrorObjectResult expectedBadRequestObjectResult =
                InternalServerError(validationException);

            var expectedActionResult =
                new ActionResult<IngestionTracking>(expectedBadRequestObjectResult);

            this.ingestionTrackingServiceMock.Setup(service =>
                service.AddIngestionTrackingAsync(someIngestionTracking))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<IngestionTracking> actualActionResult =
                await this.ingestionTrackingsController.PostIngestionTrackingAsync(someIngestionTracking);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.AddIngestionTrackingAsync(someIngestionTracking),
                    Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnConflictOnPostIfAlreadyExistsIngestionTrackingErrorOccurredAsync()
        {
            // given
            IngestionTracking someIngestionTracking = CreateRandomIngestionTracking();
            var someInnerException = new Exception();
            string someMessage = GetRandomString();

            var alreadyExistsIngestionTrackingException =
                new AlreadyExistsIngestionTrackingException(
                    message: someMessage,
                    innerException: someInnerException);

            var IngestionTrackingDependencyValidationException =
                new IngestionTrackingDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsIngestionTrackingException);

            ConflictObjectResult expectedConflictObjectResult =
                Conflict(alreadyExistsIngestionTrackingException);

            var expectedActionResult =
                new ActionResult<IngestionTracking>(expectedConflictObjectResult);

            this.ingestionTrackingServiceMock.Setup(service =>
                service.AddIngestionTrackingAsync(It.IsAny<IngestionTracking>()))
                    .ThrowsAsync(IngestionTrackingDependencyValidationException);

            // when
            ActionResult<IngestionTracking> actualActionResult =
                await this.ingestionTrackingsController.PostIngestionTrackingAsync(someIngestionTracking);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.AddIngestionTrackingAsync(It.IsAny<IngestionTracking>()),
                    Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnFailedDependencyOnPostIfInvalidIngestionTrackingReferenceAsync()
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
                service.AddIngestionTrackingAsync(someIngestionTracking))
                    .ThrowsAsync(IngestionTrackingDependencyValidationException);

            // when
            ActionResult<IngestionTracking> actualActionResult =
                await this.ingestionTrackingsController.PostIngestionTrackingAsync(someIngestionTracking);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.AddIngestionTrackingAsync(someIngestionTracking),
                    Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
        }
    }
}
