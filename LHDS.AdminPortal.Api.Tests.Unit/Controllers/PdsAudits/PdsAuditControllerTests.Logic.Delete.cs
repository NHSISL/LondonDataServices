// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.PdsAudits;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.PdsAudits
{
    public partial class PdsAuditControllerTests
    {
        [Fact]
        public async Task ShouldReturnOkOnDeleteAsync()
        {
            // given
            PdsAudit randomPdsAudit = CreateRandomPdsAudit();
            Guid inputId = randomPdsAudit.Id;
            PdsAudit storagePdsAudit = randomPdsAudit.DeepClone();
            PdsAudit expectedPdsAudit = storagePdsAudit.DeepClone();

            var expectedObjectResult =
                new OkObjectResult(expectedPdsAudit);

            var expectedActionResult =
                new ActionResult<PdsAudit>(expectedObjectResult);

            this.pdsAuditServiceMock.Setup(service =>
                service.RemovePdsAuditByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(storagePdsAudit);

            // when
            ActionResult<PdsAudit> actualActionResult =
                await this.pdsAuditsController.DeletePdsAuditByIdAsync(inputId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.pdsAuditServiceMock.Verify(service =>
                service.RemovePdsAuditByIdAsync(It.IsAny<Guid>()),
                   Times.Once);

            this.pdsAuditServiceMock.VerifyNoOtherCalls();
        }
    }
}
