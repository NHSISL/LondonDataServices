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
using RESTFulSense.Clients.Extensions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.OptOuts
{
    public partial class OptOutsControllerTests
    {
        [Fact]
        public async Task ShouldReturnOkOnPutAsync()
        {
            // given
            OptOut randomOptOut = CreateRandomOptOut();
            OptOut inputOptOut = randomOptOut;
            OptOut storageOptOut = inputOptOut.DeepClone();
            OptOut expectedOptOut = storageOptOut.DeepClone();

            var expectedObjectResult =
                new OkObjectResult(expectedOptOut);

            var expectedActionResult =
                new ActionResult<OptOut>(expectedObjectResult);

            this.optOutProcessingServiceMock.Setup(service =>
                service.AddOrModifyOptOutAsync(inputOptOut))
                    .ReturnsAsync(storageOptOut);

            // when
            ActionResult<OptOut> actualActionResult =
                await this.optOutsController.PutOptOutAsync(inputOptOut);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.optOutProcessingServiceMock.Verify(service =>
                service.AddOrModifyOptOutAsync(inputOptOut),
                   Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
