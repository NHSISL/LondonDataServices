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
        public async Task ShouldReturnIngestionTrackingsOnGetAsync()
        {
            // given
            IQueryable<IngestionTracking> randomIngestionTracking = CreateRandomIngestionTrackings();
            IQueryable<IngestionTracking> storageIngestionTracking = randomIngestionTracking.DeepClone();
            IQueryable<IngestionTracking> expectedIngestionTracking = storageIngestionTracking.DeepClone();
            var expectedObjectResult = new OkObjectResult(expectedIngestionTracking);

            var expectedActionResult =
                new ActionResult<IQueryable<IngestionTracking>>(expectedObjectResult);

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackingsAsync())
                    .ReturnsAsync(expectedIngestionTracking);

            // when
            ActionResult<IQueryable<IngestionTracking>> actualActionResult =
                await this.ingestionTrackingsController.Get();

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackingsAsync(),
                    Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
        }
    }
}
