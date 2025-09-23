// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Brokers.Storages.StorageQueues;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.ResolvedAddresses
{
    public partial class ResolvedAddressOrchestrationTests
    {
        [Fact]
        public async Task ShouldUploadResolvedAddressAsync()
        {
            // Given
            Guid identifier = Guid.NewGuid();
            DateTimeOffset randomDate = GetRandomDateTimeOffset();
            EntraUser entraUser = CreateRandomEntraUser();
            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;
            string inputContent = GetRandomString();
            string uploadingAuditMessage = $"Uploading addresses to resolve with correlation id {identifier}";
            string uploadedAuditMessage = $"Uploaded addresses to resolve with correlation id {identifier}";
            string uploadingAuditTitle = "Uploading Resolved Addresses";
            string uploadedAuditTitle = "Uploaded Resolved Addresses";
            string auditType = "Resolved Address Upload";
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputContent);
            Stream inputStream = new MemoryStream(inputBytes);
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomResolvedAddresses(count: 1);
            List<ResolvedAddress> mappedResolvedAddresses = randomResolvedAddresses;

            Dictionary<string, int> fieldMappings =
                new Dictionary<string, int>
                {
                    { nameof(ResolvedAddress.UniqueReference), 0 },
                    { nameof(ResolvedAddress.UnstructuredPostalAddress), 2 }
                };

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDate);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ReturnsAsync(identifier);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(entraUser);

            this.csvHelperBrokerMock.Setup(service =>
                service.MapCsvToObjectAsync<ResolvedAddress>(inputContent, true, fieldMappings, true))
                    .ReturnsAsync(mappedResolvedAddresses);

            this.resolvedAddressProcessingServiceMock.Setup(service =>
                service.BulkAddResolvedAddressesAsync(mappedResolvedAddresses, It.IsAny<string>()))
                    .Returns(ValueTask.CompletedTask);

            // When
            await this.resolvedAddressOrchestrationService
                .UploadAddressesToResolveAsync(input: inputStream, fileName: inputFileName);

            // Then
            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            foreach (ResolvedAddress resolvedAddress in mappedResolvedAddresses)
            {
                Payload<Guid> payload = new Payload<Guid>
                {
                    Message = resolvedAddress.Id,
                    User = null,
                    EnqueuedAtUtc = randomDate
                };

                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = null,
                    WriteIndented = false,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };

                string payloadMessage = JsonSerializer.Serialize(payload, jsonOptions);

                this.storageQueueBrokerMock.Verify(broker =>
                    broker.SendMessageAsync(storageQueues.ResolvedAddressQueue, payloadMessage),
                        Times.Once);
            }

            this.auditBrokerMock.Verify(service =>
                service.LogAsync(
                    auditType,
                    uploadingAuditTitle,
                    uploadingAuditMessage,
                    null,
                    identifier.ToString(),
                    "Information"),
                        Times.Once());

            this.csvHelperBrokerMock.Verify(service =>
                service.MapCsvToObjectAsync<ResolvedAddress>(inputContent, true, fieldMappings, true),
                    Times.Once);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.BulkAddResolvedAddressesAsync(mappedResolvedAddresses, It.IsAny<string>()),
                    Times.Once);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(entraUser);

            this.auditBrokerMock.Verify(service =>
                service.LogAsync(
                    auditType,
                    uploadedAuditTitle,
                    uploadedAuditMessage,
                    null,
                    identifier.ToString(),
                    "Information"),
                        Times.Once());

            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }
    }
}
