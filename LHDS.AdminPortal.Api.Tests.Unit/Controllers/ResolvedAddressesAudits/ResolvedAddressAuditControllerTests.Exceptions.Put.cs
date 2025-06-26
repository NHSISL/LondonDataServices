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
        [Fact]
        public async Task ShouldReturnBadRequestOnPutIfValidationErrorOccurredAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            ResolvedAddressAudit someResolvedAddressAudit = CreateRandomResolvedAddressAudit();

            var resolvedAddressAuditValidationException =
                new ResolvedAddressAuditValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(resolvedAddressAuditValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<ResolvedAddressAudit>(expectedBadRequestObjectResult);

            this.resolvedAddressAuditServiceMock.Setup(service =>
                service.ModifyResolvedAddressAuditAsync(It.IsAny<ResolvedAddressAudit>()))
                    .ThrowsAsync(resolvedAddressAuditValidationException);

            // when
            ActionResult<ResolvedAddressAudit> actualActionResult =
                await this.resolvedAddressAuditsController.PutResolvedAddressAuditAsync(someResolvedAddressAudit);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.resolvedAddressAuditServiceMock.Verify(service =>
                service.ModifyResolvedAddressAuditAsync(It.IsAny<ResolvedAddressAudit>()),
                    Times.Once);

            this.resolvedAddressAuditServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnPutIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();
            ResolvedAddressAudit someResolvedAddressAudit = CreateRandomResolvedAddressAudit();

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
                service.ModifyResolvedAddressAuditAsync(It.IsAny<ResolvedAddressAudit>()))
                    .ThrowsAsync(resolvedAddressAuditValidationException);

            // when
            ActionResult<ResolvedAddressAudit> actualActionResult =
                await this.resolvedAddressAuditsController.PutResolvedAddressAuditAsync(someResolvedAddressAudit);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.resolvedAddressAuditServiceMock.Verify(service =>
                service.ModifyResolvedAddressAuditAsync(It.IsAny<ResolvedAddressAudit>()),
                    Times.Once);

            this.resolvedAddressAuditServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnFailedDependencyOnPutIfInvalidResolvedAddressAuditReferenceAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            ResolvedAddressAudit someResolvedAddressAudit = CreateRandomResolvedAddressAudit();

            var alreadyExistsResolvedAddressAuditException =
                new InvalidResolvedAddressAuditReferenceException(
                    message: someMessage,
                    innerException: someInnerException);

            var resolvedAddressAuditDependencyValidationException =
                new ResolvedAddressAuditDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsResolvedAddressAuditException);

            FailedDependencyObjectResult expectedBadRequestObjectResult =
                FailedDependency(resolvedAddressAuditDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<ResolvedAddressAudit>(expectedBadRequestObjectResult);

            this.resolvedAddressAuditServiceMock.Setup(service =>
                service.ModifyResolvedAddressAuditAsync(It.IsAny<ResolvedAddressAudit>()))
                    .ThrowsAsync(resolvedAddressAuditDependencyValidationException);

            // when
            ActionResult<ResolvedAddressAudit> actualActionResult =
                await this.resolvedAddressAuditsController.PutResolvedAddressAuditAsync(someResolvedAddressAudit);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.resolvedAddressAuditServiceMock.Verify(service =>
                service.ModifyResolvedAddressAuditAsync(It.IsAny<ResolvedAddressAudit>()),
                    Times.Once);

            this.resolvedAddressAuditServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnConflictOnPutIfAlreadyExistsResolvedAddressAuditErrorOccurredAsync()
        {
            // given
            ResolvedAddressAudit someResolvedAddressAudit = CreateRandomResolvedAddressAudit();
            var someInnerException = new Exception();
            string someMessage = GetRandomString();

            var alreadyExistsResolvedAddressAuditException =
                new AlreadyExistsResolvedAddressAuditException(
                    message: someMessage,
                    innerException: someInnerException);

            var resolvedAddressAuditDependencyValidationException =
                new ResolvedAddressAuditDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsResolvedAddressAuditException);

            ConflictObjectResult expectedConflictObjectResult =
                Conflict(alreadyExistsResolvedAddressAuditException);

            var expectedActionResult =
                new ActionResult<ResolvedAddressAudit>(expectedConflictObjectResult);

            this.resolvedAddressAuditServiceMock.Setup(service =>
                service.ModifyResolvedAddressAuditAsync(It.IsAny<ResolvedAddressAudit>()))
                    .ThrowsAsync(resolvedAddressAuditDependencyValidationException);

            // when
            ActionResult<ResolvedAddressAudit> actualActionResult =
                await this.resolvedAddressAuditsController.PutResolvedAddressAuditAsync(someResolvedAddressAudit);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.resolvedAddressAuditServiceMock.Verify(service =>
                service.ModifyResolvedAddressAuditAsync(It.IsAny<ResolvedAddressAudit>()),
                    Times.Once);

            this.resolvedAddressAuditServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnPutIfServerErrorOccurredAsync(
            Xeption validationException)
        {
            // given
            ResolvedAddressAudit someResolvedAddressAudit = CreateRandomResolvedAddressAudit();

            InternalServerErrorObjectResult expectedBadRequestObjectResult =
                InternalServerError(validationException);

            var expectedActionResult =
                new ActionResult<ResolvedAddressAudit>(expectedBadRequestObjectResult);

            this.resolvedAddressAuditServiceMock.Setup(service =>
                service.ModifyResolvedAddressAuditAsync(It.IsAny<ResolvedAddressAudit>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<ResolvedAddressAudit> actualActionResult =
                await this.resolvedAddressAuditsController.PutResolvedAddressAuditAsync(someResolvedAddressAudit);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.resolvedAddressAuditServiceMock.Verify(service =>
                service.ModifyResolvedAddressAuditAsync(It.IsAny<ResolvedAddressAudit>()),
                    Times.Once);

            this.resolvedAddressAuditServiceMock.VerifyNoOtherCalls();
        }
    }
}
