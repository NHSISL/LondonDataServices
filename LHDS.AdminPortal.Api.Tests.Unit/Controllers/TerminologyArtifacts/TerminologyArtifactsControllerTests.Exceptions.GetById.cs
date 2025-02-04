// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.TerminologyArtifacts
{
    public partial class TerminologyArtifactsControllerTests
    {
        [Fact]
        public async Task ShouldReturnBadRequestOnGetByIdIfValidationErrorOccurredAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            var TerminologyArtifactValidationException =
                new TerminologyArtifactValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(TerminologyArtifactValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<TerminologyArtifact>(expectedBadRequestObjectResult);

            this.terminologyArtifactsServiceMock.Setup(service =>
                service.RetrieveTerminologyArtifactByIdAsync(someId))
                    .ThrowsAsync(TerminologyArtifactValidationException);

            // when
            ActionResult<TerminologyArtifact> actualActionResult =
                await this.terminologyArtifactsController.GetTerminologyArtifactByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.terminologyArtifactsServiceMock.Verify(service =>
                service.RetrieveTerminologyArtifactByIdAsync(someId),
                    Times.Once);

            this.terminologyArtifactsServiceMock.VerifyNoOtherCalls();
        }
    }
}
