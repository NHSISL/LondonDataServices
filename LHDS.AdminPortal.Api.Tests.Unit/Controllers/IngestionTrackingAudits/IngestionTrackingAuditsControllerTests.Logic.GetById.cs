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
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditsControllerTests
    {
        [Fact]
        public async Task ShouldReturnIngestionTrackingAuditOnGetByIdAsync()
        {
            // given 
            IngestionTrackingAudit randomIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();
            Guid inputId = randomIngestionTrackingAudit.Id;
            IngestionTrackingAudit storageIngestionTrackingAudit = randomIngestionTrackingAudit.DeepClone();
            IngestionTrackingAudit expectedIngestionTrackingAudit = storageIngestionTrackingAudit.DeepClone();
            var expectedObjectResult = new OkObjectResult(expectedIngestionTrackingAudit);

            var expectedActionResult =
                new ActionResult<IngestionTrackingAudit>(expectedObjectResult);

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingAuditByIdAsync(inputId))
                    .ReturnsAsync(expectedIngestionTrackingAudit);

            // when
            ActionResult<IngestionTrackingAudit> actualActionResult =
                await this.ingestionTrackingAuditsController.GetAuditByIdAsync(inputId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingAuditByIdAsync(inputId),
                    Times.Once);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
        }
    }
}
