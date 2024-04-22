// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.TerminologyArtifacts
{
    public partial class TerminologyArtifactServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveTerminologyArtifactByIdAsync()
        {
            // given
            TerminologyArtifact randomTerminologyArtifact = CreateRandomTerminologyArtifact();
            TerminologyArtifact inputTerminologyArtifact = randomTerminologyArtifact;
            TerminologyArtifact storageTerminologyArtifact = randomTerminologyArtifact;
            TerminologyArtifact expectedTerminologyArtifact = storageTerminologyArtifact.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyArtifactByIdAsync(inputTerminologyArtifact.Id))
                    .ReturnsAsync(storageTerminologyArtifact);

            // when
            TerminologyArtifact actualTerminologyArtifact =
                await this.terminologyArtifactService.RetrieveTerminologyArtifactByIdAsync(inputTerminologyArtifact.Id);

            // then
            actualTerminologyArtifact.Should().BeEquivalentTo(expectedTerminologyArtifact);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyArtifactByIdAsync(inputTerminologyArtifact.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}