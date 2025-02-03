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
        public async Task ShouldReturnIngestionTrackingAuditsOnGetAsync()
        {
            // given 
            IQueryable<IngestionTrackingAudit> randomIngestionTrackingAudit = CreateRandomIngestionTrackingAudits();
            IQueryable<IngestionTrackingAudit> storageIngestionTrackingAudit = randomIngestionTrackingAudit.DeepClone();
            
            IQueryable<IngestionTrackingAudit> expectedIngestionTrackingAudit = 
                storageIngestionTrackingAudit.DeepClone();
            
            var expectedObjectResult = new OkObjectResult(expectedIngestionTrackingAudit);

            var expectedActionResult =
                new ActionResult<IQueryable<IngestionTrackingAudit>>(expectedObjectResult);

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackingAuditsAsync())
                    .ReturnsAsync(expectedIngestionTrackingAudit);

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
