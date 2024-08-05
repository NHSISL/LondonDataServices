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
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomUnmatchedAddresses(count: 2);
            List<ResolvedAddress> storageResolvedAddresses = randomResolvedAddresses.DeepClone();
            List<ResolvedAddress> processingResolvedAddresses = storageResolvedAddresses.DeepClone();
            List<ResolvedAddress> doneProcessingResolvedAddresses = processingResolvedAddresses.DeepClone();
            string ouputCsv = GetRandomString();
            byte[] inputData = Encoding.UTF8.GetBytes(ouputCsv);
            Stream inputStream = new MemoryStream(inputData);
            Stream expectedStream = inputStream;
            Stream actualStream = new MemoryStream();
            Guid batchReference = Guid.NewGuid();

            this.resolvedAddressProcessingServiceMock.SetupSequence(service =>
                service.RetrieveAllResolvedAddresses())
                    .Returns(storageResolvedAddresses.AsQueryable())
                    .Returns(storageResolvedAddresses.AsQueryable())
                    .Returns(new List<ResolvedAddress>().AsQueryable());

            processingResolvedAddresses.ForEach(pra =>
            {
                pra.IsProcessing = true;
                pra.RetryCount += 1;
                pra.BatchReference = batchReference;
            });

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(batchReference);

            this.resolvedAddressProcessingServiceMock.Setup(processing =>
                processing.BulkModifyResolvedAddressesAsync(processingResolvedAddresses))
                    .Returns(ValueTask.CompletedTask);

            this.csvHelperBrokerMock.Setup(broker =>
                broker.MapObjectToCsvAsync<ResolvedAddress>(
                    It.Is(SameResolvedAddressListAs(processingResolvedAddresses)),
                    false,
                    null,
                    true))
                        .ReturnsAsync(ouputCsv);

            string inputFileName = $"out/{batchReference}.csv";
            string inputContainer = blobContainers.Addresses;

            this.documentProcessingServiceMock
            .Setup(processing => processing.AddDocumentAsync(
                   It.Is(SameStreamAs(inputStream)),
                   inputFileName,
                   inputContainer))
               .Callback<Stream, string, string>((input, fileName, container) =>
               {
                   input.Position = 0;
                   input.CopyTo(actualStream);
               })
               .Returns(ValueTask.CompletedTask);

            doneProcessingResolvedAddresses.ForEach(dpra =>
            {
                dpra.BatchReference = batchReference;
                dpra.IsProcessing = false;
                dpra.RetryCount = 0;
                dpra.IsExported = true;
                dpra.IsProcessed = true;
            });

            this.resolvedAddressProcessingServiceMock.Setup(processing =>
                processing.BulkModifyResolvedAddressesAsync(doneProcessingResolvedAddresses))
                    .Returns(ValueTask.CompletedTask);

            // When
            List<Guid> actualBatchList = await this.resolvedAddressOrchestrationService.ExportResolvedAddressesAsync();

            // Then
            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifier(),
                    Times.Once());

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddresses(),
                    Times.Exactly(2));

            this.resolvedAddressProcessingServiceMock.Verify(processing =>
                processing.BulkModifyResolvedAddressesAsync(
                    It.Is(SameResolvedAddressListAs(processingResolvedAddresses))),
                        Times.Once);

            this.csvHelperBrokerMock.Verify(broker =>
                broker.MapObjectToCsvAsync<ResolvedAddress>(
                    It.Is(SameResolvedAddressListAs(processingResolvedAddresses)), false, null, true),
                        Times.Once);

            this.documentProcessingServiceMock.Verify(processing =>
                processing.AddDocumentAsync(It.IsAny<Stream>(), inputFileName, inputContainer),
                    Times.Once);

            this.resolvedAddressProcessingServiceMock.Verify(processing =>
                processing.BulkModifyResolvedAddressesAsync(
                    It.Is(SameResolvedAddressListAs(doneProcessingResolvedAddresses))),
                        Times.Once);

            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
