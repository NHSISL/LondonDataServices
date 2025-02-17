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
        public async Task ShouldReturnTerminologyArtifactsOnGetAsync()
        {
            // given 
            IQueryable<TerminologyArtifact> randomTerminologyArtifact = CreateRandomTerminologyArtifacts();
            IQueryable<TerminologyArtifact> storageTerminologyArtifact = randomTerminologyArtifact.DeepClone();
            IQueryable<TerminologyArtifact> expectedTerminologyArtifact = storageTerminologyArtifact.DeepClone();
            var expectedObjectResult = new OkObjectResult(expectedTerminologyArtifact);

            var expectedActionResult =
                new ActionResult<IQueryable<TerminologyArtifact>>(expectedObjectResult);

            this.terminologyArtifactsServiceMock.Setup(service =>
                service.RetrieveAllTerminologyArtifactsAsync())
                    .ReturnsAsync(expectedTerminologyArtifact);

            // when
            ActionResult<IQueryable<TerminologyArtifact>> actualActionResult =
                await this.terminologyArtifactsController.Get();

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.terminologyArtifactsServiceMock.Verify(service =>
                service.RetrieveAllTerminologyArtifactsAsync(),
                    Times.Once());

            this.terminologyArtifactsServiceMock.VerifyNoOtherCalls();
        }
    }
}
