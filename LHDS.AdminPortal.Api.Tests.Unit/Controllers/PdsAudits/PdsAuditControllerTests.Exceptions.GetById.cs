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
        public async Task ShouldReturnBadRequestOnGetByIdIfValidationErrorOccurredAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            var pdsAuditValidationException =
                new PdsAuditValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(pdsAuditValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<PdsAudit>(expectedBadRequestObjectResult);

            this.pdsAuditServiceMock.Setup(service =>
                service.RetrievePdsAuditByIdAsync(someId))
                    .ThrowsAsync(pdsAuditValidationException);

            // when
            ActionResult<PdsAudit> actualActionResult =
                await this.pdsAuditsController.GetPdsAuditByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.pdsAuditServiceMock.Verify(service =>
                service.RetrievePdsAuditByIdAsync(someId),
                    Times.Once);

            this.pdsAuditServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnGetByIdIfServerErrorOccurredAsync(Xeption serverException)
        {
            // given 
            Guid someId = Guid.NewGuid();

            InternalServerErrorObjectResult expectedInternalServerErrorObjectResult =
                InternalServerError(serverException);

            var expectedActionResult =
                new ActionResult<PdsAudit>(expectedInternalServerErrorObjectResult);

            this.pdsAuditServiceMock.Setup(service =>
                service.RetrievePdsAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serverException);

            // when
            ActionResult<PdsAudit> actualActionResult =
                await this.pdsAuditsController.GetPdsAuditByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.pdsAuditServiceMock.Verify(service =>
                service.RetrievePdsAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.pdsAuditServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnGetByIdIfItemDoesNotExistAsync()
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
                service.RetrievePdsAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(pdsAuditValidationException);

            // when
            ActionResult<PdsAudit> actualActionResult =
                await this.pdsAuditsController.GetPdsAuditByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.pdsAuditServiceMock.Verify(service =>
                service.RetrievePdsAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.pdsAuditServiceMock.VerifyNoOtherCalls();
        }
    }
}
