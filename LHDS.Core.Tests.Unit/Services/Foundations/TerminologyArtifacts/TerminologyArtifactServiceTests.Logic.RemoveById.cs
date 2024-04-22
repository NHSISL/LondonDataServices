// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public async Task ShouldRemoveTerminologyArtifactByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputTerminologyArtifactId = randomId;
            TerminologyArtifact randomTerminologyArtifact = CreateRandomTerminologyArtifact();
            TerminologyArtifact storageTerminologyArtifact = randomTerminologyArtifact;
            TerminologyArtifact expectedInputTerminologyArtifact = storageTerminologyArtifact;
            TerminologyArtifact deletedTerminologyArtifact = expectedInputTerminologyArtifact;
            TerminologyArtifact expectedTerminologyArtifact = deletedTerminologyArtifact.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyArtifactByIdAsync(inputTerminologyArtifactId))
                    .ReturnsAsync(storageTerminologyArtifact);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteTerminologyArtifactAsync(expectedInputTerminologyArtifact))
                    .ReturnsAsync(deletedTerminologyArtifact);

            // when
            TerminologyArtifact actualTerminologyArtifact = await this.terminologyArtifactService
                .RemoveTerminologyArtifactByIdAsync(inputTerminologyArtifactId);

            // then
            actualTerminologyArtifact.Should().BeEquivalentTo(expectedTerminologyArtifact);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyArtifactByIdAsync(inputTerminologyArtifactId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTerminologyArtifactAsync(expectedInputTerminologyArtifact),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}