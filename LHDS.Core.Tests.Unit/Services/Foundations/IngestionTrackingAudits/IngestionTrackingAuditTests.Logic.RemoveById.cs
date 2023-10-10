// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Audits
{
    public partial class IngestionTrackingAuditTests
    {
        [Fact]
        public async Task ShouldRemoveAuditByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputAuditId = randomId;
            IngestionTrackingAudit randomIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();
            IngestionTrackingAudit storageIngestionTrackingAudit = randomIngestionTrackingAudit;
            IngestionTrackingAudit expectedInputIngestionTrackingAudit = storageIngestionTrackingAudit;
            IngestionTrackingAudit deletedIngestionTrackingAudit = expectedInputIngestionTrackingAudit;
            IngestionTrackingAudit expectedIngestionTrackingAudit = deletedIngestionTrackingAudit.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(inputAuditId))
                    .ReturnsAsync(storageIngestionTrackingAudit);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteIngestionTrackingAuditAsync(expectedInputIngestionTrackingAudit))
                    .ReturnsAsync(deletedIngestionTrackingAudit);

            // when
            IngestionTrackingAudit actualIngestionTrackingAudit = await this.ingestionTrackingAuditService
                .RemoveIngestionTrackingAuditByIdAsync(inputAuditId);

            // then
            actualIngestionTrackingAudit.Should().BeEquivalentTo(expectedIngestionTrackingAudit);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(inputAuditId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(storageIngestionTrackingAudit.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteIngestionTrackingAuditAsync(expectedInputIngestionTrackingAudit),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}