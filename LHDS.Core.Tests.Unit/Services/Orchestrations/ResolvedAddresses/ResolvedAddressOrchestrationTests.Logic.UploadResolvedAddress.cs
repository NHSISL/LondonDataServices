// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Documents;
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
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomResolvedAddresses();
            List<ResolvedAddress> storageResolvedAddresses = randomResolvedAddresses.DeepClone();
            List<ResolvedAddress> verifyResolvedAddresses = randomResolvedAddresses.DeepClone();
            string ouputCsv = GetRandomString();
            byte[] inputData = Encoding.UTF8.GetBytes(ouputCsv);
            Guid batchReference = Guid.NewGuid();
            string fileName = $"{batchReference}.csv";
            string container = "Addresses";

            Document inputDocument = new Document
            {
                DocumentData = inputData,
                FileName = fileName
            };

            this.resolvedAddressProcessingServiceMock.Setup(service =>
                service.RetrieveAllResolvedAddresses()).
                    Returns(storageResolvedAddresses.AsQueryable());

            this.csvMapperProcessingServiceMock.Setup(service =>
                service.MapObjectToCsvAsync(storageResolvedAddresses, false, true))
                    .ReturnsAsync(ouputCsv);

            this.documentProcessingServiceMock.Setup(service =>
                service.AddDocumentAsync(inputDocument, container))
                    .ReturnsAsync(fileName);

            foreach(ResolvedAddress resolvedAddress in storageResolvedAddresses)
            {
                resolvedAddress.BatchReference = batchReference;
                resolvedAddress.IsProcessed = true;

                this.resolvedAddressProcessingServiceMock.Setup(service => 
                    service.ModifyResolvedAddressAsync(resolvedAddress))
                        .ReturnsAsync(resolvedAddress);
            }

            // When
            Guid actualBatchReference =
                await this.resolvedAddressOrchestrationService.UploadResolvedAddressesAsync();

            // Then
            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddresses(),
                    Times.Once);

            this.csvMapperProcessingServiceMock.Verify(service =>
                service.MapObjectToCsvAsync(storageResolvedAddresses, false, true),
                    Times.Once);

            this.documentProcessingServiceMock.Verify(service =>
                service.AddDocumentAsync(inputDocument, container),
                    Times.Once);

            foreach (ResolvedAddress resolvedAddress in storageResolvedAddresses)
            {
                this.resolvedAddressProcessingServiceMock.Verify(service =>
                    service.ModifyResolvedAddressAsync(resolvedAddress), 
                        Times.Once);
            }

            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.csvMapperProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
