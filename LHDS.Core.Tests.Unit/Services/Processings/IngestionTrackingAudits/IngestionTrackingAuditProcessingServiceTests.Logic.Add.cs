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
        public async Task ShouldAddIngestionTrackingAuditAsync()
        {
            // Given
            Audit randomIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();
            Audit inputIngestionTrackingAudit = randomIngestionTrackingAudit;
            Audit storageIngestionTrackingAudit = inputIngestionTrackingAudit;
            Audit expectedIngestionTrackingAudit = storageIngestionTrackingAudit.DeepClone();

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.AddAuditAsync(inputIngestionTrackingAudit))
                    .ReturnsAsync(storageIngestionTrackingAudit);

            // When
            Audit actualIngestionTrackingAudit = await this.ingestionTrackingAuditProcessingService
                .AddIngestionTrackingAuditAsync(inputIngestionTrackingAudit);

            // Then
            actualIngestionTrackingAudit.Should().BeEquivalentTo(expectedIngestionTrackingAudit);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.AddAuditAsync(inputIngestionTrackingAudit),
                    Times.Once);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
