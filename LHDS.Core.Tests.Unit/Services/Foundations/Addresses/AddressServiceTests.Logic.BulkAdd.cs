// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Addresses
{
    public partial class AddressServiceTests
    {
        [Fact]
        public async Task ShouldBulkAddAddressesAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            string randomFileName = GetRandomString();
            string imputFileName = randomFileName;

            List<Address> randomAddresses = new List<Address>
                {
                    CreateRandomAddress(randomDateTimeOffset),
                    CreateRandomAddress(randomDateTimeOffset)
                };

            List<Address> inputAddresses = randomAddresses;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAddresses())
                    .Returns(new List<Address> { randomAddresses.Last() }.AsQueryable());

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            await this.addressService
                .BulkAddAddressesAsync(inputAddresses, imputFileName);

            // then
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Exactly(randomAddresses.Count));

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAddresses(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.BulkInsertAddressesAsync(inputAddresses),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}