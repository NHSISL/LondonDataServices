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
        public async Task ShouldReturnBadRequestOnPostIfValidationErrorOccurredAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            OptOut someOptOut = CreateRandomOptOut();

            var OptOutValidationException =
                new OptOutProcessingValidationException(
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(OptOutValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<OptOut>(expectedBadRequestObjectResult);

            this.optOutProcessingServiceMock.Setup(service =>
                service.RetrieveOrAddOptOutAsync(someOptOut))
                    .ThrowsAsync(OptOutValidationException);

            // when
            ActionResult<OptOut> actualActionResult =
                await this.optOutsController.PostOptOutAsync(someOptOut);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.optOutProcessingServiceMock.Verify(service =>
                service.RetrieveOrAddOptOutAsync(someOptOut),
                    Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnPostIfServerErrorOccurredAsync(
            Xeption validationException)
        {
            // given
            OptOut someOptOut = CreateRandomOptOut();

            InternalServerErrorObjectResult expectedBadRequestObjectResult =
                InternalServerError(validationException);

            var expectedActionResult =
                new ActionResult<OptOut>(expectedBadRequestObjectResult);

            this.optOutProcessingServiceMock.Setup(service =>
                service.RetrieveOrAddOptOutAsync(someOptOut))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<OptOut> actualActionResult =
                await this.optOutsController.PostOptOutAsync(someOptOut);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.optOutProcessingServiceMock.Verify(service =>
                service.RetrieveOrAddOptOutAsync(someOptOut),
                    Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnConflictOnPostIfAlreadyExistsOptOutErrorOccurredAsync()
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
                    innerException: alreadyExistsOptOutException);

            ConflictObjectResult expectedConflictObjectResult =
                Conflict(alreadyExistsOptOutException);

            var expectedActionResult =
                new ActionResult<OptOut>(expectedConflictObjectResult);

            this.optOutProcessingServiceMock.Setup(service =>
                service.RetrieveOrAddOptOutAsync(It.IsAny<OptOut>()))
                    .ThrowsAsync(optOutProcessingDependencyValidationException);

            // when
            ActionResult<OptOut> actualActionResult =
                await this.optOutsController.PostOptOutAsync(someOptOut);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.optOutProcessingServiceMock.Verify(service =>
                service.RetrieveOrAddOptOutAsync(It.IsAny<OptOut>()),
                    Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnFailedDependencyOnPostIfInvalidOptOutReferenceAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            OptOut someOptOut = CreateRandomOptOut();

            var alreadyExistsOptOutException =
                new InvalidOptOutReferenceException(
                    message: someMessage,
                    innerException: someInnerException);

            var optOutProcessingDependencyValidationException =
                new OptOutProcessingDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsOptOutException);

            FailedDependencyObjectResult expectedBadRequestObjectResult =
                FailedDependency(optOutProcessingDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<OptOut>(expectedBadRequestObjectResult);

            this.optOutProcessingServiceMock.Setup(service =>
                service.RetrieveOrAddOptOutAsync(someOptOut))
                    .ThrowsAsync(optOutProcessingDependencyValidationException);

            // when
            ActionResult<OptOut> actualActionResult =
                await this.optOutsController.PostOptOutAsync(someOptOut);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.optOutProcessingServiceMock.Verify(service =>
                service.RetrieveOrAddOptOutAsync(someOptOut),
                    Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
