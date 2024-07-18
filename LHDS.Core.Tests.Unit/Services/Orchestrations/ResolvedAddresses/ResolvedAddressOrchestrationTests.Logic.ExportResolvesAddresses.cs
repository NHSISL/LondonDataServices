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

            List<ResolvedAddress> randomResolvedAddresses = CreateRandomUnmatchedAddresses(count: 20000);
            List<ResolvedAddress> storageResolvedAddresses = randomResolvedAddresses.DeepClone();
            List<ResolvedAddress> updatedResolvedAddresses = randomResolvedAddresses.DeepClone();
            //Matched = true, IsProcessing = false, IsExported = false and retry count <= 3
            List<ResolvedAddressReturn> returnResolvedAddresses = MapToResolvedAddressReturn(storageResolvedAddresses);
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
                    .Returns(storageResolvedAddresses.AsQueryable());

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(batchReference);

            foreach (ResolvedAddress resolvedAddress in updatedResolvedAddresses)
            {
                resolvedAddress.IsProcessing = true;
                resolvedAddress.BatchReference = batchReference;
                resolvedAddress.RetryCount += 1;
                resolvedAddress.UpdatedDate = randomDateTimeOffset;
                ResolvedAddress foundAddress = resolvedAddress.DeepClone();
                ResolvedAddress foundAddressLocked = foundAddress.DeepClone();

                this.dateTimeBrokerMock.Setup(broker =>
                    broker.GetCurrentDateTimeOffset()).Returns(randomDateTimeOffset);

                this.resolvedAddressProcessingServiceMock.Setup(processing =>
                    processing.ModifyResolvedAddressAsync(resolvedAddress))
                        .ReturnsAsync(foundAddress);

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

                foundAddressLocked.IsProcessing = false;
                foundAddressLocked.RetryCount = 0;
                foundAddressLocked.UpdatedDate = randomDateTimeOffset;

                this.resolvedAddressProcessingServiceMock.Setup(processing =>
                    processing.ModifyResolvedAddressAsync(foundAddressLocked))
                        .ReturnsAsync(foundAddressLocked);
            }

            // When
            List<Guid> actualBatchList =
                await this.resolvedAddressOrchestrationService.ExportResolvedAddressesAsync();

            // Then

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddresses(),
                    Times.Once);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifier(),
                    Times.Once);

            this.csvHelperBrokerMock.Verify(service =>
                service.MapObjectToCsvAsync(
                    It.Is(SameResolvedAddressReturnsAs(returnResolvedAddresses)), false, null, true),
                        Times.Once);

            foreach (ResolvedAddress resolvedAddress in storageResolvedAddresses)
            {
                this.dateTimeBrokerMock.Verify(broker =>
                    broker.GetCurrentDateTimeOffset(),
                        Times.Exactly(storageResolvedAddresses.Count));

                this.resolvedAddressProcessingServiceMock.Verify(service =>
                    service.ModifyResolvedAddressAsync(resolvedAddress),
                        Times.Exactly(2));

                this.csvHelperBrokerMock.Verify(service =>
                    service.MapObjectToCsvAsync(
                        It.Is(SameResolvedAddressReturnsAs(returnResolvedAddresses)), false, null, true),
                            Times.Once);

                this.documentProcessingServiceMock.Verify(service =>
                    service.AddDocumentAsync(It.IsAny<Stream>(), inputFileName, inputContainer),
                        Times.Once);
            }

            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
