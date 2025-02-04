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
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.TerminologyArtifacts
{
    public partial class TerminologyArtifactsControllerTests
    {
        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnGetIfServerErrorOccurredAsync(Xeption serverException)
        {
            // given 
            IQueryable<TerminologyArtifact> someTerminologyArtifacts = CreateRandomTerminologyArtifacts();

            InternalServerErrorObjectResult expectedInternalServerErrorObjectResult =
                InternalServerError(serverException);

            var expectedActionResult =
                new ActionResult<IQueryable<TerminologyArtifact>>(expectedInternalServerErrorObjectResult);

            this.terminologyArtifactsServiceMock.Setup(service =>
                service.RetrieveAllTerminologyArtifactsAsync())
                    .ThrowsAsync(serverException);

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
