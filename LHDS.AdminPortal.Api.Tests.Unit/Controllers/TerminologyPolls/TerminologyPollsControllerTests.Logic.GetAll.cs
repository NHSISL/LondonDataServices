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
        public async Task ShouldReturnTerminologyPollsOnGetAsync()
        {
            // given 
            IQueryable<TerminologyPoll> randomTerminologyPoll = CreateRandomTerminologyPolls();
            IQueryable<TerminologyPoll> storageTerminologyPoll = randomTerminologyPoll.DeepClone();
            IQueryable<TerminologyPoll> expectedTerminologyPoll = storageTerminologyPoll.DeepClone();
            var expectedObjectResult = new OkObjectResult(expectedTerminologyPoll);

            var expectedActionResult =
                new ActionResult<IQueryable<TerminologyPoll>>(expectedObjectResult);

            this.terminologyPollsServiceMock.Setup(service =>
                service.RetrieveAllTerminologyPollsAsync())
                    .ReturnsAsync(expectedTerminologyPoll);

            // when
            ActionResult<IQueryable<TerminologyPoll>> actualActionResult =
                await this.terminologyPollsController.GetAllTerminologyPollsAsync();

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.terminologyPollsServiceMock.Verify(service =>
                service.RetrieveAllTerminologyPollsAsync(),
                    Times.Once());

            this.terminologyPollsServiceMock.VerifyNoOtherCalls();
        }
    }
}
