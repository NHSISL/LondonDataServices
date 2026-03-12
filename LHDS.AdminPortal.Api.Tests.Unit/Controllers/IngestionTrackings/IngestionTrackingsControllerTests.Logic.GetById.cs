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
using LHDS.Core.Models.Foundations.IngestionTrackings;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.IngestionTrackings
{
    public partial class IngestionTrackingsControllerTests
    {
        [Fact]
        public async Task ShouldReturnIngestionTrackingOnGetByIdAsync()
        {
            // given 
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking();
            Guid inputId = randomIngestionTracking.Id;
            IngestionTracking storageIngestionTracking = randomIngestionTracking.DeepClone();
            IngestionTracking expectedIngestionTracking = storageIngestionTracking.DeepClone();
            var expectedObjectResult = new OkObjectResult(expectedIngestionTracking);

            var expectedActionResult =
                new ActionResult<IngestionTracking>(expectedObjectResult);

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingByIdAsync(inputId))
                    .ReturnsAsync(expectedIngestionTracking);

            // when
            ActionResult<IngestionTracking> actualActionResult =
                await this.ingestionTrackingsController.GetIngestionTrackingByIdAsync(inputId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByIdAsync(inputId),
                    Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
        }
    }
}