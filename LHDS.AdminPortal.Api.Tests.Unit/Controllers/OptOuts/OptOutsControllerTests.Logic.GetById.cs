// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task ShouldReturnOptOutOnGetByIdAsync()
        {
            // given 
            OptOut randomOptOut = CreateRandomOptOut();
            string randomNhsNumber = GetRandomString();
            OptOut storageOptOut = randomOptOut.DeepClone();
            OptOut expectedOptOut = storageOptOut.DeepClone();
            var expectedObjectResult = new OkObjectResult(expectedOptOut);

            var expectedActionResult =
                new ActionResult<OptOut>(expectedObjectResult);

            this.optOutProcessingServiceMock.Setup(service =>
                service.RetrieveOptOutByNhsNumberAsync(randomNhsNumber))
                    .ReturnsAsync(expectedOptOut);

            // when
            ActionResult<OptOut> actualActionResult =
                await this.optOutsController.GetOptOutByNhsNumberAsync(randomNhsNumber);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.optOutProcessingServiceMock.Verify(service =>
                service.RetrieveOptOutByNhsNumberAsync(randomNhsNumber),
                    Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
