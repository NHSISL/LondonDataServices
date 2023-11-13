// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Linq;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.TerminologyArtifacts
{
    public partial class TerminologyArtifactProcessingServiceTests
    {
        [Fact]
        public void ShouldRetrieveTerminologyArtifactsByOntologyAsset()
        {
            // given
            IQueryable<TerminologyArtifact> randomTerminologyArtifacts = CreateRandomTerminologyArtifacts();
            IQueryable<TerminologyArtifact> outputTerminologyArtifacts = randomTerminologyArtifacts;
            IQueryable<TerminologyArtifact> expectedTerminologyArtifacts = outputTerminologyArtifacts.DeepClone();

            this.terminologyArtifactServiceMock.Setup(service =>
                service.RetrieveAllTerminologyArtifacts())
                    .Returns(outputTerminologyArtifacts);

            // when
            IQueryable<TerminologyArtifact> actualTerminologyArtifacts =
                this.terminologyArtifactProcessingService.RetrieveAllTerminologyArtifactsAsync();

            // then
            actualTerminologyArtifacts.Should().BeEquivalentTo(expectedTerminologyArtifacts);

            this.terminologyArtifactServiceMock.Verify(service =>
                service.RetrieveAllTerminologyArtifacts(),
                    Times.Once());

            this.terminologyArtifactServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
