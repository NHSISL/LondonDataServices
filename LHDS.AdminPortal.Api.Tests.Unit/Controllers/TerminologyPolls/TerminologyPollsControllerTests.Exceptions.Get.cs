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
    }
}
