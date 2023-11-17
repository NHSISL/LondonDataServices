// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
            IQueryable<TerminologyArtifact> randomTerminologyArtifacts = CreateRandomTerminologyArtifacts();
            IQueryable<TerminologyArtifact> outputTerminologyArtifacts = randomTerminologyArtifacts;
            TerminologyArtifact randomTerminologyArtifact = CreateRandomTerminologyArtifact();
            TerminologyArtifact downloadedTerminologyArtifact = randomTerminologyArtifact;
            downloadedTerminologyArtifact.IsDownloaded = false;
            outputTerminologyArtifacts.Append(downloadedTerminologyArtifact);

            this.terminologyArtifactServiceMock.Setup(service =>
                service.RetrieveAllTerminologyArtifacts())
                    .Returns(outputTerminologyArtifacts);

            TerminologyArtifact expectedTerminologyArtifact = downloadedTerminologyArtifact;

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
