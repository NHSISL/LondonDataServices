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

namespace LHDS.Core.Tests.Unit.Services.Processings.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditProcessingServiceTests
    {
        [Fact]
        public async Task ShouldModifyIngestionTrackingIfOneExistsAndNotAddAsync()
        {
            // Given
            IngestionTrackingAudit randomIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();
            IngestionTrackingAudit storageIngestionTrackingAudit = randomIngestionTrackingAudit;
            IngestionTrackingAudit modifiedIngestionTrackingAudit = storageIngestionTrackingAudit.DeepClone();
            modifiedIngestionTrackingAudit.UpdatedDate = DateTimeOffset.UtcNow;
            IngestionTrackingAudit updatedIngestionTrackingAudit = modifiedIngestionTrackingAudit.DeepClone();
            IngestionTrackingAudit expectedIngestionTrackingAudit = updatedIngestionTrackingAudit;

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingAuditByIdAsync(modifiedIngestionTrackingAudit.Id))
                    .ReturnsAsync(value: storageIngestionTrackingAudit);

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.ModifyIngestionTrackingAuditAsync(modifiedIngestionTrackingAudit))
                    .ReturnsAsync(value: updatedIngestionTrackingAudit);

            // When
            IngestionTrackingAudit actualIngestionTracking = await this.ingestionTrackingAuditProcessingService
                .ModifyOrAddIngestionTrackingAuditAsync(modifiedIngestionTrackingAudit);

            // Then
            actualIngestionTracking.Should().BeEquivalentTo(expectedIngestionTrackingAudit);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingAuditByIdAsync(modifiedIngestionTrackingAudit.Id),
                    Times.Once);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.ModifyIngestionTrackingAuditAsync(modifiedIngestionTrackingAudit),
                    Times.Once);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.AddIngestionTrackingAuditAsync(modifiedIngestionTrackingAudit),
                    Times.Never);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldAddIngestionTrackingIfIngestionTrackingDoesNotExistsAsync()
        {
            // Given
            IngestionTrackingAudit randomIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();
            IngestionTrackingAudit inputIngestionTrackingAudit = randomIngestionTrackingAudit;
            IngestionTrackingAudit storageIngestionTrackingAudit = inputIngestionTrackingAudit.DeepClone();
            IngestionTrackingAudit expectedIngestionTrackingAudit = storageIngestionTrackingAudit;

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingAuditByIdAsync(inputIngestionTrackingAudit.Id))
                    .ReturnsAsync(value: null);

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.AddIngestionTrackingAuditAsync(inputIngestionTrackingAudit))
                    .ReturnsAsync(value: storageIngestionTrackingAudit);

            // When
            await this.ingestionTrackingAuditProcessingService
                .ModifyOrAddIngestionTrackingAuditAsync(inputIngestionTrackingAudit);

            // Then
            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingAuditByIdAsync(inputIngestionTrackingAudit.Id),
                    Times.Once);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
            service.AddIngestionTrackingAuditAsync(inputIngestionTrackingAudit),
            Times.Once);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.ModifyIngestionTrackingAuditAsync(inputIngestionTrackingAudit),
                    Times.Never);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
