// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditsControllerTests
    {
        [Fact]
        public async Task ShouldReturnCreatedOnPostAsync()
        {
            // given
            IngestionTrackingAudit randomIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();
            IngestionTrackingAudit inputIngestionTrackingAudit = randomIngestionTrackingAudit;
            IngestionTrackingAudit addedIngestionTrackingAudit = inputIngestionTrackingAudit.DeepClone();
            IngestionTrackingAudit expectedIngestionTrackingAudit = addedIngestionTrackingAudit.DeepClone();

            var expectedObjectResult =
                new CreatedObjectResult(expectedIngestionTrackingAudit);

            var expectedActionResult =
                new ActionResult<IngestionTrackingAudit>(expectedObjectResult);

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.AddIngestionTrackingAuditAsync(inputIngestionTrackingAudit))
                    .ReturnsAsync(addedIngestionTrackingAudit);

            // when
            ActionResult<IngestionTrackingAudit> actualActionResult =
                await this.ingestionTrackingAuditsController.PostAuditAsync(inputIngestionTrackingAudit);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.AddIngestionTrackingAuditAsync(inputIngestionTrackingAudit),
                   Times.Once);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
        }
    }
}
