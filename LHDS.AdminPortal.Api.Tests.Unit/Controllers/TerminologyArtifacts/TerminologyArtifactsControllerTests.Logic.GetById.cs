// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.TerminologyArtifacts
{
    public partial class TerminologyArtifactsControllerTests
    {
        [Fact]
        public async Task ShouldReturnTerminologyArtifactOnGetByIdAsync()
        {
            // given 
            TerminologyArtifact randomTerminologyArtifact = CreateRandomTerminologyArtifact();
            Guid inputId = randomTerminologyArtifact.Id;
            TerminologyArtifact storageTerminologyArtifact = randomTerminologyArtifact.DeepClone();
            TerminologyArtifact expectedTerminologyArtifact = storageTerminologyArtifact.DeepClone();
            var expectedObjectResult = new OkObjectResult(expectedTerminologyArtifact);

            var expectedActionResult =
                new ActionResult<TerminologyArtifact>(expectedObjectResult);

            this.terminologyArtifactsServiceMock.Setup(service =>
                service.RetrieveTerminologyArtifactByIdAsync(inputId))
                    .ReturnsAsync(expectedTerminologyArtifact);

            // when
            ActionResult<TerminologyArtifact> actualActionResult =
                await this.terminologyArtifactsController.GetTerminologyArtifactByIdAsync(inputId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.terminologyArtifactsServiceMock.Verify(service =>
                service.RetrieveTerminologyArtifactByIdAsync(inputId),
                    Times.Once);

            this.terminologyArtifactsServiceMock.VerifyNoOtherCalls();
        }
    }
}
