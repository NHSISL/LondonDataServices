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
        public async Task ShouldReturnBadRequestOnGetByIdIfValidationErrorOccurredAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            var TerminologyPollValidationException =
                new TerminologyPollValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(TerminologyPollValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<TerminologyPoll>(expectedBadRequestObjectResult);

            this.terminologyPollsServiceMock.Setup(service =>
                service.RetrieveTerminologyPollByIdAsync(someId))
                    .ThrowsAsync(TerminologyPollValidationException);

            // when
            ActionResult<TerminologyPoll> actualActionResult =
                await this.terminologyPollsController.GetTerminologyPollByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.terminologyPollsServiceMock.Verify(service =>
                service.RetrieveTerminologyPollByIdAsync(someId),
                    Times.Once);

            this.terminologyPollsServiceMock.VerifyNoOtherCalls();
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
                new ActionResult<TerminologyPoll>(expectedInternalServerErrorObjectResult);

            this.terminologyPollsServiceMock.Setup(service =>
                service.RetrieveTerminologyPollByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serverException);

            // when
            ActionResult<TerminologyPoll> actualActionResult =
                await this.terminologyPollsController.GetTerminologyPollByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.terminologyPollsServiceMock.Verify(service =>
                service.RetrieveTerminologyPollByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.terminologyPollsServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnGetByIdIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();

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
                service.RetrieveTerminologyPollByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(TerminologyPollValidationException);

            // when
            ActionResult<TerminologyPoll> actualActionResult =
                await this.terminologyPollsController.GetTerminologyPollByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.terminologyPollsServiceMock.Verify(service =>
                service.RetrieveTerminologyPollByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.terminologyPollsServiceMock.VerifyNoOtherCalls();
        }
    }
}