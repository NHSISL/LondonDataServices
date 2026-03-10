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
            string randomUserId = GetRandomStringWithLengthOf(50);

            TerminologyArtifact randomTerminologyArtifact =
                CreateRandomTerminologyArtifact(randomDateTimeOffset, randomUserId);

            TerminologyArtifact inputTerminologyArtifact = randomTerminologyArtifact;
            TerminologyArtifact storageTerminologyArtifact = inputTerminologyArtifact;
            TerminologyArtifact expectedTerminologyArtifact = storageTerminologyArtifact.DeepClone();

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(inputTerminologyArtifact))
                    .ReturnsAsync(inputTerminologyArtifact);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertTerminologyArtifactAsync(inputTerminologyArtifact))
                    .ReturnsAsync(storageTerminologyArtifact);

            // when
            TerminologyArtifact actualTerminologyArtifact = await this.terminologyArtifactService
                .AddTerminologyArtifactAsync(inputTerminologyArtifact);

            // then
            actualTerminologyArtifact.Should().BeEquivalentTo(expectedTerminologyArtifact);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(inputTerminologyArtifact),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTerminologyArtifactAsync(inputTerminologyArtifact),
                    Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}