// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using RESTFulSense.Models;
using Xeptions;
using LHDS.Core.Models.Foundations.ResolvedAddressesAudits;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.ResolvedAddressAudits
{
    public partial class ResolvedAddressAuditControllerTests
    {
        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnGetIfServerErrorOccurredAsync(Xeption serverException)
        {
            // given 
            IQueryable<ResolvedAddressAudit> someResolvedAddressAudites = CreateRandomResolvedAddressAudits();

            InternalServerErrorObjectResult expectedInternalServerErrorObjectResult =
                InternalServerError(serverException);

            var expectedActionResult =
                new ActionResult<IQueryable<ResolvedAddressAudit>>(expectedInternalServerErrorObjectResult);

            this.resolvedAddressAuditServiceMock.Setup(service =>
                service.RetrieveAllResolvedAddressAuditsAsync())
                    .ThrowsAsync(serverException);

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
