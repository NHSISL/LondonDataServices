// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using LHDS.Core.Models.Foundations.PdsAudits;
using RESTFulSense.Models;
using Xunit;
using LHDS.Core.Models.Foundations.PdsAudits.Exceptions;
using RESTFulSense.Models;
using System;
using Xeptions;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.PdsAudits
{
    public partial class PdsAuditControllerTests
    {
        [Fact]
        public async Task ShouldReturnBadRequestOnPostIfValidationErrorOccurredAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            PdsAudit somePdsAudit = CreateRandomPdsAudit();

            var PdsAuditValidationException =
                new PdsAuditValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult = 
                BadRequest(PdsAuditValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<PdsAudit>(expectedBadRequestObjectResult);

            this.pdsAuditServiceMock.Setup(service =>
                service.AddPdsAuditAsync(somePdsAudit))
                    .ThrowsAsync(PdsAuditValidationException);

            // when
            ActionResult<PdsAudit> actualActionResult =
                await this.pdsAuditsController.PostPdsAuditAsync(somePdsAudit);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.pdsAuditServiceMock.Verify(service =>
                service.AddPdsAuditAsync(somePdsAudit),
                    Times.Once);

            this.pdsAuditServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnPostIfServerErrorOccurredAsync(
            Xeption validationException)
        {
            // given
            PdsAudit somePdsAudit = CreateRandomPdsAudit();

            InternalServerErrorObjectResult expectedBadRequestObjectResult =
                InternalServerError(validationException);

            var expectedActionResult =
                new ActionResult<PdsAudit>(expectedBadRequestObjectResult);

            this.pdsAuditServiceMock.Setup(service =>
                service.AddPdsAuditAsync(somePdsAudit))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<PdsAudit> actualActionResult =
                await this.pdsAuditsController.PostPdsAuditAsync(somePdsAudit);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.pdsAuditServiceMock.Verify(service =>
                service.AddPdsAuditAsync(somePdsAudit),
                    Times.Once);

            this.pdsAuditServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnConflictOnPostIfAlreadyExistsPdsAuditErrorOccurredAsync()
        {
            // given
            PdsAudit somePdsAudit = CreateRandomPdsAudit();
            var someInnerException = new Exception();
            string someMessage = GetRandomString();

            var alreadyExistsPdsAuditException =
                new AlreadyExistsPdsAuditException(
                    message: someMessage,
                    innerException: someInnerException);

            var PdsAuditDependencyValidationException =
                new PdsAuditDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsPdsAuditException);

            ConflictObjectResult expectedConflictObjectResult =
                Conflict(alreadyExistsPdsAuditException);

            var expectedActionResult =
                new ActionResult<PdsAudit>(expectedConflictObjectResult);

            this.pdsAuditServiceMock.Setup(service =>
                service.AddPdsAuditAsync(It.IsAny<PdsAudit>()))
                    .ThrowsAsync(PdsAuditDependencyValidationException);

            // when
            ActionResult<PdsAudit> actualActionResult =
                await this.pdsAuditsController.PostPdsAuditAsync(somePdsAudit);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.pdsAuditServiceMock.Verify(service =>
                service.AddPdsAuditAsync(It.IsAny<PdsAudit>()),
                    Times.Once);

            this.pdsAuditServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnFailedDependencyOnPostIfInvalidPdsAuditReferenceAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            PdsAudit somePdsAudit = CreateRandomPdsAudit();

            var alreadyExistsPdsAuditException =
                new InvalidPdsAuditReferenceException(
                    message: someMessage,
                    innerException: someInnerException);

            var PdsAuditDependencyValidationException =
                new PdsAuditDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsPdsAuditException);

            FailedDependencyObjectResult expectedBadRequestObjectResult =
                FailedDependency(PdsAuditDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<PdsAudit>(expectedBadRequestObjectResult);

            this.pdsAuditServiceMock.Setup(service =>
                service.AddPdsAuditAsync(somePdsAudit))
                    .ThrowsAsync(PdsAuditDependencyValidationException);

            // when
            ActionResult<PdsAudit> actualActionResult =
                await this.pdsAuditsController.PostPdsAuditAsync(somePdsAudit);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.pdsAuditServiceMock.Verify(service =>
                service.AddPdsAuditAsync(somePdsAudit),
                    Times.Once);

            this.pdsAuditServiceMock.VerifyNoOtherCalls();
        }
    }
}
