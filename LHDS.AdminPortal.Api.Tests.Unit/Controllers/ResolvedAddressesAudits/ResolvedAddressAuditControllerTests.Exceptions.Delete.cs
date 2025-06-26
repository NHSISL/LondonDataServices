// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ResolvedAddressAudits.Exceptions;
using LHDS.Core.Models.Foundations.ResolvedAddressesAudits;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.ResolvedAddressAudits
{
    public partial class ResolvedAddressAuditControllerTests
    {
        [Theory]
        [MemberData(nameof(ValidationExceptions))]
        public async Task ShouldReturnBadRequestOnDeleteIfValidationErrorOccurredAsync(Xeption validationException)
        {
            // given
            Guid someId = Guid.NewGuid();

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(validationException.InnerException);

            var expectedActionResult =
                new ActionResult<ResolvedAddressAudit>(expectedBadRequestObjectResult);

            this.resolvedAddressAuditServiceMock.Setup(service =>
                service.RemoveResolvedAddressAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<ResolvedAddressAudit> actualActionResult =
                await this.resolvedAddressAuditsController.DeleteResolvedAddressAuditByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.resolvedAddressAuditServiceMock.Verify(service =>
                service.RemoveResolvedAddressAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.resolvedAddressAuditServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnDeleteIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();

            var notFoundResolvedAddressAuditException =
                new NotFoundResolvedAddressAuditException(
                    resolvedAddressAuditId: someId);

            var resolvedAddressAuditValidationException =
                new ResolvedAddressAuditValidationException(
                    message: someMessage,
                    innerException: notFoundResolvedAddressAuditException);

            NotFoundObjectResult expectedNotFoundObjectResult =
                NotFound(notFoundResolvedAddressAuditException);

            var expectedActionResult =
                new ActionResult<ResolvedAddressAudit>(expectedNotFoundObjectResult);

            this.resolvedAddressAuditServiceMock.Setup(service =>
                service.RemoveResolvedAddressAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(resolvedAddressAuditValidationException);

            // when
            ActionResult<ResolvedAddressAudit> actualActionResult =
                await this.resolvedAddressAuditsController.DeleteResolvedAddressAuditByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.resolvedAddressAuditServiceMock.Verify(service =>
                service.RemoveResolvedAddressAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.resolvedAddressAuditServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnLockedOnDeleteIfLockedResolvedAddressAuditAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            var lockedResolvedAddressAuditException =
                new LockedResolvedAddressAuditException(
                    message: someMessage,
                    innerException: someInnerException);

            var resolvedAddressAuditDependencyValidationException =
                new ResolvedAddressAuditDependencyValidationException(
                    message: someMessage,
                    innerException: lockedResolvedAddressAuditException);

            LockedObjectResult expectedBadRequestObjectResult =
                Locked(resolvedAddressAuditDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<ResolvedAddressAudit>(expectedBadRequestObjectResult);

            this.resolvedAddressAuditServiceMock.Setup(service =>
                service.RemoveResolvedAddressAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(resolvedAddressAuditDependencyValidationException);

            // when
            ActionResult<ResolvedAddressAudit> actualActionResult =
                await this.resolvedAddressAuditsController.DeleteResolvedAddressAuditByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.resolvedAddressAuditServiceMock.Verify(service =>
                service.RemoveResolvedAddressAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.resolvedAddressAuditServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnDeleteIfServerErrorOccurredAsync(
            Xeption validationException)
        {
            // given
            Guid someId = Guid.NewGuid();

            InternalServerErrorObjectResult expectedBadRequestObjectResult =
                InternalServerError(validationException);

            var expectedActionResult =
                new ActionResult<ResolvedAddressAudit>(expectedBadRequestObjectResult);

            this.resolvedAddressAuditServiceMock.Setup(service =>
                service.RemoveResolvedAddressAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<ResolvedAddressAudit> actualActionResult =
                await this.resolvedAddressAuditsController.DeleteResolvedAddressAuditByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.resolvedAddressAuditServiceMock.Verify(service =>
                service.RemoveResolvedAddressAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.resolvedAddressAuditServiceMock.VerifyNoOtherCalls();
        }
    }
}
