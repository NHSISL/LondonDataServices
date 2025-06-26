// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xunit;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.ResolvedAddressesAudits;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.ResolvedAddressAudits
{
    public partial class ResolvedAddressAuditControllerTests
    {
        [Fact]
        public async Task ShouldReturnCreatedOnPostAsync()
        {
            // given
            ResolvedAddressAudit randomResolvedAddressAudit = CreateRandomResolvedAddressAudit();
            ResolvedAddressAudit inputResolvedAddressAudit = randomResolvedAddressAudit;
            ResolvedAddressAudit addedResolvedAddressAudit = inputResolvedAddressAudit.DeepClone();
            ResolvedAddressAudit expectedResolvedAddressAudit = addedResolvedAddressAudit.DeepClone();

            var expectedObjectResult =
                new CreatedObjectResult(expectedResolvedAddressAudit);

            var expectedActionResult =
                new ActionResult<ResolvedAddressAudit>(expectedObjectResult);

            this.resolvedAddressAuditServiceMock.Setup(service =>
                service.AddResolvedAddressAuditAsync(inputResolvedAddressAudit))
                    .ReturnsAsync(addedResolvedAddressAudit);

            // when
            ActionResult<ResolvedAddressAudit> actualActionResult =
                await this.resolvedAddressAuditsController.PostResolvedAddressAuditAsync(inputResolvedAddressAudit);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.resolvedAddressAuditServiceMock.Verify(service =>
                service.AddResolvedAddressAuditAsync(inputResolvedAddressAudit),
                   Times.Once);

            this.resolvedAddressAuditServiceMock.VerifyNoOtherCalls();
        }
    }
}
