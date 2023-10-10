// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
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
        public async Task ShouldModifyIngestionTrackingIfOneExistsAndNotAddAsync()
        {
            // Given
            Audit randomIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();
            Audit storageIngestionTrackingAudit = randomIngestionTrackingAudit;
            Audit modifiedIngestionTrackingAudit = storageIngestionTrackingAudit.DeepClone();
            modifiedIngestionTrackingAudit.UpdatedDate = DateTimeOffset.UtcNow;
            Audit updatedIngestionTrackingAudit = modifiedIngestionTrackingAudit.DeepClone();
            Audit expectedIngestionTrackingAudit = updatedIngestionTrackingAudit;

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.RetrieveAuditByIdAsync(modifiedIngestionTrackingAudit.Id))
                    .ReturnsAsync(value: storageIngestionTrackingAudit);

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.ModifyAuditAsync(modifiedIngestionTrackingAudit))
                    .ReturnsAsync(value: updatedIngestionTrackingAudit);

            // When
            Audit actualIngestionTracking = await this.ingestionTrackingAuditProcessingService
                .ModifyOrAddIngestionTrackingAuditAsync(modifiedIngestionTrackingAudit);

            // Then
            actualIngestionTracking.Should().BeEquivalentTo(expectedIngestionTrackingAudit);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.RetrieveAuditByIdAsync(modifiedIngestionTrackingAudit.Id),
                    Times.Once);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.ModifyAuditAsync(modifiedIngestionTrackingAudit),
                    Times.Once);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.AddAuditAsync(modifiedIngestionTrackingAudit),
                    Times.Never);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldAddIngestionTrackingIfIngestionTrackingDoesNotExistsAsync()
        {
            // Given
            Audit randomIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();
            Audit inputIngestionTrackingAudit = randomIngestionTrackingAudit;
            Audit storageIngestionTrackingAudit = inputIngestionTrackingAudit.DeepClone();
            Audit expectedIngestionTrackingAudit = storageIngestionTrackingAudit;

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.RetrieveAuditByIdAsync(inputIngestionTrackingAudit.Id))
                    .ReturnsAsync(value: null);

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.AddAuditAsync(inputIngestionTrackingAudit))
                    .ReturnsAsync(value: storageIngestionTrackingAudit);

            // When
            await this.ingestionTrackingAuditProcessingService
                .ModifyOrAddIngestionTrackingAuditAsync(inputIngestionTrackingAudit);

            // Then
            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.RetrieveAuditByIdAsync(inputIngestionTrackingAudit.Id),
                    Times.Once);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
            service.AddAuditAsync(inputIngestionTrackingAudit),
            Times.Once);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.ModifyAuditAsync(inputIngestionTrackingAudit),
                    Times.Never);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
