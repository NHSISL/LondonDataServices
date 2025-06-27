// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddressesAudits;
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
            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;
            string inputContent = GetRandomString();
            string uploadingAuditMessage = $"Uploading addresses to resolve with correlation id {identifier}";
            string uploadedAuditMessage = $"Uploaded addresses to resolve with correlation id {identifier}";
            string auditType = "Upload";
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputContent);
            Stream inputStream = new MemoryStream(inputBytes);
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomResolvedAddresses();
            List<ResolvedAddress> mappedResolvedAddresses = randomResolvedAddresses;

            Dictionary<string, int> fieldMappings =
                new Dictionary<string, int>
                {
                    { nameof(ResolvedAddress.UniqueReference), 0 },
                    { nameof(ResolvedAddress.UnstructuredPostalAddress), 2 }
                };

            ResolvedAddressAudit randomUploadingResolvedAddressAudit =
                GetRandomResolvedAddressAudit(identifier, identifier, randomDate, uploadingAuditMessage, auditType);

            ResolvedAddressAudit randomUploadedResolvedAddressAudit =
                GetRandomResolvedAddressAudit(identifier, identifier, randomDate, uploadedAuditMessage, auditType);

            ResolvedAddressAudit inputUploadingResolvedAddressAudit = randomUploadingResolvedAddressAudit;
            ResolvedAddressAudit outputUploadingResolvedAddressAudit = inputUploadingResolvedAddressAudit.DeepClone();
            ResolvedAddressAudit inputUploadedResolvedAddressAudit = randomUploadedResolvedAddressAudit;
            ResolvedAddressAudit outputUploadedResolvedAddressAudit = inputUploadedResolvedAddressAudit.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDate);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ReturnsAsync(identifier);

            this.storageBrokerMock.Setup(service =>
                service.InsertResolvedAddressAuditAsync(inputUploadingResolvedAddressAudit))
                    .ReturnsAsync(outputUploadingResolvedAddressAudit);

            this.csvHelperBrokerMock.Setup(service =>
                service.MapCsvToObjectAsync<ResolvedAddress>(inputContent, true, fieldMappings, true))
                    .ReturnsAsync(mappedResolvedAddresses);

            this.resolvedAddressProcessingServiceMock.Setup(service =>
                service.BulkAddResolvedAddressesAsync(mappedResolvedAddresses, It.IsAny<string>()))
                    .Returns(ValueTask.CompletedTask);

            this.storageBrokerMock.Setup(service =>
                service.InsertResolvedAddressAuditAsync(inputUploadedResolvedAddressAudit))
                    .ReturnsAsync(outputUploadedResolvedAddressAudit);

            // When
            await this.resolvedAddressOrchestrationService
                .UploadAddressesToResolveAsync(input: inputStream, fileName: inputFileName);

            // Then
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(2));

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Exactly(3));

            this.storageBrokerMock.Verify(service =>
                service.InsertResolvedAddressAuditAsync(inputUploadingResolvedAddressAudit),
                    Times.Once());

            this.csvHelperBrokerMock.Verify(service =>
                service.MapCsvToObjectAsync<ResolvedAddress>(inputContent, true, fieldMappings, true),
                    Times.Once);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.BulkAddResolvedAddressesAsync(mappedResolvedAddresses, It.IsAny<string>()),
                    Times.Once);

            this.storageBrokerMock.Verify(service =>
                service.InsertResolvedAddressAuditAsync(inputUploadedResolvedAddressAudit),
                    Times.Once());

            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
