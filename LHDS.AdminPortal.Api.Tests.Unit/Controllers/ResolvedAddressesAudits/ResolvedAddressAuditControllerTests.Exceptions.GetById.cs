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
        public async Task ShouldReturnBadRequestOnGetByIdIfValidationErrorOccurredAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            var resolvedAddressAuditValidationException =
                new ResolvedAddressAuditValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(resolvedAddressAuditValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<ResolvedAddressAudit>(expectedBadRequestObjectResult);

            this.resolvedAddressAuditServiceMock.Setup(service =>
                service.RetrieveResolvedAddressAuditByIdAsync(someId))
                    .ThrowsAsync(resolvedAddressAuditValidationException);

            // when
            ActionResult<ResolvedAddressAudit> actualActionResult =
                await this.resolvedAddressAuditsController.GetResolvedAddressAuditByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.resolvedAddressAuditServiceMock.Verify(service =>
                service.RetrieveResolvedAddressAuditByIdAsync(someId),
                    Times.Once);

            this.resolvedAddressAuditServiceMock.VerifyNoOtherCalls();
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
                new ActionResult<ResolvedAddressAudit>(expectedInternalServerErrorObjectResult);

            this.resolvedAddressAuditServiceMock.Setup(service =>
                service.RetrieveResolvedAddressAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serverException);

            // when
            ActionResult<ResolvedAddressAudit> actualActionResult =
                await this.resolvedAddressAuditsController.GetResolvedAddressAuditByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.resolvedAddressAuditServiceMock.Verify(service =>
                service.RetrieveResolvedAddressAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.resolvedAddressAuditServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnGetByIdIfItemDoesNotExistAsync()
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
                service.RetrieveResolvedAddressAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(resolvedAddressAuditValidationException);

            // when
            ActionResult<ResolvedAddressAudit> actualActionResult =
                await this.resolvedAddressAuditsController.GetResolvedAddressAuditByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.resolvedAddressAuditServiceMock.Verify(service =>
                service.RetrieveResolvedAddressAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.resolvedAddressAuditServiceMock.VerifyNoOtherCalls();
        }
    }
}
