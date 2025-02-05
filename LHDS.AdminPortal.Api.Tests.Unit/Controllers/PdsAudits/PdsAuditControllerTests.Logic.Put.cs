// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using LHDS.Core.Models.Foundations.PdsAudits;
using RESTFulSense.Clients.Extensions;
using Xunit;
using Force.DeepCloner;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.PdsAudits
{
    public partial class PdsAuditControllerTests
    {
        [Fact]
        public async Task ShouldReturnOkOnPutAsync()
        {
            // given
            PdsAudit randomPdsAudit = CreateRandomPdsAudit();
            PdsAudit inputPdsAudit = randomPdsAudit;
            PdsAudit storagePdsAudit = inputPdsAudit.DeepClone();
            PdsAudit expectedPdsAudit = storagePdsAudit.DeepClone();

            var expectedObjectResult =
                new OkObjectResult(expectedPdsAudit);

            var expectedActionResult =
                new ActionResult<PdsAudit>(expectedObjectResult);

            this.pdsAuditServiceMock.Setup(service =>
                service.ModifyPdsAuditAsync(inputPdsAudit))
                    .ReturnsAsync(storagePdsAudit);

            // when
            ActionResult<PdsAudit> actualActionResult =
                await this.pdsAuditsController.PutPdsAuditAsync(inputPdsAudit);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.pdsAuditServiceMock.Verify(service =>
                service.ModifyPdsAuditAsync(inputPdsAudit),
                   Times.Once);

            this.pdsAuditServiceMock.VerifyNoOtherCalls();
        }
    }
}
