// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using LHDS.Core.Models.Foundations.ResolvedAddressesAudits;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.ResolvedAddressAudits
{
    public partial class ResolvedAddressAuditControllerTests
    {
        [Fact]
        public async Task ShouldReturnResolvedAddressAuditsOnGetAsync()
        {
            // given
            IQueryable<ResolvedAddressAudit> randomResolvedAddressAudit = CreateRandomResolvedAddressAudits();
            IQueryable<ResolvedAddressAudit> storageResolvedAddressAudit = randomResolvedAddressAudit.DeepClone();
            IQueryable<ResolvedAddressAudit> expectedResolvedAddressAudit = storageResolvedAddressAudit.DeepClone();
            var expectedObjectResult = new OkObjectResult(expectedResolvedAddressAudit);

            var expectedActionResult =
                new ActionResult<IQueryable<ResolvedAddressAudit>>(expectedObjectResult);

            this.resolvedAddressAuditServiceMock.Setup(service =>
                service.RetrieveAllResolvedAddressAuditsAsync())
                    .ReturnsAsync(expectedResolvedAddressAudit);

            // when
            ActionResult<IQueryable<ResolvedAddressAudit>> actualActionResult =
                await this.resolvedAddressAuditsController.Get();

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.resolvedAddressAuditServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddressAuditsAsync(),
                    Times.Once());

            this.resolvedAddressAuditServiceMock.VerifyNoOtherCalls();
        }
    }
}
