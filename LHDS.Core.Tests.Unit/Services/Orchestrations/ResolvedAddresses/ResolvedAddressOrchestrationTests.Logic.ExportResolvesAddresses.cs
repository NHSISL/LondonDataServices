// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.ResolvedAddresses
{
    public partial class ResolvedAddressOrchestrationTests
    {
        [Fact]
        public async Task ShouldExportResolvedAddressAsync()
        {
            // Given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Guid identifier = Guid.NewGuid();

            List<ResolvedAddress> randomUnmatchedResolvedAddresses =
                CreateRandomUnmatchedAddresses(count: 2, dateTimeOffset: randomDateTimeOffset);

            List<ResolvedAddress> storageResolvedAddresses = randomUnmatchedResolvedAddresses.DeepClone();

            this.resolvedAddressProcessingServiceMock.Setup(service =>
                service.RetrieveAllResolvedAddressesAsync())
                    .ReturnsAsync(storageResolvedAddresses.AsQueryable());

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ReturnsAsync(identifier);

            string auditType = "Resolved Address Export";
            string exportingAuditTitle = "Exporting Resolved Addresses";
            string exportingAuditMessage = $"Exporting resolved addresses with correlation id {identifier}";
            string exportedAuditTitle = "Exported Resolved Addresses";
            string exportedAuditMessage = $"Exported resolved addresses with correlation id {identifier}";

            List<ResolvedAddress> processingResolvedAddresses = storageResolvedAddresses.DeepClone();

            processingResolvedAddresses.ForEach(resolvedAddress =>
            {
                resolvedAddress.IsProcessing = true;
                resolvedAddress.RetryCount += 1;
                resolvedAddress.BatchReference = identifier;
            });

            this.resolvedAddressProcessingServiceMock.Setup(service =>
                service.BulkModifyResolvedAddressesAsync(processingResolvedAddresses))
                    .Returns(ValueTask.CompletedTask);

            Dictionary<string, int> fieldMappings = new Dictionary<string, int>
            {
                { nameof(ResolvedAddress.UniqueReference), 0 },
                { nameof(ResolvedAddress.UPRN), 1 },
                { nameof(ResolvedAddress.USRN), 2 },
                { nameof(ResolvedAddress.OrganisationName), 3 },
                { nameof(ResolvedAddress.DepartmentName), 4 },
                { nameof(ResolvedAddress.SubBuildingName), 5 },
                { nameof(ResolvedAddress.BuildingName), 6 },
                { nameof(ResolvedAddress.BuildingNumber), 7 },
                { nameof(ResolvedAddress.DependentThoroughfare), 8 },
                { nameof(ResolvedAddress.Thoroughfare), 9 },
                { nameof(ResolvedAddress.DoubleDependentLocality), 10 },
                { nameof(ResolvedAddress.DependentLocality), 11 },
                { nameof(ResolvedAddress.PostTown), 12 },
                { nameof(ResolvedAddress.PostCode), 13 },
                { nameof(ResolvedAddress.AddressFormatQuality), 14 },
                { nameof(ResolvedAddress.PostCodeQuality), 15 },
                { nameof(ResolvedAddress.MatchedWithAssign), 16 },
                { nameof(ResolvedAddress.Qualifier), 17 },
                { nameof(ResolvedAddress.Classification), 18 },
                { nameof(ResolvedAddress.Algorithm), 19 },
                { nameof(ResolvedAddress.MatchPattern), 20 },
                { nameof(ResolvedAddress.UnstructuredPostalAddress), 21 },
                { nameof(ResolvedAddress.XCoordinate), 22 },
                { nameof(ResolvedAddress.YCoordinate), 23 },
                { nameof(ResolvedAddress.Latitude), 24 },
                { nameof(ResolvedAddress.Longitude), 25 }
            };

            string ouputCsv = GetRandomString();

            this.csvHelperBrokerMock.Setup(broker =>
                broker.MapObjectToCsvAsync<ResolvedAddress>(
                    It.Is(SameResolvedAddressListAs(processingResolvedAddresses)),
                    true,
                    fieldMappings,
                    false))
                        .ReturnsAsync(ouputCsv);

            byte[] inputData = Encoding.UTF8.GetBytes(ouputCsv);
            Stream inputStream = new MemoryStream(inputData);
            Stream expectedStream = inputStream;
            Stream actualStream = new MemoryStream();
            string inputFileName = $"out/{identifier}.csv";
            string inputContainer = blobContainers.Addresses;

            this.documentProcessingServiceMock
                .Setup(service => service.AddDocumentAsync(
                       It.Is(SameStreamAs(inputStream)),
                       inputFileName,
                       inputContainer))
                   .Callback<Stream, string, string>((input, fileName, container) =>
                   {
                       input.Position = 0;
                       input.CopyTo(actualStream);
                   })
                   .Returns(ValueTask.CompletedTask);

            List<ResolvedAddress> processedResolvedAddresses = processingResolvedAddresses.DeepClone();

            processedResolvedAddresses.ForEach(resolvedAddress =>
            {
                resolvedAddress.BatchReference = identifier;
                resolvedAddress.IsProcessing = false;
                resolvedAddress.RetryCount = 0;
                resolvedAddress.IsExported = true;
                resolvedAddress.IsProcessed = true;
            });

            this.resolvedAddressProcessingServiceMock.Setup(service =>
                service.BulkModifyResolvedAddressesAsync(processedResolvedAddresses))
                    .Returns(ValueTask.CompletedTask);


            // When
            List<Guid> actualBatchList = await this.resolvedAddressOrchestrationService.ExportResolvedAddressesAsync();

            // Then
            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddressesAsync(),
                    Times.Once);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Once);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddressesAsync(),
                    Times.Exactly(2));

            this.auditBrokerMock.Verify(service =>
                service.LogAsync(
                    auditType,
                    exportingAuditTitle,
                    exportingAuditMessage,
                    $"{identifier}.csv",
                    identifier.ToString(),
                    "Information"),
                        Times.Once());

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.BulkModifyResolvedAddressesAsync(
                    It.Is(SameResolvedAddressListAs(processingResolvedAddresses))),
                        Times.Once);

            this.csvHelperBrokerMock.Verify(broker =>
                broker.MapObjectToCsvAsync<ResolvedAddress>(
                    It.Is(SameResolvedAddressListAs(processingResolvedAddresses)),
                    true,
                    fieldMappings,
                    false),
                        Times.Once);

            this.documentProcessingServiceMock.Verify(processing =>
                processing.AddDocumentAsync(It.IsAny<Stream>(), inputFileName, inputContainer),
                    Times.Once);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.BulkModifyResolvedAddressesAsync(
                    It.Is(SameResolvedAddressListAs(processedResolvedAddresses))),
                        Times.Once);

            this.auditBrokerMock.Verify(service =>
                service.LogAsync(
                    auditType,
                    exportedAuditTitle,
                    exportedAuditMessage,
                    $"{identifier}.csv",
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
