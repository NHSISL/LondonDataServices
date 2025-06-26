// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using Xunit;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.ResolvedAddressesAudits;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.ResolvedAddressAudits
{
    public partial class ResolvedAddressAuditControllerTests
    {
        [Fact]
        public async Task ShouldReturnOkOnPutAsync()
        {
            // given
            ResolvedAddressAudit randomResolvedAddressAudit = CreateRandomResolvedAddressAudit();
            ResolvedAddressAudit inputResolvedAddressAudit = randomResolvedAddressAudit;
            ResolvedAddressAudit storageResolvedAddressAudit = inputResolvedAddressAudit.DeepClone();
            ResolvedAddressAudit expectedResolvedAddressAudit = storageResolvedAddressAudit.DeepClone();

            var expectedObjectResult =
                new OkObjectResult(expectedResolvedAddressAudit);

            var expectedActionResult =
                new ActionResult<ResolvedAddressAudit>(expectedObjectResult);

            this.resolvedAddressAuditServiceMock.Setup(service =>
                service.ModifyResolvedAddressAuditAsync(inputResolvedAddressAudit))
                    .ReturnsAsync(storageResolvedAddressAudit);

            // when
            ActionResult<ResolvedAddressAudit> actualActionResult =
                await this.resolvedAddressAuditsController.PutResolvedAddressAuditAsync(inputResolvedAddressAudit);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.resolvedAddressAuditServiceMock.Verify(service =>
                service.ModifyResolvedAddressAuditAsync(inputResolvedAddressAudit),
                   Times.Once);

            this.resolvedAddressAuditServiceMock.VerifyNoOtherCalls();
        }
    }
}
