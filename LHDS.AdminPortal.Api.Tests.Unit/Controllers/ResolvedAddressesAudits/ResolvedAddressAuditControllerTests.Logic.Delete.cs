// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.ResolvedAddressesAudits;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.ResolvedAddressAudits
{
    public partial class ResolvedAddressAuditControllerTests
    {
        [Fact]
        public async Task ShouldReturnOkOnDeleteAsync()
        {
            // given
            ResolvedAddressAudit randomResolvedAddressAudit = CreateRandomResolvedAddressAudit();
            Guid inputId = randomResolvedAddressAudit.Id;
            ResolvedAddressAudit storageResolvedAddressAudit = randomResolvedAddressAudit.DeepClone();
            ResolvedAddressAudit expectedResolvedAddressAudit = storageResolvedAddressAudit.DeepClone();

            var expectedObjectResult =
                new OkObjectResult(expectedResolvedAddressAudit);

            var expectedActionResult =
                new ActionResult<ResolvedAddressAudit>(expectedObjectResult);

            this.resolvedAddressAuditServiceMock.Setup(service =>
                service.RemoveResolvedAddressAuditByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(storageResolvedAddressAudit);

            // when
            ActionResult<ResolvedAddressAudit> actualActionResult =
                await this.resolvedAddressAuditsController.DeleteResolvedAddressAuditByIdAsync(inputId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.resolvedAddressAuditServiceMock.Verify(service =>
                service.RemoveResolvedAddressAuditByIdAsync(It.IsAny<Guid>()),
                   Times.Once);

            this.resolvedAddressAuditServiceMock.VerifyNoOtherCalls();
        }
    }
}
