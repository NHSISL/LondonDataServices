// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Force.DeepCloner;
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

            Guid randomIdentifier = Guid.NewGuid();
            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;

            List<Address> randomAddresses = new List<Address>
                {
                    CreateRandomAddress(randomDateTimeOffset),
                    CreateRandomAddress(randomDateTimeOffset)
                };

            List<Address> inputAddresses = randomAddresses;
            List<Address> validatedAddresses = inputAddresses.DeepClone();

            validatedAddresses.ForEach(address =>
            {
                address.Id = randomIdentifier;
                address.CreatedBy = "System";
                address.CreatedDate = randomDateTimeOffset;
                address.UpdatedBy = "System";
                address.UpdatedDate = randomDateTimeOffset;
            });

            List<Address> newAddresses = new List<Address> { validatedAddresses.First() };

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAddresses())
                    .Returns(new List<Address> { randomAddresses.Last() }.AsQueryable());

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(randomIdentifier);

            // when
            await this.addressService
                .BulkAddAddressesAsync(inputAddresses, inputFileName);

            // then
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(randomAddresses.Count * 2));

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifier(),
                    Times.Exactly(randomAddresses.Count));

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAddresses(),
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