// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditTests
    {
        [Fact]
        public async Task ShouldModifyIngestionTrackingAuditAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser entraUser = CreateRandomEntraUser();

            IngestionTrackingAudit randomIngestionTrackingAudit =
                CreateRandomModifyIngestionTrackingAudit(randomDateTimeOffset, entraUser.EntraUserId);

            IngestionTrackingAudit inputIngestionTrackingAudit = randomIngestionTrackingAudit;
            IngestionTrackingAudit storageIngestionTrackingAudit = inputIngestionTrackingAudit.DeepClone();
            storageIngestionTrackingAudit.UpdatedDate = randomIngestionTrackingAudit.CreatedDate;
            IngestionTrackingAudit updatedIngestionTrackingAudit = inputIngestionTrackingAudit;
            IngestionTrackingAudit expectedIngestionTrackingAudit = updatedIngestionTrackingAudit.DeepClone();
            Guid ingestionTrackingAuditId = inputIngestionTrackingAudit.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(entraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(ingestionTrackingAuditId))
                    .ReturnsAsync(storageIngestionTrackingAudit);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateIngestionTrackingAuditAsync(inputIngestionTrackingAudit))
                    .ReturnsAsync(updatedIngestionTrackingAudit);

            // when
            IngestionTrackingAudit actualIngestionTrackingAudit =
                await this.ingestionTrackingAuditService.ModifyIngestionTrackingAuditAsync(inputIngestionTrackingAudit);

            // then
            actualIngestionTrackingAudit.Should().BeEquivalentTo(expectedIngestionTrackingAudit);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(2));

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(inputIngestionTrackingAudit.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateIngestionTrackingAuditAsync(inputIngestionTrackingAudit),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}