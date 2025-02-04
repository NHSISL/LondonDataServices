// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Foundations.OptOuts.Exceptions;
using LHDS.Core.Models.Processings.OptOuts.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.OptOuts
{
    public partial class OptOutsControllerTests
    {
        [Fact]
        public async Task ShouldReturnBadRequestOnPutIfValidationErrorOccurredAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            OptOut someOptOut = CreateRandomOptOut();

            var optOutProcessingValidationException =
                new OptOutProcessingValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(optOutProcessingValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<OptOut>(expectedBadRequestObjectResult);

            this.optOutProcessingServiceMock.Setup(service =>
                service.AddOrModifyOptOutAsync(someOptOut))
                    .ThrowsAsync(optOutProcessingValidationException);

            // when
            ActionResult<OptOut> actualActionResult =
                await this.optOutsController.PutOptOutAsync(someOptOut);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.optOutProcessingServiceMock.Verify(service =>
                service.AddOrModifyOptOutAsync(someOptOut),
                    Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnPutIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();
            OptOut someOptOut = CreateRandomOptOut();

            var notFoundOptOutException =
                new NotFoundOptOutException(message: $"Couldn't find optOut with optOutId: {someId}.");

            var OptOutValidationException =
                new OptOutProcessingValidationException(
                    message: someMessage,
                    innerException: notFoundOptOutException);

            NotFoundObjectResult expectedNotFoundObjectResult =
                NotFound(notFoundOptOutException);

            var expectedActionResult =
                new ActionResult<OptOut>(expectedNotFoundObjectResult);

            this.optOutProcessingServiceMock.Setup(service =>
                service.AddOrModifyOptOutAsync(someOptOut))
                    .ThrowsAsync(OptOutValidationException);

            // when
            ActionResult<OptOut> actualActionResult =
                await this.optOutsController.PutOptOutAsync(someOptOut);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.optOutProcessingServiceMock.Verify(service =>
                service.AddOrModifyOptOutAsync(someOptOut),
                    Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnFailedDependencyOnPutIfInvalidOptOutReferenceAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            OptOut someOptOut = CreateRandomOptOut();

            var alreadyExistsOptOutException =
                new InvalidOptOutReferenceException(
                    message: someMessage,
                    innerException: someInnerException);

            var OptOutDependencyValidationException =
                new OptOutDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsOptOutException);

            FailedDependencyObjectResult expectedBadRequestObjectResult =
                FailedDependency(OptOutDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<OptOut>(expectedBadRequestObjectResult);

            this.optOutProcessingServiceMock.Setup(service =>
                service.AddOrModifyOptOutAsync(It.IsAny<OptOut>()))
                    .ThrowsAsync(OptOutDependencyValidationException);

            // when
            ActionResult<OptOut> actualActionResult =
                await this.optOutsController.PutOptOutAsync(someOptOut);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.optOutProcessingServiceMock.Verify(service =>
                service.AddOrModifyOptOutAsync(It.IsAny<OptOut>()),
                    Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnConflictOnPutIfAlreadyExistsOptOutErrorOccurredAsync()
        {
            // given
            OptOut someOptOut = CreateRandomOptOut();
            var someInnerException = new Exception();
            string someMessage = GetRandomString();

            var alreadyExistsOptOutException =
                new AlreadyExistsOptOutException(
                    message: someMessage,
                    innerException: someInnerException);

            var optOutProcessingDependencyValidationException =
                new OptOutProcessingDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsOptOutException);

            ConflictObjectResult expectedConflictObjectResult =
                Conflict(alreadyExistsOptOutException);

            var expectedActionResult =
                new ActionResult<OptOut>(expectedConflictObjectResult);

            this.optOutProcessingServiceMock.Setup(service =>
                service.AddOrModifyOptOutAsync(It.IsAny<OptOut>()))
                    .ThrowsAsync(optOutProcessingDependencyValidationException);

            // when
            ActionResult<OptOut> actualActionResult =
                await this.optOutsController.PutOptOutAsync(someOptOut);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.optOutProcessingServiceMock.Verify(service =>
                service.AddOrModifyOptOutAsync(It.IsAny<OptOut>()),
                    Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnPutIfServerErrorOccurredAsync(
            Xeption validationException)
        {
            // given
            OptOut someOptOut = CreateRandomOptOut();

            InternalServerErrorObjectResult expectedBadRequestObjectResult =
                InternalServerError(validationException);

            var expectedActionResult =
                new ActionResult<OptOut>(expectedBadRequestObjectResult);

            this.optOutProcessingServiceMock.Setup(service =>
                service.AddOrModifyOptOutAsync(It.IsAny<OptOut>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<OptOut> actualActionResult =
                await this.optOutsController.PutOptOutAsync(someOptOut);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.optOutProcessingServiceMock.Verify(service =>
                service.AddOrModifyOptOutAsync(It.IsAny<OptOut>()),
                    Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
