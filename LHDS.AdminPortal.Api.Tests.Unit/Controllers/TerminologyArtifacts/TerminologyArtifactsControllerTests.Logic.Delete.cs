// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.TerminologyArtifacts
{
    public partial class TerminologyArtifactsControllerTests
    {
        [Fact]
        public async Task ShouldReturnOkOnDeleteAsync()
        {
            // given
            TerminologyArtifact randomTerminologyArtifact = CreateRandomTerminologyArtifact();
            Guid inputId = randomTerminologyArtifact.Id;
            TerminologyArtifact storageTerminologyArtifact = randomTerminologyArtifact.DeepClone();
            TerminologyArtifact expectedTerminologyArtifact = storageTerminologyArtifact.DeepClone();

            var expectedObjectResult =
                new OkObjectResult(expectedTerminologyArtifact);

            var expectedActionResult =
                new ActionResult<TerminologyArtifact>(expectedObjectResult);

            this.terminologyArtifactsServiceMock.Setup(service =>
                service.RemoveTerminologyArtifactByIdAsync(inputId))
                    .ReturnsAsync(storageTerminologyArtifact);

            // when
            ActionResult<TerminologyArtifact> actualActionResult =
                await this.terminologyArtifactsController.DeleteTerminologyArtifactByIdAsync(inputId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.terminologyArtifactsServiceMock.Verify(service =>
                service.RemoveTerminologyArtifactByIdAsync(inputId),
                    Times.Once);

            this.terminologyArtifactsServiceMock.VerifyNoOtherCalls();
        }
    }
}
