// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Addresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Addresses
{
    public partial class AddressServiceTests
    {
        [Fact]
        public async Task ShouldReturnAddressesByPostCode()
        {
            // given
            List<Address> randomAddresses = CreateRandomAddresses().ToList();
            string randomString = GetRandomString();
            string inputPostCode = randomString;

            foreach (var address in randomAddresses)
            {
                address.PostCode = inputPostCode;
            }

            List<Address> expectedAddresses = randomAddresses.DeepClone();
            List<Address> extraRandomAddresses = CreateRandomAddresses().ToList();
            List<Address> allRandomAddresses = new List<Address>();
            allRandomAddresses.AddRange(randomAddresses);
            allRandomAddresses.AddRange(extraRandomAddresses);

            IQueryable<Address> storageAddresses = allRandomAddresses.AsQueryable();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAddressesAsync())
                    .ReturnsAsync(storageAddresses);

            // when
            List<Address> actualAddresses = await this.addressService.RetrieveAddressesByPostCodeAsync(inputPostCode);

            // then
            actualAddresses.Should().BeEquivalentTo(expectedAddresses);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAddressesAsync(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}