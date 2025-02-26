// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.TerminologyPolls
{
    public partial class TerminologyPollsControllerTests
    {
        [Fact]
        public async Task ShouldReturnCreatedOnPostAsync()
        {
            // given
            TerminologyPoll randomTerminologyPoll = CreateRandomTerminologyPoll();
            TerminologyPoll inputTerminologyPoll = randomTerminologyPoll;
            TerminologyPoll addedTerminologyPoll = inputTerminologyPoll.DeepClone();
            TerminologyPoll expectedTerminologyPoll = addedTerminologyPoll.DeepClone();

            var expectedObjectResult =
                new CreatedObjectResult(expectedTerminologyPoll);

            var expectedActionResult =
                new ActionResult<TerminologyPoll>(expectedObjectResult);

            this.terminologyPollsServiceMock.Setup(service =>
                service.AddTerminologyPollAsync(inputTerminologyPoll))
                    .ReturnsAsync(addedTerminologyPoll);

            // when
            ActionResult<TerminologyPoll> actualActionResult =
                await this.terminologyPollsController.PostTerminologyPollAsync(inputTerminologyPoll);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.terminologyPollsServiceMock.Verify(service =>
                service.AddTerminologyPollAsync(inputTerminologyPoll),
                   Times.Once);

            this.terminologyPollsServiceMock.VerifyNoOtherCalls();
        }
    }
}
