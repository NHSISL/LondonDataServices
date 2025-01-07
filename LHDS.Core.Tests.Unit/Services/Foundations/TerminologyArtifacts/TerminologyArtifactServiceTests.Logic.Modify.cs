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
        public async Task ShouldModifyTerminologyArtifactAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            TerminologyArtifact randomTerminologyArtifact = CreateRandomModifyTerminologyArtifact(randomDateTimeOffset);
            TerminologyArtifact inputTerminologyArtifact = randomTerminologyArtifact;
            TerminologyArtifact storageTerminologyArtifact = inputTerminologyArtifact.DeepClone();
            storageTerminologyArtifact.UpdatedDate = randomTerminologyArtifact.CreatedDate;
            TerminologyArtifact updatedTerminologyArtifact = inputTerminologyArtifact;
            TerminologyArtifact expectedTerminologyArtifact = updatedTerminologyArtifact.DeepClone();
            Guid terminologyArtifactId = inputTerminologyArtifact.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyArtifactByIdAsync(terminologyArtifactId))
                    .ReturnsAsync(storageTerminologyArtifact);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateTerminologyArtifactAsync(inputTerminologyArtifact))
                    .ReturnsAsync(updatedTerminologyArtifact);

            // when
            TerminologyArtifact actualTerminologyArtifact =
                await this.terminologyArtifactService.ModifyTerminologyArtifactAsync(inputTerminologyArtifact);

            // then
            actualTerminologyArtifact.Should().BeEquivalentTo(expectedTerminologyArtifact);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyArtifactByIdAsync(inputTerminologyArtifact.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTerminologyArtifactAsync(inputTerminologyArtifact),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}