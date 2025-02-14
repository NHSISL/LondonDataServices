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
using RESTFulSense.Models;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.IngestionTrackings
{
    public partial class IngestionTrackingsControllerTests
    {
        [Fact]
        public async Task ShouldReturnCreatedOnPostAsync()
        {
            // given
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking();
            IngestionTracking inputIngestionTracking = randomIngestionTracking;
            IngestionTracking addedIngestionTracking = inputIngestionTracking.DeepClone();
            IngestionTracking expectedIngestionTracking = addedIngestionTracking.DeepClone();

            var expectedObjectResult =
                new CreatedObjectResult(expectedIngestionTracking);

            var expectedActionResult =
                new ActionResult<IngestionTracking>(expectedObjectResult);

            this.ingestionTrackingServiceMock.Setup(service =>
                service.AddIngestionTrackingAsync(inputIngestionTracking))
                    .ReturnsAsync(addedIngestionTracking);

            // when
            ActionResult<IngestionTracking> actualActionResult =
                await this.ingestionTrackingsController.PostIngestionTrackingAsync(inputIngestionTracking);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.AddIngestionTrackingAsync(inputIngestionTracking),
                   Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
        }
    }
}
