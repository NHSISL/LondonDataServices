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
using RESTFulSense.Models;
using Xeptions;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.PdsAudits
{
    public partial class PdsAuditControllerTests
    {
        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnGetIfServerErrorOccurredAsync(Xeption serverException)
        {
            // given 
            IQueryable<PdsAudit> somePdsAudites = CreateRandomPdsAudits();

            InternalServerErrorObjectResult expectedInternalServerErrorObjectResult =
                InternalServerError(serverException);

            var expectedActionResult =
                new ActionResult<IQueryable<PdsAudit>>(expectedInternalServerErrorObjectResult);

            this.pdsAuditServiceMock.Setup(service =>
                service.RetrieveAllPdsAuditsAsync())
                    .ThrowsAsync(serverException);

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
