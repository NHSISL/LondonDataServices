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
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditsControllerTests
    {
        [Fact]
        public async Task ShouldReturnBadRequestOnPutIfValidationErrorOccurredAsync()
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
                service.ModifyIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()))
                    .ThrowsAsync(IngestionTrackingAuditValidationException);

            // when
            ActionResult<IngestionTrackingAudit> actualActionResult =
                await this.ingestionTrackingAuditsController.PutAuditAsync(someIngestionTrackingAudit);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.ModifyIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                    Times.Once);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
        }
    }
}
