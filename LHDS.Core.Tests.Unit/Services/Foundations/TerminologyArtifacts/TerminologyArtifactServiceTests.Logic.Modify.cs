// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Services.Foundations.TerminologyArtifacts;
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
            string randomUserId = GetRandomStringWithLengthOf(50);

            TerminologyArtifact randomTerminologyArtifact =
                CreateRandomModifyTerminologyArtifact(randomDateTimeOffset, randomUserId);

            TerminologyArtifact inputTerminologyArtifact = randomTerminologyArtifact.DeepClone();
            TerminologyArtifact storageTerminologyArtifact = randomTerminologyArtifact.DeepClone();
            storageTerminologyArtifact.UpdatedDate = storageTerminologyArtifact.CreatedDate;
            TerminologyArtifact updatedTerminologyArtifact = inputTerminologyArtifact.DeepClone();
            updatedTerminologyArtifact.CreatedBy = storageTerminologyArtifact.CreatedBy;
            updatedTerminologyArtifact.CreatedDate = storageTerminologyArtifact.CreatedDate;
            TerminologyArtifact expectedTerminologyArtifact = updatedTerminologyArtifact.DeepClone();
            Guid objectColumnId = inputTerminologyArtifact.Id;

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(inputTerminologyArtifact))
                    .ReturnsAsync(inputTerminologyArtifact);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyArtifactByIdAsync(objectColumnId))
                    .ReturnsAsync(storageTerminologyArtifact);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                    inputTerminologyArtifact, storageTerminologyArtifact))
                        .ReturnsAsync(inputTerminologyArtifact);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateTerminologyArtifactAsync(inputTerminologyArtifact))
                    .ReturnsAsync(updatedTerminologyArtifact);

            // when
            TerminologyArtifact actualTerminologyArtifact =
                await this.terminologyArtifactService.ModifyTerminologyArtifactAsync(inputTerminologyArtifact);

            // then
            actualTerminologyArtifact.Should().BeEquivalentTo(expectedTerminologyArtifact);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(inputTerminologyArtifact),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                    inputTerminologyArtifact, storageTerminologyArtifact),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyArtifactByIdAsync(inputTerminologyArtifact.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTerminologyArtifactAsync(inputTerminologyArtifact),
                    Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}