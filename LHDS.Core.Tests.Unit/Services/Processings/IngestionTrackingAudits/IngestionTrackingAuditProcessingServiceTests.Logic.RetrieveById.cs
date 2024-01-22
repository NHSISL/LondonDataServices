// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveIngestionTrackingByIdAsync()
        {
            // Given
            IngestionTrackingAudit randomIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();
            IngestionTrackingAudit storageIngestionTrackingAudit = randomIngestionTrackingAudit;
            IngestionTrackingAudit expectedIngestionTrackingAudit = storageIngestionTrackingAudit.DeepClone();

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingAuditByIdAsync(randomIngestionTrackingAudit.Id))
                    .ReturnsAsync(storageIngestionTrackingAudit);

            // When
            IngestionTrackingAudit actualIngestionTrackingAudit =
                await this.ingestionTrackingAuditProcessingService
                    .RetrieveIngestionTrackingAuditByIdAsync(randomIngestionTrackingAudit.Id);

            // Then
            actualIngestionTrackingAudit.Should().BeEquivalentTo(expectedIngestionTrackingAudit);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingAuditByIdAsync(randomIngestionTrackingAudit.Id),
                    Times.Once);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
