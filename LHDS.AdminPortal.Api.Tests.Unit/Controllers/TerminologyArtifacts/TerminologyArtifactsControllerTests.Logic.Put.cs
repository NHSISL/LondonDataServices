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
using RESTFulSense.Models;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.TerminologyArtifacts
{
    public partial class TerminologyArtifactsControllerTests
    {
        [Fact]
        public async Task ShouldReturnCreatedOnPostAsync()
        {
            // given
            TerminologyArtifact randomTerminologyArtifact = CreateRandomTerminologyArtifact();
            TerminologyArtifact inputTerminologyArtifact = randomTerminologyArtifact;
            TerminologyArtifact addedTerminologyArtifact = inputTerminologyArtifact.DeepClone();
            TerminologyArtifact expectedTerminologyArtifact = addedTerminologyArtifact.DeepClone();

            var expectedObjectResult =
                new CreatedObjectResult(expectedTerminologyArtifact);

            var expectedActionResult =
                new ActionResult<TerminologyArtifact>(expectedObjectResult);

            this.terminologyArtifactsServiceMock.Setup(service =>
                service.AddTerminologyArtifactAsync(inputTerminologyArtifact))
                    .ReturnsAsync(addedTerminologyArtifact);

            // when
            ActionResult<TerminologyArtifact> actualActionResult =
                await this.terminologyArtifactsController.PostTerminologyArtifactAsync(inputTerminologyArtifact);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.terminologyArtifactsServiceMock.Verify(service =>
                service.AddTerminologyArtifactAsync(inputTerminologyArtifact),
                   Times.Once);

            this.terminologyArtifactsServiceMock.VerifyNoOtherCalls();
        }
    }
}
