// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attrify.Attributes;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Models.Foundations.OptOuts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.OptOuts
{
    public partial class OptOutsControllerTests
    {
        [Fact]
        public async Task ShouldReturnOptOutsOnGetAsync()
        {
            // given
            IQueryable<OptOut> randomOptOut = CreateRandomOptOuts();
            IQueryable<OptOut> storageOptOut = randomOptOut.DeepClone();
            IQueryable<OptOut> expectedOptOut = storageOptOut.DeepClone();
            var expectedObjectResult = new OkObjectResult(expectedOptOut);

            var expectedActionResult =
                new ActionResult<IQueryable<OptOut>>(expectedObjectResult);

            this.optOutProcessingServiceMock.Setup(service =>
                service.RetrieveAllOptOutsAsync())
                    .ReturnsAsync(expectedOptOut);

            // when
            ActionResult<IQueryable<OptOut>> actualActionResult =
                await this.optOutsController.Get();

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.optOutProcessingServiceMock.Verify(service =>
                service.RetrieveAllOptOutsAsync(),
                    Times.Once());

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
