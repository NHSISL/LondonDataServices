// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
        public async Task ShouldRetrieveAuditByIdAsync()
        {
            // given
            IngestionTrackingAudit randomIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();
            IngestionTrackingAudit inputIngestionTrackingAudit = randomIngestionTrackingAudit;
            IngestionTrackingAudit storageIngestionTrackingAudit = randomIngestionTrackingAudit;
            IngestionTrackingAudit expectedIngestionTrackingAudit = storageIngestionTrackingAudit.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(inputIngestionTrackingAudit.Id))
                    .ReturnsAsync(storageIngestionTrackingAudit);

            // when
            IngestionTrackingAudit actualIngestionTrackingAudit =
                await this.ingestionTrackingAuditService.RetrieveIngestionTrackingAuditByIdAsync(
                    inputIngestionTrackingAudit.Id);

            // then
            actualIngestionTrackingAudit.Should().BeEquivalentTo(expectedIngestionTrackingAudit);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(inputIngestionTrackingAudit.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}