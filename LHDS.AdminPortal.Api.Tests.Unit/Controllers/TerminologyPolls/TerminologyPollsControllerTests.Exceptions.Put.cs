// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.TerminologyPolls
{
    public partial class TerminologyPollsControllerTests
    {
        [Fact]
        public async Task ShouldReturnBadRequestOnPutIfValidationErrorOccurredAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            TerminologyPoll someTerminologyPoll = CreateRandomTerminologyPoll();

            var TerminologyPollValidationException =
                new TerminologyPollValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(TerminologyPollValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<TerminologyPoll>(expectedBadRequestObjectResult);

            this.terminologyPollsServiceMock.Setup(service =>
                service.ModifyTerminologyPollAsync(It.IsAny<TerminologyPoll>()))
                    .ThrowsAsync(TerminologyPollValidationException);

            // when
            ActionResult<TerminologyPoll> actualActionResult =
                await this.terminologyPollsController.PutTerminologyPollAsync(someTerminologyPoll);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.terminologyPollsServiceMock.Verify(service =>
                service.ModifyTerminologyPollAsync(It.IsAny<TerminologyPoll>()),
                    Times.Once);

            this.terminologyPollsServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnPutIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();
            TerminologyPoll someTerminologyPoll = CreateRandomTerminologyPoll();

            var notFoundTerminologyPollException =
                new NotFoundTerminologyPollException(
                    terminologyPollId: someId);

            var TerminologyPollValidationException =
                new TerminologyPollValidationException(
                    message: someMessage,
                    innerException: notFoundTerminologyPollException);

            NotFoundObjectResult expectedNotFoundObjectResult =
                NotFound(notFoundTerminologyPollException);

            var expectedActionResult =
                new ActionResult<TerminologyPoll>(expectedNotFoundObjectResult);

            this.terminologyPollsServiceMock.Setup(service =>
                service.ModifyTerminologyPollAsync(It.IsAny<TerminologyPoll>()))
                    .ThrowsAsync(TerminologyPollValidationException);

            // when
            ActionResult<TerminologyPoll> actualActionResult =
                await this.terminologyPollsController.PutTerminologyPollAsync(someTerminologyPoll);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.terminologyPollsServiceMock.Verify(service =>
                service.ModifyTerminologyPollAsync(It.IsAny<TerminologyPoll>()),
                    Times.Once);

            this.terminologyPollsServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnFailedDependencyOnPutIfInvalidTerminologyPollReferenceAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            TerminologyPoll someTerminologyPoll = CreateRandomTerminologyPoll();

            var alreadyExistsTerminologyPollException =
                new InvalidTerminologyPollReferenceException(
                    message: someMessage,
                    innerException: someInnerException);

            var TerminologyPollDependencyValidationException =
                new TerminologyPollDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsTerminologyPollException);

            FailedDependencyObjectResult expectedBadRequestObjectResult =
                FailedDependency(TerminologyPollDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<TerminologyPoll>(expectedBadRequestObjectResult);

            this.terminologyPollsServiceMock.Setup(service =>
                service.ModifyTerminologyPollAsync(It.IsAny<TerminologyPoll>()))
                    .ThrowsAsync(TerminologyPollDependencyValidationException);

            // when
            ActionResult<TerminologyPoll> actualActionResult =
                await this.terminologyPollsController.PutTerminologyPollAsync(someTerminologyPoll);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.terminologyPollsServiceMock.Verify(service =>
                service.ModifyTerminologyPollAsync(It.IsAny<TerminologyPoll>()),
                    Times.Once);

            this.terminologyPollsServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnConflictOnPutIfAlreadyExistsTerminologyPollErrorOccurredAsync()
        {
            // given
            TerminologyPoll someTerminologyPoll = CreateRandomTerminologyPoll();
            var someInnerException = new Exception();
            string someMessage = GetRandomString();

            var alreadyExistsTerminologyPollException =
                new AlreadyExistsTerminologyPollException(
                    message: someMessage,
                    innerException: someInnerException);

            var TerminologyPollDependencyValidationException =
                new TerminologyPollDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsTerminologyPollException);

            ConflictObjectResult expectedConflictObjectResult =
                Conflict(alreadyExistsTerminologyPollException);

            var expectedActionResult =
                new ActionResult<TerminologyPoll>(expectedConflictObjectResult);

            this.terminologyPollsServiceMock.Setup(service =>
                service.ModifyTerminologyPollAsync(It.IsAny<TerminologyPoll>()))
                    .ThrowsAsync(TerminologyPollDependencyValidationException);

            // when
            ActionResult<TerminologyPoll> actualActionResult =
                await this.terminologyPollsController.PutTerminologyPollAsync(someTerminologyPoll);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.terminologyPollsServiceMock.Verify(service =>
                service.ModifyTerminologyPollAsync(It.IsAny<TerminologyPoll>()),
                    Times.Once);

            this.terminologyPollsServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnPutIfServerErrorOccurredAsync(
            Xeption validationException)
        {
            // given
            TerminologyPoll someTerminologyPoll = CreateRandomTerminologyPoll();

            InternalServerErrorObjectResult expectedBadRequestObjectResult =
                InternalServerError(validationException);

            var expectedActionResult =
                new ActionResult<TerminologyPoll>(expectedBadRequestObjectResult);

            this.terminologyPollsServiceMock.Setup(service =>
                service.ModifyTerminologyPollAsync(It.IsAny<TerminologyPoll>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<TerminologyPoll> actualActionResult =
                await this.terminologyPollsController.PutTerminologyPollAsync(someTerminologyPoll);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.terminologyPollsServiceMock.Verify(service =>
                service.ModifyTerminologyPollAsync(It.IsAny<TerminologyPoll>()),
                    Times.Once);

            this.terminologyPollsServiceMock.VerifyNoOtherCalls();
        }
    }
}
