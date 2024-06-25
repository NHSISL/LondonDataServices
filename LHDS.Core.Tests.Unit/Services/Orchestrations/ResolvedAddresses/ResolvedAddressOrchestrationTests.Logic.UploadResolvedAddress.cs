// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.ResolvedAddresses
{
    public partial class ResolvedAddressOrchestrationTests
    {
        [Fact(Skip = "Conversion to stream")]
        public async Task ShouldUploadResolvedAddressAsync()
        {
            //// Given
            //DateTimeOffset dateTimeOffset = GetRandomDateTimeOffset();
            //List<ResolvedAddress> randomResolvedAddresses = CreateRandomResolvedAddresses();
            //List<ResolvedAddress> storageResolvedAddresses = randomResolvedAddresses.DeepClone();
            //List<ResolvedAddress> updatedResolvedAddresses = randomResolvedAddresses.DeepClone();
            //List<ResolvedAddress> verifyResolvedAddresses = randomResolvedAddresses.DeepClone();
            //List<ResolvedAddressReturn> returnResolvedAddresses = MapToResolvedAddressReturn(storageResolvedAddresses);
            //string ouputCsv = GetRandomString();
            //byte[] inputData = Encoding.UTF8.GetBytes(ouputCsv);
            //Guid batchReference = Guid.NewGuid();
            //string fileName = $"{batchReference}.csv";
            //string container = blobContainers.Addresses;

            //Document inputDocument = new Document
            //{
            //    DocumentData = inputData,
            //    FileName = fileName
            //};

            //this.resolvedAddressProcessingServiceMock.Setup(service =>
            //    service.RetrieveAllResolvedAddresses()).
            //        Returns(storageResolvedAddresses.AsQueryable());

            //this.identifierBrokerMock.Setup(broker =>
            //    broker.GetIdentifier())
            //        .Returns(batchReference);

            //this.csvHelperBrokerMock.Setup(service =>
            //    service.MapObjectToCsvAsync(
            //        It.Is(SameResolvedAddressReturnsAs(returnResolvedAddresses)), false, null, true))
            //            .ReturnsAsync(ouputCsv);

            //foreach (ResolvedAddress resolvedAddress in updatedResolvedAddresses)
            //{
            //    resolvedAddress.BatchReference = batchReference;
            //    resolvedAddress.IsProcessed = true;
            //    resolvedAddress.UpdatedDate = dateTimeOffset;

            //    this.dateTimeBrokerMock.Setup(broker =>
            //        broker.GetCurrentDateTimeOffset()).Returns(dateTimeOffset);

            //    this.resolvedAddressProcessingServiceMock.Setup(service =>
            //        service.ModifyResolvedAddressAsync(resolvedAddress))
            //            .ReturnsAsync(resolvedAddress);
            //}

            //// When
            //Guid? actualBatchReference =
            //    await this.resolvedAddressOrchestrationService.UploadResolvedAddressesAsync();

            //// Then
            //this.resolvedAddressProcessingServiceMock.Verify(service =>
            //    service.RetrieveAllResolvedAddresses(),
            //        Times.Once);

            //this.identifierBrokerMock.Verify(broker =>
            //    broker.GetIdentifier(),
            //        Times.Once);

            //this.csvHelperBrokerMock.Verify(service =>
            //    service.MapObjectToCsvAsync(
            //        It.Is(SameResolvedAddressReturnsAs(returnResolvedAddresses)), false, null, true),
            //            Times.Once);

            //this.documentProcessingServiceMock.Verify(service =>
            //    service.AddDocumentAsync(It.Is(SameDocumentAs(inputDocument)), container),
            //        Times.Once);

            //this.dateTimeBrokerMock.Verify(broker =>
            //    broker.GetCurrentDateTimeOffset(),
            //        Times.Exactly(storageResolvedAddresses.Count));

            //foreach (ResolvedAddress resolvedAddress in storageResolvedAddresses)
            //{
            //    this.resolvedAddressProcessingServiceMock.Verify(service =>
            //        service.ModifyResolvedAddressAsync(resolvedAddress),
            //            Times.Once);
            //}

            //this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            //this.identifierBrokerMock.VerifyNoOtherCalls();
            //this.csvHelperBrokerMock.VerifyNoOtherCalls();
            //this.documentProcessingServiceMock.VerifyNoOtherCalls();
            //this.dateTimeBrokerMock.VerifyNoOtherCalls();
            //this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
