// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.PdsAudites.Exceptions;
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Models.Foundations.PdsAudits.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.PdsAudits
{
    public partial class PdsAuditControllerTests
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
                new ActionResult<PdsAudit>(expectedBadRequestObjectResult);

            this.pdsAuditServiceMock.Setup(service =>
                service.RemovePdsAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<PdsAudit> actualActionResult =
                await this.pdsAuditsController.DeletePdsAuditByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.pdsAuditServiceMock.Verify(service =>
                service.RemovePdsAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.pdsAuditServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnDeleteIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();

            var notFoundPdsAuditException =
                new NotFoundPdsAuditException(
                    pdsAuditId: someId);

            var pdsAuditValidationException =
                new PdsAuditValidationException(
                    message: someMessage,
                    innerException: notFoundPdsAuditException);

            NotFoundObjectResult expectedNotFoundObjectResult =
                NotFound(notFoundPdsAuditException);

            var expectedActionResult =
                new ActionResult<PdsAudit>(expectedNotFoundObjectResult);

            this.pdsAuditServiceMock.Setup(service =>
                service.RemovePdsAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(pdsAuditValidationException);

            // when
            ActionResult<PdsAudit> actualActionResult =
                await this.pdsAuditsController.DeletePdsAuditByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.pdsAuditServiceMock.Verify(service =>
                service.RemovePdsAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.pdsAuditServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnLockedOnDeleteIfLockedPdsAuditAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            var lockedPdsAuditException =
                new LockedPdsAuditException(
                    message: someMessage,
                    innerException: someInnerException);

            var pdsAuditDependencyValidationException =
                new PdsAuditDependencyValidationException(
                    message: someMessage,
                    innerException: lockedPdsAuditException);

            LockedObjectResult expectedBadRequestObjectResult =
                Locked(pdsAuditDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<PdsAudit>(expectedBadRequestObjectResult);

            this.pdsAuditServiceMock.Setup(service =>
                service.RemovePdsAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(pdsAuditDependencyValidationException);

            // when
            ActionResult<PdsAudit> actualActionResult =
                await this.pdsAuditsController.DeletePdsAuditByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.pdsAuditServiceMock.Verify(service =>
                service.RemovePdsAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.pdsAuditServiceMock.VerifyNoOtherCalls();
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
                new ActionResult<PdsAudit>(expectedBadRequestObjectResult);

            this.pdsAuditServiceMock.Setup(service =>
                service.RemovePdsAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<PdsAudit> actualActionResult =
                await this.pdsAuditsController.DeletePdsAuditByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.pdsAuditServiceMock.Verify(service =>
                service.RemovePdsAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.pdsAuditServiceMock.VerifyNoOtherCalls();
        }
    }
}
