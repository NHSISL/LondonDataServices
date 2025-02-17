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
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.TerminologyPolls
{
    public partial class TerminologyPollsControllerTests
    {
        [Fact]
        public async Task ShouldReturnOkOnPutAsync()
        {
            // given
            TerminologyPoll randomTerminologyPoll = CreateRandomTerminologyPoll();
            TerminologyPoll inputTerminologyPoll = randomTerminologyPoll;
            TerminologyPoll storageTerminologyPoll = inputTerminologyPoll.DeepClone();
            TerminologyPoll expectedTerminologyPoll = storageTerminologyPoll.DeepClone();

            var expectedObjectResult =
                new OkObjectResult(expectedTerminologyPoll);

            var expectedActionResult =
                new ActionResult<TerminologyPoll>(expectedObjectResult);

            this.terminologyPollsServiceMock.Setup(service =>
                service.ModifyTerminologyPollAsync(inputTerminologyPoll))
                    .ReturnsAsync(storageTerminologyPoll);

            // when
            ActionResult<TerminologyPoll> actualActionResult =
                await this.terminologyPollsController.PutTerminologyPollAsync(inputTerminologyPoll);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.terminologyPollsServiceMock.Verify(service =>
                service.ModifyTerminologyPollAsync(inputTerminologyPoll),
                   Times.Once);

            this.terminologyPollsServiceMock.VerifyNoOtherCalls();
        }
    }
}
