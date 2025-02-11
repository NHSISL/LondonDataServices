// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditsControllerTests
    {
        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnGetIfServerErrorOccurredAsync(Xeption serverException)
        {
            // given 
            IQueryable<IngestionTrackingAudit> someIngestionTrackingAudits = CreateRandomIngestionTrackingAudits();

            InternalServerErrorObjectResult expectedInternalServerErrorObjectResult =
                InternalServerError(serverException);

            var expectedActionResult =
                new ActionResult<IQueryable<IngestionTrackingAudit>>(expectedInternalServerErrorObjectResult);

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackingAuditsAsync())
                    .ThrowsAsync(serverException);

            // when
            ActionResult<IQueryable<IngestionTrackingAudit>> actualActionResult =
                await this.ingestionTrackingAuditsController.Get();

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackingAuditsAsync(),
                    Times.Once());

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
        }
    }
}
