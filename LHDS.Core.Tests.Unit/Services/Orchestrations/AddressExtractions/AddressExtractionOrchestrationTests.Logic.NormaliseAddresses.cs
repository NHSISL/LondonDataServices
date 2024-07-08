// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressExtractions
{
    public partial class AddressExtractionOrchestrationServiceTests
    {
        [Fact(Skip = "no longer used, will refactor out")]
        public async Task ShouldNormaliseAddressesAsync()
        {
            //// Given
            //DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            //Address randomAddress = CreateRandomAddress();
            //randomAddress.IsNormalised = false;
            //randomAddress.IsErrored = false;
            //randomAddress.IsProcessing = false;
            //List<Address> randomAddresses = new List<Address> { randomAddress };

            //AddressNormalisation addressNormalisation = new AddressNormalisation
            //{
            //    PostalAddress = GetRandomString(),
            //    JsonPostalAddress = GetRandomString()
            //};

            //dateTimeBrokerMock
            //    .Setup(broker => broker.GetCurrentDateTimeOffset())
            //        .Returns(randomDateTimeOffset);

            //addressProcessingServiceMock
            //    .SetupSequence(service => service.RetrieveAllAddresses())
            //        .Returns(randomAddresses.AsQueryable())
            //        .Returns(new List<Address>().AsQueryable());

            //Address addressToProcess = randomAddress.DeepClone();
            //addressToProcess.IsProcessing = true;
            //addressToProcess.UpdatedDate = randomDateTimeOffset;
            //Address lockForProcessingAddress = addressToProcess.DeepClone();

            //addressProcessingServiceMock
            //.Setup(service => service.ModifyAddressAsync(It.Is(SameAddressAs(addressToProcess))))
            //    .ReturnsAsync(lockForProcessingAddress);

            //addressNormalisationProcessingServiceMock
            //    .Setup(service => service.GetNormalisedAddress(randomAddress.GetFormattedAddress()))
            //        .ReturnsAsync(addressNormalisation);

            //lockForProcessingAddress.JsonPostalAddress = addressNormalisation.JsonPostalAddress;
            //lockForProcessingAddress.PostalAddress = addressNormalisation.PostalAddress;
            //lockForProcessingAddress.IsErrored = false;
            //lockForProcessingAddress.IsNormalised = true;
            //lockForProcessingAddress.IsProcessing = false;
            //lockForProcessingAddress.UpdatedDate = randomDateTimeOffset;
            //Address modifiedAddress = lockForProcessingAddress.DeepClone();

            //addressProcessingServiceMock
            //    .Setup(service => service.ModifyAddressAsync(lockForProcessingAddress))
            //        .ReturnsAsync(modifiedAddress);

            //// When
            //await this.addressExtractionOrchestrationService.NormaliseAddressesAsync();

            //// Then
            //addressProcessingServiceMock
            //    .Verify(service => service.RetrieveAllAddresses(),
            //        Times.Exactly(2));

            //addressProcessingServiceMock
            //    .Verify(service => service.ModifyAddressAsync(It.Is(SameAddressAs(addressToProcess))),
            //        Times.Once);

            //addressNormalisationProcessingServiceMock
            //    .Verify(service => service.GetNormalisedAddress(randomAddress.GetFormattedAddress()),
            //        Times.Once);

            //dateTimeBrokerMock
            //    .Verify(broker => broker.GetCurrentDateTimeOffset(),
            //        Times.Exactly(2));

            //addressProcessingServiceMock
            //    .Verify(service => service.ModifyAddressAsync(It.Is(SameAddressAs(modifiedAddress))),
            //        Times.Once);

            //this.csvHelperBrokerMock.VerifyNoOtherCalls();
            //this.auditBrokerMock.VerifyNoOtherCalls();
            //this.loggingBrokerMock.VerifyNoOtherCalls();
            //this.addressNormalisationProcessingServiceMock.VerifyNoOtherCalls();
            //this.addressProcessingServiceMock.VerifyNoOtherCalls();
            //this.dateTimeBrokerMock.VerifyNoOtherCalls();
            //this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}

