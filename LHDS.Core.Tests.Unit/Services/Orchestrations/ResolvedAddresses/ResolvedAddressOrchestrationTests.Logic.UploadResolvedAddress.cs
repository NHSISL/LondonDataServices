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
        public async Task ShouldUploadResolvedAddressAsync()
        {
            // Given
            DateTimeOffset dateTimeOffset = GetRandomDateTimeOffset();
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomResolvedAddresses();
            List<ResolvedAddress> storageResolvedAddresses = randomResolvedAddresses.DeepClone();
            List<ResolvedAddress> updatedResolvedAddresses = randomResolvedAddresses.DeepClone();
            List<ResolvedAddress> verifyResolvedAddresses = randomResolvedAddresses.DeepClone();
            List<ResolvedAddressReturn> returnResolvedAddresses = MapToResolvedAddressReturn(storageResolvedAddresses);
            string ouputCsv = GetRandomString();
            byte[] inputData = Encoding.UTF8.GetBytes(ouputCsv);
            Stream inputStream = new MemoryStream(inputData);
            Stream expectedStream = inputStream;
            Stream actualStream = new MemoryStream();
            Guid batchReference = Guid.NewGuid();
            string inputFileName = $"{batchReference}.csv";
            string inputContainer = blobContainers.Addresses;

            this.resolvedAddressProcessingServiceMock.Setup(service =>
                service.RetrieveAllResolvedAddresses()).
                    Returns(storageResolvedAddresses.AsQueryable());

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(batchReference);

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

            foreach (ResolvedAddress resolvedAddress in updatedResolvedAddresses)
            {
                resolvedAddress.BatchReference = batchReference;
                resolvedAddress.IsProcessed = true;
                resolvedAddress.UpdatedDate = dateTimeOffset;

                this.dateTimeBrokerMock.Setup(broker =>
                    broker.GetCurrentDateTimeOffset()).Returns(dateTimeOffset);

                this.resolvedAddressProcessingServiceMock.Setup(service =>
                    service.ModifyResolvedAddressAsync(resolvedAddress))
                        .ReturnsAsync(resolvedAddress);
            }

            // When
            Guid? actualBatchReference =
                await this.resolvedAddressOrchestrationService.UploadResolvedAddressesAsync();

            // Then
            Assert.True(IsSameStream(expectedStream, actualStream));

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

            this.documentProcessingServiceMock.Verify(service =>
                service.AddDocumentAsync(It.IsAny<Stream>(), inputFileName, inputContainer),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Exactly(storageResolvedAddresses.Count));

            foreach (ResolvedAddress resolvedAddress in storageResolvedAddresses)
            {
                this.resolvedAddressProcessingServiceMock.Verify(service =>
                    service.ModifyResolvedAddressAsync(resolvedAddress),
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
