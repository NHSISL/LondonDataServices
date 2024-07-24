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
        public async Task ShouldExportResolvedAddressNEWWAsync()
        {
            // Given
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomUnmatchedAddresses(count: 2);
            List<ResolvedAddress> storageResolvedAddresses = randomResolvedAddresses.DeepClone();
            List<ResolvedAddress> processingResolvedAddresses = storageResolvedAddresses.DeepClone();
            List<ResolvedAddressReturn> returnResolvedAddresses = MapToResolvedAddressReturn(processingResolvedAddresses);
            List<ResolvedAddress> doneProcessingResolvedAddresses = storageResolvedAddresses.DeepClone();
            string ouputCsv = GetRandomString();
            byte[] inputData = Encoding.UTF8.GetBytes(ouputCsv);
            Stream inputStream = new MemoryStream(inputData);
            Stream expectedStream = inputStream;
            Stream actualStream = new MemoryStream();
            Guid batchReference = Guid.NewGuid();
            string inputFileName = $"{batchReference}.csv";
            string inputContainer = blobContainers.Addresses;

            this.resolvedAddressProcessingServiceMock.SetupSequence(service =>
                service.RetrieveAllResolvedAddresses())
                    .Returns(storageResolvedAddresses.AsQueryable())
                    .Returns(storageResolvedAddresses.AsQueryable())
                    .Returns(new List<ResolvedAddress>().AsQueryable());

            processingResolvedAddresses.ForEach(pra =>
            {
                pra.IsProcessing = true;
                pra.RetryCount += 1;
            });

            this.resolvedAddressProcessingServiceMock.Setup(processing =>
                processing.BulkModifyResolvedAddressesAsync(processingResolvedAddresses))
                    .Returns(ValueTask.CompletedTask);

            this.csvHelperBrokerMock.Setup(service =>
                service.MapObjectToCsvAsync(
                    It.Is(SameResolvedAddressReturnsAs(returnResolvedAddresses)), false, null, true))
                        .ReturnsAsync(ouputCsv);

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
            List<Guid> actualBatchList =
                await this.resolvedAddressOrchestrationService.ExportResolvedAddressesAsync();

            // Then
            this.resolvedAddressProcessingServiceMock.Verify(service =>
               service.RetrieveAllResolvedAddresses(),
                   Times.Once);

            this.csvHelperBrokerMock.Verify(service =>
                service.MapObjectToCsvAsync(
                    It.Is(SameResolvedAddressReturnsAs(returnResolvedAddresses)), false, null, true),
                        Times.Once);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.BulkModifyResolvedAddressesAsync(processingResolvedAddresses),
                    Times.Once);

            this.csvHelperBrokerMock.Verify(service =>
                service.MapObjectToCsvAsync(
                    It.Is(SameResolvedAddressReturnsAs(returnResolvedAddresses)), false, null, true),
                        Times.Once);

            this.documentProcessingServiceMock.Verify(service =>
                service.AddDocumentAsync(It.IsAny<Stream>(), inputFileName, inputContainer),
                    Times.Once);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.BulkModifyResolvedAddressesAsync(doneProcessingResolvedAddresses),
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
