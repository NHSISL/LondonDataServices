// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task ShouldRetrieveTerminologyArtifactsByOntologyAssetAsync()
        {
            // given
            List<TerminologyArtifact> randomTerminologyArtifacts = CreateRandomTerminologyArtifacts();
            IQueryable<TerminologyArtifact> outputTerminologyArtifacts = randomTerminologyArtifacts.AsQueryable();
            IQueryable<TerminologyArtifact> expectedTerminologyArtifacts = outputTerminologyArtifacts.DeepClone();

            this.terminologyArtifactServiceMock.Setup(service =>
                service.RetrieveAllTerminologyArtifactsAsync())
                    .ReturnsAsync(outputTerminologyArtifacts);

            // when
            IQueryable<TerminologyArtifact> actualTerminologyArtifacts =
                await this.terminologyArtifactProcessingService.RetrieveAllTerminologyArtifactsAsync();

            // then
            actualTerminologyArtifacts.Should().BeEquivalentTo(expectedTerminologyArtifacts);

            this.terminologyArtifactServiceMock.Verify(service =>
                service.RetrieveAllTerminologyArtifactsAsync(),
                    Times.Once());

            this.terminologyArtifactServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
