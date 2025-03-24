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
        public async Task ShouldAddTerminologyArtifactAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            TerminologyArtifact randomTerminologyArtifact =
                CreateRandomTerminologyArtifact(randomDateTimeOffset, randomEntraUser.EntraUserId);

            TerminologyArtifact inputTerminologyArtifact = randomTerminologyArtifact;
            TerminologyArtifact storageTerminologyArtifact = inputTerminologyArtifact;
            TerminologyArtifact expectedTerminologyArtifact = storageTerminologyArtifact.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertTerminologyArtifactAsync(inputTerminologyArtifact))
                    .ReturnsAsync(storageTerminologyArtifact);

            // when
            TerminologyArtifact actualTerminologyArtifact = await this.terminologyArtifactService
                .AddTerminologyArtifactAsync(inputTerminologyArtifact);

            // then
            actualTerminologyArtifact.Should().BeEquivalentTo(expectedTerminologyArtifact);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(2));

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTerminologyArtifactAsync(inputTerminologyArtifact),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}