// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.TerminologyPolls
{
    public partial class TerminologyPollsControllerTests
    {
        [Fact]
        public async Task ShouldReturnTerminologyPollOnGetByIdAsync()
        {
            // given 
            TerminologyPoll randomTerminologyPoll = CreateRandomTerminologyPoll();
            Guid inputId = randomTerminologyPoll.Id;
            TerminologyPoll storageTerminologyPoll = randomTerminologyPoll.DeepClone();
            TerminologyPoll expectedTerminologyPoll = storageTerminologyPoll.DeepClone();
            var expectedObjectResult = new OkObjectResult(expectedTerminologyPoll);

            var expectedActionResult =
                new ActionResult<TerminologyPoll>(expectedObjectResult);

            this.terminologyPollsServiceMock.Setup(service =>
                service.RetrieveTerminologyPollByIdAsync(inputId))
                    .ReturnsAsync(expectedTerminologyPoll);

            // when
            ActionResult<TerminologyPoll> actualActionResult =
                await this.terminologyPollsController.GetTerminologyPollByIdAsync(inputId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.terminologyPollsServiceMock.Verify(service =>
                service.RetrieveTerminologyPollByIdAsync(inputId),
                    Times.Once);

            this.terminologyPollsServiceMock.VerifyNoOtherCalls();
        }
    }
}
