// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.IngestionTrackings
{
    public partial class IngestionTrackingsControllerTests
    {
        [Fact]
        public async Task ShouldReturnOkOnDeleteAsync()
        {
            // given
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking();
            Guid inputId = randomIngestionTracking.Id;
            IngestionTracking storageIngestionTracking = randomIngestionTracking.DeepClone();
            IngestionTracking expectedIngestionTracking = storageIngestionTracking.DeepClone();

            var expectedObjectResult =
                new OkObjectResult(expectedIngestionTracking);

            var expectedActionResult =
                new ActionResult<IngestionTracking>(expectedObjectResult);

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RemoveIngestionTrackingByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(storageIngestionTracking);

            // when
            ActionResult<IngestionTracking> actualActionResult =
                await this.ingestionTrackingsController.DeleteIngestionTrackingByIdAsync(inputId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RemoveIngestionTrackingByIdAsync(It.IsAny<Guid>()),
                   Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
        }
    }
}
