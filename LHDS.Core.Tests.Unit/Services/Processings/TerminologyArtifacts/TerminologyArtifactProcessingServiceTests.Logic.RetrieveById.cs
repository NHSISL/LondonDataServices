// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        public async Task ShouldRetrieveTerminologyArtifactsByIdAsync()
        {
            // given
            TerminologyArtifact randomTerminologyArtifacts = CreateRandomTerminologyArtifact();
            TerminologyArtifact storageTerminologyArtifacts = randomTerminologyArtifacts;
            TerminologyArtifact expectedTerminologyArtifacts = storageTerminologyArtifacts.DeepClone();

            this.terminologyArtifactServiceMock.Setup(service =>
                service.RetrieveTerminologyArtifactByIdAsync(randomTerminologyArtifacts.Id))
                    .ReturnsAsync(storageTerminologyArtifacts);

            // when
            TerminologyArtifact actualTerminologyArtifacts =
                await this.terminologyArtifactProcessingService
                    .RetrieveTerminologyArtifactByIdAsync(randomTerminologyArtifacts.Id);

            // then
            actualTerminologyArtifacts.Should().BeEquivalentTo(expectedTerminologyArtifacts);

            this.terminologyArtifactServiceMock.Verify(service =>
                service.RetrieveTerminologyArtifactByIdAsync(randomTerminologyArtifacts.Id),
                    Times.Once());

            this.terminologyArtifactServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
