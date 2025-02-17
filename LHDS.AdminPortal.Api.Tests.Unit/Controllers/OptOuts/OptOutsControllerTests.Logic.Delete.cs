// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.OptOuts;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using LHDS.Core.Models.Foundations.PdsAudits;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.OptOuts
{
    public partial class OptOutsControllerTests
    {
        [Fact]
        public async Task ShouldReturnOkOnDeleteAsync()
        {
            // given
            OptOut randomOptOut = CreateRandomOptOut();
            Guid inputId = randomOptOut.Id;
            OptOut storageOptOut = randomOptOut.DeepClone();
            OptOut expectedOptOut = storageOptOut.DeepClone();

            var expectedObjectResult =
                new OkObjectResult(expectedOptOut);

            var expectedActionResult =
                new ActionResult<OptOut>(expectedObjectResult);

            this.optOutProcessingServiceMock.Setup(service =>
                service.RemoveOptOutByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(storageOptOut);

            // when
            ActionResult<OptOut> actualActionResult =
                await this.optOutsController.DeleteOptOutByIdAsync(inputId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.optOutProcessingServiceMock.Verify(service =>
                service.RemoveOptOutByIdAsync(It.IsAny<Guid>()),
                   Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
