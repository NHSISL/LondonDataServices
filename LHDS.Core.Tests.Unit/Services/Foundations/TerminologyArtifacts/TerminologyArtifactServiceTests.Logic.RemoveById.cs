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
            //Given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            TerminologyArtifact randomTerminologyArtifact = 
                CreateRandomTerminologyArtifact(randomDateTimeOffset, randomUserId);

            Guid inputTerminologyArtifactId = randomTerminologyArtifact.Id;
            TerminologyArtifact storageTerminologyArtifact = randomTerminologyArtifact;
            TerminologyArtifact terminologyArtifactWithDeleteAuditApplied = storageTerminologyArtifact.DeepClone();
            terminologyArtifactWithDeleteAuditApplied.UpdatedBy = randomUserId.ToString();
            terminologyArtifactWithDeleteAuditApplied.UpdatedDate = randomDateTimeOffset;
            TerminologyArtifact updatedTerminologyArtifact = terminologyArtifactWithDeleteAuditApplied;
            TerminologyArtifact deletedTerminologyArtifact = updatedTerminologyArtifact;
            TerminologyArtifact expectedTerminologyArtifact = deletedTerminologyArtifact.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyArtifactByIdAsync(inputTerminologyArtifactId))
                    .ReturnsAsync(storageTerminologyArtifact);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyRemoveAuditValuesAsync(storageTerminologyArtifact))
                    .ReturnsAsync(terminologyArtifactWithDeleteAuditApplied);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateTerminologyArtifactAsync(terminologyArtifactWithDeleteAuditApplied))
                    .ReturnsAsync(updatedTerminologyArtifact);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteTerminologyArtifactAsync(updatedTerminologyArtifact))
                    .ReturnsAsync(deletedTerminologyArtifact);

            //When
            TerminologyArtifact actualTerminologyArtifact = 
                await this.terminologyArtifactService.RemoveTerminologyArtifactByIdAsync(inputTerminologyArtifactId);

            //Then
            actualTerminologyArtifact.Should().BeEquivalentTo(expectedTerminologyArtifact);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyArtifactByIdAsync(inputTerminologyArtifactId),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyRemoveAuditValuesAsync(storageTerminologyArtifact),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTerminologyArtifactAsync(terminologyArtifactWithDeleteAuditApplied),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTerminologyArtifactAsync(updatedTerminologyArtifact),
                    Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}