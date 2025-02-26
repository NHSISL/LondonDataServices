// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        [Fact]
        public async Task ShouldReturnBadRequestOnPutIfValidationErrorOccurredAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            PdsAudit somePdsAudit = CreateRandomPdsAudit();

            var pdsAuditValidationException =
                new PdsAuditValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(pdsAuditValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<PdsAudit>(expectedBadRequestObjectResult);

            this.pdsAuditServiceMock.Setup(service =>
                service.ModifyPdsAuditAsync(It.IsAny<PdsAudit>()))
                    .ThrowsAsync(pdsAuditValidationException);

            // when
            ActionResult<PdsAudit> actualActionResult =
                await this.pdsAuditsController.PutPdsAuditAsync(somePdsAudit);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.pdsAuditServiceMock.Verify(service =>
                service.ModifyPdsAuditAsync(It.IsAny<PdsAudit>()),
                    Times.Once);

            this.pdsAuditServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnPutIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();
            PdsAudit somePdsAudit = CreateRandomPdsAudit();

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
                service.ModifyPdsAuditAsync(It.IsAny<PdsAudit>()))
                    .ThrowsAsync(pdsAuditValidationException);

            // when
            ActionResult<PdsAudit> actualActionResult =
                await this.pdsAuditsController.PutPdsAuditAsync(somePdsAudit);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.pdsAuditServiceMock.Verify(service =>
                service.ModifyPdsAuditAsync(It.IsAny<PdsAudit>()),
                    Times.Once);

            this.pdsAuditServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnFailedDependencyOnPutIfInvalidPdsAuditReferenceAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            PdsAudit somePdsAudit = CreateRandomPdsAudit();

            var alreadyExistsPdsAuditException =
                new InvalidPdsAuditReferenceException(
                    message: someMessage,
                    innerException: someInnerException);

            var pdsAuditDependencyValidationException =
                new PdsAuditDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsPdsAuditException);

            FailedDependencyObjectResult expectedBadRequestObjectResult =
                FailedDependency(pdsAuditDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<PdsAudit>(expectedBadRequestObjectResult);

            this.pdsAuditServiceMock.Setup(service =>
                service.ModifyPdsAuditAsync(It.IsAny<PdsAudit>()))
                    .ThrowsAsync(pdsAuditDependencyValidationException);

            // when
            ActionResult<PdsAudit> actualActionResult =
                await this.pdsAuditsController.PutPdsAuditAsync(somePdsAudit);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.pdsAuditServiceMock.Verify(service =>
                service.ModifyPdsAuditAsync(It.IsAny<PdsAudit>()),
                    Times.Once);

            this.pdsAuditServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnConflictOnPutIfAlreadyExistsPdsAuditErrorOccurredAsync()
        {
            // given
            PdsAudit somePdsAudit = CreateRandomPdsAudit();
            var someInnerException = new Exception();
            string someMessage = GetRandomString();

            var alreadyExistsPdsAuditException =
                new AlreadyExistsPdsAuditException(
                    message: someMessage,
                    innerException: someInnerException);

            var pdsAuditDependencyValidationException =
                new PdsAuditDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsPdsAuditException);

            ConflictObjectResult expectedConflictObjectResult =
                Conflict(alreadyExistsPdsAuditException);

            var expectedActionResult =
                new ActionResult<PdsAudit>(expectedConflictObjectResult);

            this.pdsAuditServiceMock.Setup(service =>
                service.ModifyPdsAuditAsync(It.IsAny<PdsAudit>()))
                    .ThrowsAsync(pdsAuditDependencyValidationException);

            // when
            ActionResult<PdsAudit> actualActionResult =
                await this.pdsAuditsController.PutPdsAuditAsync(somePdsAudit);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.pdsAuditServiceMock.Verify(service =>
                service.ModifyPdsAuditAsync(It.IsAny<PdsAudit>()),
                    Times.Once);

            this.pdsAuditServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnPutIfServerErrorOccurredAsync(
            Xeption validationException)
        {
            // given
            PdsAudit somePdsAudit = CreateRandomPdsAudit();

            InternalServerErrorObjectResult expectedBadRequestObjectResult =
                InternalServerError(validationException);

            var expectedActionResult =
                new ActionResult<PdsAudit>(expectedBadRequestObjectResult);

            this.pdsAuditServiceMock.Setup(service =>
                service.ModifyPdsAuditAsync(It.IsAny<PdsAudit>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<PdsAudit> actualActionResult =
                await this.pdsAuditsController.PutPdsAuditAsync(somePdsAudit);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.pdsAuditServiceMock.Verify(service =>
                service.ModifyPdsAuditAsync(It.IsAny<PdsAudit>()),
                    Times.Once);

            this.pdsAuditServiceMock.VerifyNoOtherCalls();
        }
    }
}
