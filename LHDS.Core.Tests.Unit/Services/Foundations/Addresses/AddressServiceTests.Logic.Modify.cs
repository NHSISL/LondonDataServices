// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
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
        public async Task ShouldModifyAddressAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomEntraUserId = GetRandomStringWithLengthOf(50);
            Address randomAddress = CreateRandomModifyAddress(randomDateTimeOffset, randomEntraUserId);
            Address inputAddress = randomAddress;
            Address storageAddress = inputAddress.DeepClone();
            storageAddress.UpdatedDate = randomAddress.CreatedDate;
            Address updatedAddress = inputAddress;
            Address expectedAddress = updatedAddress.DeepClone();
            Guid addressId = inputAddress.Id;

            this.securityAuditBrokerMock.Setup(service =>
                service.ApplyModifyAuditValuesAsync(randomAddress))
                    .ReturnsAsync(randomAddress);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressByIdAsync(addressId))
                    .ReturnsAsync(storageAddress);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateAddressAsync(inputAddress))
                    .ReturnsAsync(updatedAddress);

            // when
            Address actualAddress =
                await this.addressService.ModifyAddressAsync(inputAddress);

            // then
            actualAddress.Should().BeEquivalentTo(expectedAddress);

            this.securityAuditBrokerMock.Verify(service =>
               service.ApplyModifyAuditValuesAsync(inputAddress),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(brokers =>
                brokers.GetUserIdAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressByIdAsync(inputAddress.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressAsync(inputAddress),
                    Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}