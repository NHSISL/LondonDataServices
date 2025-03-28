// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
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

            EntraUser randomEntraUser = CreateRandomEntraUser();
            Guid randomIdentifier = Guid.NewGuid();
            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;

            List<Address> randomAddresses = new List<Address>
                {
                    CreateRandomAddress(randomDateTimeOffset, randomEntraUser.EntraUserId),
                    CreateRandomAddress(randomDateTimeOffset, randomEntraUser.EntraUserId)
                };

            List<Address> inputAddresses = randomAddresses;
            List<Address> validatedAddresses = inputAddresses.DeepClone();

            validatedAddresses.ForEach(address =>
            {
                address.Id = randomIdentifier;
            });

            List<Address> newAddresses = new List<Address> { validatedAddresses.First() };

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAddressesAsync())
                    .ReturnsAsync(new List<Address> { randomAddresses.Last() }.AsQueryable());

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ReturnsAsync(randomIdentifier);

            // when
            await this.addressService
                .BulkAddAddressesAsync(inputAddresses, inputFileName);

            // then
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(randomAddresses.Count * 2));

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(randomAddresses.Count * 2));

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Exactly(randomAddresses.Count));

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAddressesAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.BulkInsertAddressesAsync(It.Is(SameAddressesAs(newAddresses))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}