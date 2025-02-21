// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using LHDS.Core.Models.Foundations.PdsAudits;
using RESTFulSense.Models;
using Xunit;
using Force.DeepCloner;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.PdsAudits
{
    public partial class PdsAuditControllerTests
    {
        [Fact]
        public async Task ShouldReturnCreatedOnPostAsync()
        {
            // given
            PdsAudit randomPdsAudit = CreateRandomPdsAudit();
            PdsAudit inputPdsAudit = randomPdsAudit;
            PdsAudit addedPdsAudit = inputPdsAudit.DeepClone();
            PdsAudit expectedPdsAudit = addedPdsAudit.DeepClone();

            var expectedObjectResult =
                new CreatedObjectResult(expectedPdsAudit);

            var expectedActionResult =
                new ActionResult<PdsAudit>(expectedObjectResult);

            this.pdsAuditServiceMock.Setup(service =>
                service.AddPdsAuditAsync(inputPdsAudit))
                    .ReturnsAsync(addedPdsAudit);

            // when
            ActionResult<PdsAudit> actualActionResult =
                await this.pdsAuditsController.PostPdsAuditAsync(inputPdsAudit);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.pdsAuditServiceMock.Verify(service =>
                service.AddPdsAuditAsync(inputPdsAudit),
                   Times.Once);

            this.pdsAuditServiceMock.VerifyNoOtherCalls();
        }
    }
}
