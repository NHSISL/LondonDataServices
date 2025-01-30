// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditsControllerTests
    {
        [Theory]
        [MemberData(nameof(ValidationExceptions))]
        public async Task ShouldReturnBadRequestOnDeleteIfValidationErrorOccurredAsync(Xeption validationException)
        {
            // given
            Guid someId = Guid.NewGuid();

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(validationException.InnerException ?? validationException);

            var expectedActionResult =
                new ActionResult<IngestionTrackingAudit>(expectedBadRequestObjectResult);

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.RemoveIngestionTrackingAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<IngestionTrackingAudit> actualActionResult =
                await this.ingestionTrackingAuditsController.DeleteAuditByIdAsync(someId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.RemoveIngestionTrackingAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnDeleteIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();

            var notFoundIngestionTrackingAuditException =
                new NotFoundIngestionTrackingAuditException(
                    ingestionTrackingAuditId: someId);

            var IngestionTrackingAuditValidationException =
                new IngestionTrackingAuditValidationException(
                    message: someMessage,
                    innerException: notFoundIngestionTrackingAuditException);

            NotFoundObjectResult expectedNotFoundObjectResult =
                NotFound(notFoundIngestionTrackingAuditException);

            var expectedActionResult =
                new ActionResult<IngestionTrackingAudit>(expectedNotFoundObjectResult);

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.RemoveIngestionTrackingAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(IngestionTrackingAuditValidationException);

            // when
            ActionResult<IngestionTrackingAudit> actualActionResult =
                await this.ingestionTrackingAuditsController.DeleteAuditByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.RemoveIngestionTrackingAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnLockedOnDeleteIfLockedIngestionTrackingAuditAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            var lockedIngestionTrackingAuditException =
                new LockedIngestionTrackingAuditException(
                    message: someMessage,
                    innerException: someInnerException);

            var IngestionTrackingAuditDependencyValidationException =
                new IngestionTrackingAuditDependencyValidationException(
                    message: someMessage,
                    innerException: lockedIngestionTrackingAuditException);

            LockedObjectResult expectedBadRequestObjectResult =
                Locked(IngestionTrackingAuditDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<IngestionTrackingAudit>(expectedBadRequestObjectResult);

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.RemoveIngestionTrackingAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(IngestionTrackingAuditDependencyValidationException);

            // when
            ActionResult<IngestionTrackingAudit> actualActionResult =
                await this.ingestionTrackingAuditsController.DeleteAuditByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.RemoveIngestionTrackingAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
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
                new ActionResult<IngestionTrackingAudit>(expectedBadRequestObjectResult);

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.RemoveIngestionTrackingAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<IngestionTrackingAudit> actualActionResult =
                await this.ingestionTrackingAuditsController.DeleteAuditByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.RemoveIngestionTrackingAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
        }
    }
}
