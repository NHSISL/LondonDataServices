// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using LHDS.Core.Models.Foundations.PdsAudits;
using Moq;
using Xunit;
using Force.DeepCloner;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.PdsAudits
{
    public partial class PdsAuditControllerTests
    {
        [Fact]
        public async Task ShouldReturnPdsAuditOnGetByIdAsync()
        {
            // given 
            PdsAudit randomPdsAudit = CreateRandomPdsAudit();
            Guid inputId = randomPdsAudit.Id;
            PdsAudit storagePdsAudit = randomPdsAudit.DeepClone();
            PdsAudit expectedPdsAudit = storagePdsAudit.DeepClone();
            var expectedObjectResult = new OkObjectResult(expectedPdsAudit);

            var expectedActionResult =
                new ActionResult<PdsAudit>(expectedObjectResult);

            this.pdsAuditServiceMock.Setup(service =>
                service.RetrievePdsAuditByIdAsync(inputId))
                    .ReturnsAsync(expectedPdsAudit);

            // when
            ActionResult<PdsAudit> actualActionResult =
                await this.pdsAuditsController.GetPdsAuditByIdAsync(inputId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.pdsAuditServiceMock.Verify(service =>
                service.RetrievePdsAuditByIdAsync(inputId),
                    Times.Once);

            this.pdsAuditServiceMock.VerifyNoOtherCalls();
        }
    }
}
