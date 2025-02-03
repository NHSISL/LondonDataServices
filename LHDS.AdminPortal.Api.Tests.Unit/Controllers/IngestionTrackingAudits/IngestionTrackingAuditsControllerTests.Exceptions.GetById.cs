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
        public async Task ShouldReturnBadRequestOnGetByIdIfValidationErrorOccurredAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            var IngestionTrackingAuditValidationException =
                new IngestionTrackingAuditValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(IngestionTrackingAuditValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<IngestionTrackingAudit>(expectedBadRequestObjectResult);

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingAuditByIdAsync(someId))
                    .ThrowsAsync(IngestionTrackingAuditValidationException);

            // when
            ActionResult<IngestionTrackingAudit> actualActionResult =
                await this.ingestionTrackingAuditsController.GetAuditByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingAuditByIdAsync(someId),
                    Times.Once);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
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
                new ActionResult<IngestionTrackingAudit>(expectedInternalServerErrorObjectResult);

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serverException);

            // when
            ActionResult<IngestionTrackingAudit> actualActionResult =
                await this.ingestionTrackingAuditsController.GetAuditByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnGetByIdIfItemDoesNotExistAsync()
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
                service.RetrieveIngestionTrackingAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(IngestionTrackingAuditValidationException);

            // when
            ActionResult<IngestionTrackingAudit> actualActionResult =
                await this.ingestionTrackingAuditsController.GetAuditByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
        }
    }
}