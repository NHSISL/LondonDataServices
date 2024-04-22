// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditProcessingServiceTests
    {
        [Fact]
        public async Task ShouldModifyIngestionTrackingAsync()
        {
            // Given
            IngestionTrackingAudit randomIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();
            IngestionTrackingAudit inputIngestionTrackingAudit = randomIngestionTrackingAudit;
            IngestionTrackingAudit storageIngestionTrackingAudit = inputIngestionTrackingAudit;
            IngestionTrackingAudit expectedIngestionTrackingAudit = storageIngestionTrackingAudit.DeepClone();

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.ModifyIngestionTrackingAuditAsync(inputIngestionTrackingAudit))
                    .ReturnsAsync(storageIngestionTrackingAudit);

            // When
            await this.ingestionTrackingAuditProcessingService
                .ModifyIngestionTrackingAuditAsync(inputIngestionTrackingAudit);

            // Then
            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.ModifyIngestionTrackingAuditAsync(inputIngestionTrackingAudit),
                    Times.Once);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
