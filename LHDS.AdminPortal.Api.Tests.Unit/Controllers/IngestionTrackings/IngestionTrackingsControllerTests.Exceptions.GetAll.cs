// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.IngestionTrackings
{
    public partial class IngestionTrackingsControllerTests
    {
        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnGetIfServerErrorOccurredAsync(Xeption serverException)
        {
            // given 
            IQueryable<IngestionTracking> someIngestionTrackings = CreateRandomIngestionTrackings();

            InternalServerErrorObjectResult expectedInternalServerErrorObjectResult =
                InternalServerError(serverException);

            var expectedActionResult =
                new ActionResult<IQueryable<IngestionTracking>>(expectedInternalServerErrorObjectResult);

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackingsAsync())
                    .ThrowsAsync(serverException);

            // when
            ActionResult<IQueryable<IngestionTracking>> actualActionResult =
                await this.ingestionTrackingsController.Get();

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackingsAsync(),
                    Times.Once());

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
        }
    }
}
