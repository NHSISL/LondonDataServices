// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Addresses
{
    public partial class AddressProcessingServiceTests
    {
        [Fact(Skip = "No longer used, will refactor out")]
        public async Task IsExactMatchForAddressAsShouldReturnTrueIfMatchFoundAsync()
        {
            //// Given
            //IQueryable<Address> randomAddresses = CreateRandomAddresses();
            //IQueryable<Address> storageAddresses = randomAddresses;
            //string randomAddress = randomAddresses.First().PostalAddress;
            //string inputAddress = randomAddress;
            //bool expectedResult = true;

            //this.addressServiceMock.Setup(broker =>
            //    broker.RetrieveAllAddresses())
            //        .Returns(storageAddresses);

            //// When
            //bool actualResult = await this.addressProcessingService
            //    .IsExactMatchForAddressAsync(inputAddress);

            //// Then
            //actualResult.Should().Be(expectedResult);

            //this.addressServiceMock.Verify(service =>
            //    service.RetrieveAllAddresses(),
            //        Times.Once);

            //this.addressServiceMock.VerifyNoOtherCalls();
            //this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
