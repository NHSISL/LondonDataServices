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
        public async Task ShouldModifyTerminologyArtifactAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            TerminologyArtifact randomTerminologyArtifact =
                CreateRandomModifyTerminologyArtifact(randomDateTimeOffset, randomEntraUser.EntraUserId);

            TerminologyArtifact inputTerminologyArtifact = randomTerminologyArtifact;
            TerminologyArtifact storageTerminologyArtifact = inputTerminologyArtifact.DeepClone();
            storageTerminologyArtifact.UpdatedDate = randomTerminologyArtifact.CreatedDate;
            TerminologyArtifact updatedTerminologyArtifact = inputTerminologyArtifact;
            TerminologyArtifact expectedTerminologyArtifact = updatedTerminologyArtifact.DeepClone();
            Guid objectColumnId = inputTerminologyArtifact.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyArtifactByIdAsync(objectColumnId))
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
                    Times.Exactly(2));

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

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