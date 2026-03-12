// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.TerminologyPolls
{
    public partial class TerminologyPollsControllerTests
    {
        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnGetIfServerErrorOccurredAsync(Xeption serverException)
        {
            // given 
            IQueryable<TerminologyPoll> someTerminologyPolls = CreateRandomTerminologyPolls();

            InternalServerErrorObjectResult expectedInternalServerErrorObjectResult =
                InternalServerError(serverException);

            var expectedActionResult =
                new ActionResult<IQueryable<TerminologyPoll>>(expectedInternalServerErrorObjectResult);

            this.terminologyPollsServiceMock.Setup(service =>
                service.RetrieveAllTerminologyPollsAsync())
                    .ThrowsAsync(serverException);

            // when
            ActionResult<IQueryable<TerminologyPoll>> actualActionResult =
                await this.terminologyPollsController.GetAllTerminologyPollsAsync();

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.terminologyPollsServiceMock.Verify(service =>
                service.RetrieveAllTerminologyPollsAsync(),
                    Times.Once);

            this.terminologyPollsServiceMock.VerifyNoOtherCalls();
        }
    }
}
