// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Microsoft.AspNetCore.Mvc;
using LHDS.Core.Models.Foundations.PdsAudits;
using Moq;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.PdsAudits
{
    public partial class PdsAuditControllerTests
    {
        [Fact]
        public async Task ShouldReturnPdsAuditsOnGetAsync()
        {
            // given
            IQueryable<PdsAudit> randomPdsAudit = CreateRandomPdsAudits();
            IQueryable<PdsAudit> storagePdsAudit = randomPdsAudit.DeepClone();
            IQueryable<PdsAudit> expectedPdsAudit = storagePdsAudit.DeepClone();
            var expectedObjectResult = new OkObjectResult(expectedPdsAudit);

            var expectedActionResult =
                new ActionResult<IQueryable<PdsAudit>>(expectedObjectResult);

            this.pdsAuditServiceMock.Setup(service =>
                service.RetrieveAllPdsAuditsAsync())
                    .ReturnsAsync(expectedPdsAudit);

            // when
            ActionResult<IQueryable<PdsAudit>> actualActionResult =
                await this.pdsAuditsController.Get();

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.pdsAuditServiceMock.Verify(service =>
                service.RetrieveAllPdsAuditsAsync(),
                    Times.Once());

            this.pdsAuditServiceMock.VerifyNoOtherCalls();
        }
    }
}
