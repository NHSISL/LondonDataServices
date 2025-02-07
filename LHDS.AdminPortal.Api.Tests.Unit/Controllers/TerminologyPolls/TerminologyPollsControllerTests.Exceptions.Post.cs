// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async Task ShouldReturnBadRequestOnPostIfValidationErrorOccurredAsync()
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
                service.AddTerminologyPollAsync(It.IsAny<TerminologyPoll>()))
                    .ThrowsAsync(TerminologyPollValidationException);

            // when
            ActionResult<TerminologyPoll> actualActionResult =
                await this.terminologyPollsController.PostTerminologyPollAsync(someTerminologyPoll);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.terminologyPollsServiceMock.Verify(service =>
                service.AddTerminologyPollAsync(It.IsAny<TerminologyPoll>()),
                    Times.Once);

            this.terminologyPollsServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnPostIfServerErrorOccurredAsync(
            Xeption validationException)
        {
            // given
            TerminologyPoll someTerminologyPoll = CreateRandomTerminologyPoll();

            InternalServerErrorObjectResult expectedBadRequestObjectResult =
                InternalServerError(validationException);

            var expectedActionResult =
                new ActionResult<TerminologyPoll>(expectedBadRequestObjectResult);

            this.terminologyPollsServiceMock.Setup(service =>
                service.AddTerminologyPollAsync(It.IsAny<TerminologyPoll>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<TerminologyPoll> actualActionResult =
                await this.terminologyPollsController.PostTerminologyPollAsync(someTerminologyPoll);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.terminologyPollsServiceMock.Verify(service =>
                service.AddTerminologyPollAsync(It.IsAny<TerminologyPoll>()),
                    Times.Once);

            this.terminologyPollsServiceMock.VerifyNoOtherCalls();
        }
    }
}
