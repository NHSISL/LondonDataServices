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
        public async Task ShouldRemoveObjectColumnByIdAsync()
        {
            // Given
            TerminologyArtifact randomTerminologyArtifacts = CreateRandomTerminologyArtifact();
            TerminologyArtifact storageTerminologyArtifacts = randomTerminologyArtifacts;
            TerminologyArtifact expectedTerminologyArtifact = storageTerminologyArtifacts.DeepClone();

            this.terminologyArtifactServiceMock.Setup(service =>
                service.RemoveTerminologyArtifactByIdAsync(randomTerminologyArtifacts.Id))
                    .ReturnsAsync(storageTerminologyArtifacts);

            // When
            TerminologyArtifact actualTerminologyArtifact = await this.terminologyArtifactProcessingService
                .RemoveTerminologyArtifactByIdAsync(randomTerminologyArtifacts.Id);

            // Then
            actualTerminologyArtifact.Should().BeEquivalentTo(expectedTerminologyArtifact);

            this.terminologyArtifactServiceMock.Verify(service =>
                service.RemoveTerminologyArtifactByIdAsync(randomTerminologyArtifacts.Id),
                    Times.Once);

            this.terminologyArtifactServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
