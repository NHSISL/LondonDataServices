// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.PdsAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.PdsAudits
{
    public partial class PdsAuditServiceTests
    {
        [Fact]
        public async Task ShouldRemovePdsAuditByIdAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();
            PdsAudit randomPdsAudit = CreateRandomPdsAudit(randomDateTimeOffset, randomEntraUser.EntraUserId);
            Guid inputPdsAuditId = randomPdsAudit.Id;
            PdsAudit storagePdsAudit = randomPdsAudit;
            PdsAudit ingestionTrackingWithDeleteAuditApplied = storagePdsAudit.DeepClone();
            ingestionTrackingWithDeleteAuditApplied.UpdatedBy = randomEntraUser.EntraUserId.ToString();
            ingestionTrackingWithDeleteAuditApplied.UpdatedDate = randomDateTimeOffset;
            PdsAudit updatedPdsAudit = storagePdsAudit;
            PdsAudit deletedPdsAudit = updatedPdsAudit;
            PdsAudit expectedPdsAudit = deletedPdsAudit.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPdsAuditByIdAsync(inputPdsAuditId))
                    .ReturnsAsync(storagePdsAudit);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdatePdsAuditAsync(randomPdsAudit))
                    .ReturnsAsync(updatedPdsAudit);

            this.storageBrokerMock.Setup(broker =>
                broker.DeletePdsAuditAsync(updatedPdsAudit))
                    .ReturnsAsync(deletedPdsAudit);

            // when
            PdsAudit actualPdsAudit = await this.pdsAuditService
                .RemovePdsAuditByIdAsync(inputPdsAuditId);

            // then
            actualPdsAudit.Should().BeEquivalentTo(expectedPdsAudit);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(inputPdsAuditId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePdsAuditAsync(randomPdsAudit),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePdsAuditAsync(updatedPdsAudit),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}