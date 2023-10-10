// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Audits;
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
            Audit randomIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();
            Audit storageIngestionTrackingAudit = randomIngestionTrackingAudit;
            Audit expectedIngestionTrackingAudit = storageIngestionTrackingAudit.DeepClone();

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.RetrieveAuditByIdAsync(randomIngestionTrackingAudit.Id))
                    .ReturnsAsync(storageIngestionTrackingAudit);

            // When
            Audit actualIngestionTrackingAudit =
                await this.ingestionTrackingAuditProcessingService
                    .RetrieveIngestionTrackingAuditByIdAsync(randomIngestionTrackingAudit.Id);

            // Then
            actualIngestionTrackingAudit.Should().BeEquivalentTo(expectedIngestionTrackingAudit);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.RetrieveAuditByIdAsync(randomIngestionTrackingAudit.Id),
                    Times.Once);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
