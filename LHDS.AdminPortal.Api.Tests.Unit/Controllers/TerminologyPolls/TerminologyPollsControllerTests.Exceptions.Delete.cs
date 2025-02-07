// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.TerminologyPolls
{
    public partial class TerminologyPollsControllerTests
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
                new ActionResult<TerminologyPoll>(expectedBadRequestObjectResult);

            this.terminologyPollsServiceMock.Setup(service =>
                service.RemoveTerminologyPollByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<TerminologyPoll> actualActionResult =
                await this.terminologyPollsController.DeleteTerminologyPollByIdAsync(someId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.terminologyPollsServiceMock.Verify(service =>
                service.RemoveTerminologyPollByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.terminologyPollsServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnDeleteIfItemDoesNotExistAsync()
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
                service.RemoveTerminologyPollByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(TerminologyPollValidationException);

            // when
            ActionResult<TerminologyPoll> actualActionResult =
                await this.terminologyPollsController.DeleteTerminologyPollByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.terminologyPollsServiceMock.Verify(service =>
                service.RemoveTerminologyPollByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.terminologyPollsServiceMock.VerifyNoOtherCalls();
        }
    }
}
