// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
        public async Task ShouldRetrieveIngestionTrackingIfOneExistsAndNotAddAsync()
        {
            // Given
            IngestionTrackingAudit randomIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();
            IngestionTrackingAudit inputIngestionTrackingAudit = randomIngestionTrackingAudit;
            IngestionTrackingAudit storageIngestionTrackingAudit = inputIngestionTrackingAudit;
            IngestionTrackingAudit expectedIngestionTrackingAudit = storageIngestionTrackingAudit.DeepClone();

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingAuditByIdAsync(inputIngestionTrackingAudit.Id))
                    .ReturnsAsync(value: storageIngestionTrackingAudit);

            // When
            IngestionTrackingAudit actualIngestionTrackingAudit = await this.ingestionTrackingAuditProcessingService
                .RetrieveOrAddIngestionTrackingAuditAsync(inputIngestionTrackingAudit);

            // Then
            actualIngestionTrackingAudit.Should().BeEquivalentTo(expectedIngestionTrackingAudit);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingAuditByIdAsync(inputIngestionTrackingAudit.Id),
                    Times.Once);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.AddIngestionTrackingAuditAsync(inputIngestionTrackingAudit),
                    Times.Never);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldAddIngestionTrackingIfOneDoesNotExistAsync()
        {
            // Given
            IngestionTrackingAudit randomIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();
            IngestionTrackingAudit inputIngestionTrackingAudit = randomIngestionTrackingAudit;
            IngestionTrackingAudit storageIngestionTrackingAudit = inputIngestionTrackingAudit;
            IngestionTrackingAudit expectedIngestionTrackingAudit = storageIngestionTrackingAudit.DeepClone();

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingAuditByIdAsync(inputIngestionTrackingAudit.Id))
                    .ReturnsAsync(value: null);

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.AddIngestionTrackingAuditAsync(inputIngestionTrackingAudit))
                    .ReturnsAsync(storageIngestionTrackingAudit);

            // When
            await this.ingestionTrackingAuditProcessingService.RetrieveOrAddIngestionTrackingAuditAsync(inputIngestionTrackingAudit);

            // Then
            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingAuditByIdAsync(inputIngestionTrackingAudit.Id),
                    Times.Once);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.AddIngestionTrackingAuditAsync(inputIngestionTrackingAudit),
                    Times.Once);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
