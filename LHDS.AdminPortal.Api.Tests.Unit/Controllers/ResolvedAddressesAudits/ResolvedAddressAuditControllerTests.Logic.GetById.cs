// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.ResolvedAddressesAudits;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.ResolvedAddressAudits
{
    public partial class ResolvedAddressAuditControllerTests
    {
        [Fact]
        public async Task ShouldReturnResolvedAddressAuditOnGetByIdAsync()
        {
            // given 
            ResolvedAddressAudit randomResolvedAddressAudit = CreateRandomResolvedAddressAudit();
            Guid inputId = randomResolvedAddressAudit.Id;
            ResolvedAddressAudit storageResolvedAddressAudit = randomResolvedAddressAudit.DeepClone();
            ResolvedAddressAudit expectedResolvedAddressAudit = storageResolvedAddressAudit.DeepClone();
            var expectedObjectResult = new OkObjectResult(expectedResolvedAddressAudit);

            var expectedActionResult =
                new ActionResult<ResolvedAddressAudit>(expectedObjectResult);

            this.resolvedAddressAuditServiceMock.Setup(service =>
                service.RetrieveResolvedAddressAuditByIdAsync(inputId))
                    .ReturnsAsync(expectedResolvedAddressAudit);

            // when
            ActionResult<ResolvedAddressAudit> actualActionResult =
                await this.resolvedAddressAuditsController.GetResolvedAddressAuditByIdAsync(inputId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.resolvedAddressAuditServiceMock.Verify(service =>
                service.RetrieveResolvedAddressAuditByIdAsync(inputId),
                    Times.Once());

            this.resolvedAddressAuditServiceMock.VerifyNoOtherCalls();
        }
    }
}
