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
using RESTFulSense.Models;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.OptOuts
{
    public partial class OptOutsControllerTests
    {
        [Fact]
        public async Task ShouldReturnCreatedOnPostAsync()
        {
            // given
            OptOut randomOptOut = CreateRandomOptOut();
            OptOut inputOptOut = randomOptOut;
            OptOut addedOptOut = inputOptOut.DeepClone();
            OptOut expectedOptOut = addedOptOut.DeepClone();

            var expectedObjectResult =
                new CreatedObjectResult(expectedOptOut);

            var expectedActionResult =
                new ActionResult<OptOut>(expectedObjectResult);

            this.optOutProcessingServiceMock.Setup(service =>
                service.RetrieveOrAddOptOutAsync(inputOptOut))
                    .ReturnsAsync(addedOptOut);

            // when
            ActionResult<OptOut> actualActionResult =
                await this.optOutsController.PostOptOutAsync(inputOptOut);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.optOutProcessingServiceMock.Verify(service =>
                service.RetrieveOrAddOptOutAsync(inputOptOut),
                   Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
