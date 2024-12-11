// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.TerminologyArtifacts
{
    public partial class TerminologyArtifactProcessingServiceTests
    {
        [Fact]
        public async Task ShouldGetNonDownloadedTerminologyArtifact()
        {
            // given
            List<TerminologyArtifact> randomTerminologyArtifacts = CreateRandomTerminologyArtifacts();
            List<TerminologyArtifact> artifactsList = randomTerminologyArtifacts.ToList();
            artifactsList.Last().IsCore = true;
            artifactsList.Last().IsDownloaded = false;
            artifactsList.Last().IsError = false;
            TerminologyArtifact expectedTerminologyArtifact = artifactsList.Last();
            List<TerminologyArtifact> outputTerminologyArtifacts = artifactsList;

            this.terminologyArtifactServiceMock.Setup(service =>
                service.RetrieveAllTerminologyArtifacts())
                    .Returns(outputTerminologyArtifacts.AsQueryable());

            // when
            TerminologyArtifact actualTerminologyArtifact =
                await this.terminologyArtifactProcessingService.GetNonDownloadedArtifactAsync();

            // then
            actualTerminologyArtifact.Should().BeEquivalentTo(expectedTerminologyArtifact);

            this.terminologyArtifactServiceMock.Verify(service =>
                service.RetrieveAllTerminologyArtifacts(),
                    Times.Once());

            this.terminologyArtifactServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
