// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
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
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            TerminologyArtifact randomTerminologyArtifact = 
                CreateRandomTerminologyArtifact(randomDateTimeOffset, randomEntraUser.EntraUserId);

            Guid inputTerminologyArtifactId = randomTerminologyArtifact.Id;
            TerminologyArtifact storageTerminologyArtifact = randomTerminologyArtifact;

            TerminologyArtifact terminologyArtifactWithDeleteAuditApplied = storageTerminologyArtifact.DeepClone();
            terminologyArtifactWithDeleteAuditApplied.UpdatedBy = randomEntraUser.EntraUserId.ToString();
            terminologyArtifactWithDeleteAuditApplied.UpdatedDate = randomDateTimeOffset;

            TerminologyArtifact updatedTerminologyArtifact = terminologyArtifactWithDeleteAuditApplied;
            TerminologyArtifact deletedTerminologyArtifact = updatedTerminologyArtifact;
            TerminologyArtifact expectedTerminologyArtifact = deletedTerminologyArtifact.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyArtifactByIdAsync(inputTerminologyArtifactId))
                    .ReturnsAsync(storageTerminologyArtifact);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateTerminologyArtifactAsync(randomTerminologyArtifact))
                    .ReturnsAsync(updatedTerminologyArtifact);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteTerminologyArtifactAsync(updatedTerminologyArtifact))
                    .ReturnsAsync(deletedTerminologyArtifact);

            TerminologyArtifact actualTerminologyArtifact = 
                await this.terminologyArtifactService.RemoveTerminologyArtifactByIdAsync(inputTerminologyArtifactId);

            actualTerminologyArtifact.Should().BeEquivalentTo(expectedTerminologyArtifact);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyArtifactByIdAsync(inputTerminologyArtifactId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTerminologyArtifactAsync(randomTerminologyArtifact),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTerminologyArtifactAsync(updatedTerminologyArtifact),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }
    }
}