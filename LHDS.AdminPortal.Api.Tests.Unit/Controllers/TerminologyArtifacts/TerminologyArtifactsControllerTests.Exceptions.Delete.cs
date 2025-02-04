// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.TerminologyArtifacts
{
    public partial class TerminologyArtifactsControllerTests
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
                new ActionResult<TerminologyArtifact>(expectedBadRequestObjectResult);

            this.terminologyArtifactsServiceMock.Setup(service =>
                service.RemoveTerminologyArtifactByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<TerminologyArtifact> actualActionResult =
                await this.terminologyArtifactsController.DeleteTerminologyArtifactByIdAsync(someId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.terminologyArtifactsServiceMock.Verify(service =>
                service.RemoveTerminologyArtifactByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.terminologyArtifactsServiceMock.VerifyNoOtherCalls();
        }
    }
}
