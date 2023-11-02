using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.TerminologyArtifacts
{
    public partial class TerminologyArtifactServiceTests
    {
        [Fact]
        public async Task ShouldAddTerminologyArtifactAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            TerminologyArtifact randomTerminologyArtifact = CreateRandomTerminologyArtifact(randomDateTimeOffset);
            TerminologyArtifact inputTerminologyArtifact = randomTerminologyArtifact;
            TerminologyArtifact storageTerminologyArtifact = inputTerminologyArtifact;
            TerminologyArtifact expectedTerminologyArtifact = storageTerminologyArtifact.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertTerminologyArtifactAsync(inputTerminologyArtifact))
                    .ReturnsAsync(storageTerminologyArtifact);

            // when
            TerminologyArtifact actualTerminologyArtifact = await this.terminologyArtifactService
                .AddTerminologyArtifactAsync(inputTerminologyArtifact);

            // then
            actualTerminologyArtifact.Should().BeEquivalentTo(expectedTerminologyArtifact);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTerminologyArtifactAsync(inputTerminologyArtifact),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}