// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Audits;
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
            Audit randomIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();
            Audit inputIngestionTrackingAudit = randomIngestionTrackingAudit;
            Audit storageIngestionTrackingAudit = inputIngestionTrackingAudit;
            Audit expectedIngestionTrackingAudit = storageIngestionTrackingAudit.DeepClone();

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.ModifyAuditAsync(inputIngestionTrackingAudit))
                    .ReturnsAsync(storageIngestionTrackingAudit);

            // When
            await this.ingestionTrackingAuditProcessingService
                .ModifyIngestionTrackingAuditAsync(inputIngestionTrackingAudit);

            // Then
            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.ModifyAuditAsync(inputIngestionTrackingAudit),
                    Times.Once);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
