// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        [Fact]
        public async Task ShouldReturnBadRequestOnPostIfValidationErrorOccurredAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            IngestionTrackingAudit someIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();

            var IngestionTrackingAuditValidationException =
                new IngestionTrackingAuditValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(IngestionTrackingAuditValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<IngestionTrackingAudit>(expectedBadRequestObjectResult);

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.AddIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()))
                    .ThrowsAsync(IngestionTrackingAuditValidationException);

            // when
            ActionResult<IngestionTrackingAudit> actualActionResult =
                await this.ingestionTrackingAuditsController.PostAuditAsync(someIngestionTrackingAudit);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.AddIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                    Times.Once);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnPostIfServerErrorOccurredAsync(
            Xeption validationException)
        {
            // given
            IngestionTrackingAudit someIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();

            InternalServerErrorObjectResult expectedBadRequestObjectResult =
                InternalServerError(validationException);

            var expectedActionResult =
                new ActionResult<IngestionTrackingAudit>(expectedBadRequestObjectResult);

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.AddIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<IngestionTrackingAudit> actualActionResult =
                await this.ingestionTrackingAuditsController.PostAuditAsync(someIngestionTrackingAudit);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.AddIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                    Times.Once);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnConflictOnPostIfAlreadyExistsIngestionTrackingAuditErrorOccurredAsync()
        {
            // given
            IngestionTrackingAudit someIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();
            var someInnerException = new Exception();
            string someMessage = GetRandomString();

            var alreadyExistsIngestionTrackingAuditException =
                new AlreadyExistsIngestionTrackingAuditException(
                    message: someMessage,
                    innerException: someInnerException);

            var IngestionTrackingAuditDependencyValidationException =
                new IngestionTrackingAuditDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsIngestionTrackingAuditException);

            ConflictObjectResult expectedConflictObjectResult =
                Conflict(alreadyExistsIngestionTrackingAuditException);

            var expectedActionResult =
                new ActionResult<IngestionTrackingAudit>(expectedConflictObjectResult);

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.AddIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()))
                    .ThrowsAsync(IngestionTrackingAuditDependencyValidationException);

            // when
            ActionResult<IngestionTrackingAudit> actualActionResult =
                await this.ingestionTrackingAuditsController.PostAuditAsync(someIngestionTrackingAudit);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.AddIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                    Times.Once);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnFailedDependencyOnPostIfInvalidIngestionTrackingAuditReferenceAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            IngestionTrackingAudit someIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();

            var alreadyExistsIngestionTrackingAuditException =
                new InvalidIngestionTrackingAuditReferenceException(
                    message: someMessage,
                    innerException: someInnerException);

            var IngestionTrackingAuditDependencyValidationException =
                new IngestionTrackingAuditDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsIngestionTrackingAuditException);

            FailedDependencyObjectResult expectedBadRequestObjectResult =
                FailedDependency(IngestionTrackingAuditDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<IngestionTrackingAudit>(expectedBadRequestObjectResult);

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.AddIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()))
                    .ThrowsAsync(IngestionTrackingAuditDependencyValidationException);

            // when
            ActionResult<IngestionTrackingAudit> actualActionResult =
                await this.ingestionTrackingAuditsController.PostAuditAsync(someIngestionTrackingAudit);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.AddIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                    Times.Once);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnConflictOnPostIfAlreadyExistsIngestionTrackingAuditErrorOccurredAsync()
        {
            // given
            IngestionTrackingAudit someIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();
            var someInnerException = new Exception();
            string someMessage = GetRandomString();

            var alreadyExistsIngestionTrackingAuditException =
                new AlreadyExistsIngestionTrackingAuditException(
                    message: someMessage,
                    innerException: someInnerException);

            var IngestionTrackingAuditDependencyValidationException =
                new IngestionTrackingAuditDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsIngestionTrackingAuditException);

            ConflictObjectResult expectedConflictObjectResult =
                Conflict(alreadyExistsIngestionTrackingAuditException);

            var expectedActionResult =
                new ActionResult<IngestionTrackingAudit>(expectedConflictObjectResult);

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.AddIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()))
                    .ThrowsAsync(IngestionTrackingAuditDependencyValidationException);

            // when
            ActionResult<IngestionTrackingAudit> actualActionResult =
                await this.ingestionTrackingAuditsController.PostAuditAsync(someIngestionTrackingAudit);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.AddIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                    Times.Once);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnFailedDependencyOnPostIfInvalidIngestionTrackingAuditReferenceAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            IngestionTrackingAudit someIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();

            var alreadyExistsIngestionTrackingAuditException =
                new InvalidIngestionTrackingAuditReferenceException(
                    message: someMessage,
                    innerException: someInnerException);

            var IngestionTrackingAuditDependencyValidationException =
                new IngestionTrackingAuditDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsIngestionTrackingAuditException);

            FailedDependencyObjectResult expectedBadRequestObjectResult =
                FailedDependency(IngestionTrackingAuditDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<IngestionTrackingAudit>(expectedBadRequestObjectResult);

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.AddIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()))
                    .ThrowsAsync(IngestionTrackingAuditDependencyValidationException);

            // when
            ActionResult<IngestionTrackingAudit> actualActionResult =
                await this.ingestionTrackingAuditsController.PostAuditAsync(someIngestionTrackingAudit);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.AddIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                    Times.Once);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
        }
    }
}
