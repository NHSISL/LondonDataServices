// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditTests
    {
        [Fact]
        public async Task ShouldModifyAuditAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            IngestionTrackingAudit randomIngestionTrackingAudit = CreateRandomModifyIngestionTrackingAudit(randomDateTimeOffset);
            IngestionTrackingAudit inputIngestionTrackingAudit = randomIngestionTrackingAudit;
            IngestionTrackingAudit storageIngestionTrackingAudit = inputIngestionTrackingAudit.DeepClone();
            storageIngestionTrackingAudit.UpdatedDate = randomIngestionTrackingAudit.CreatedDate;
            IngestionTrackingAudit updatedIngestionTrackingAudit = inputIngestionTrackingAudit;
            IngestionTrackingAudit expectedIngestionTrackingAudit = updatedIngestionTrackingAudit.DeepClone();
            Guid auditId = inputIngestionTrackingAudit.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(auditId))
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
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(inputIngestionTrackingAudit.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateIngestionTrackingAuditAsync(inputIngestionTrackingAudit),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}