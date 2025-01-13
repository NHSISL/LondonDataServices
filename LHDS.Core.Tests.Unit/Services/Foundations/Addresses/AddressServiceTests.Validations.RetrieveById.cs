// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.Addresses.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Addresses
{
    public partial class AddressServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidAddressId = Guid.Empty;

            var invalidAddressException =
                new InvalidAddressException(
                    message: "Invalid address. Please correct the errors and try again.");

            invalidAddressException.AddData(
                key: nameof(Address.Id),
                values: "Id is required");

            var expectedAddressValidationException =
                new AddressValidationException(
                    message: "Address validation errors occurred, please try again.",
                    innerException: invalidAddressException);

            // when
            ValueTask<Address> retrieveAddressByIdTask =
                this.addressService.RetrieveAddressByIdAsync(invalidAddressId);

            AddressValidationException actualAddressValidationException =
                await Assert.ThrowsAsync<AddressValidationException>(
                    retrieveAddressByIdTask.AsTask);

            // then
            actualAddressValidationException.Should()
                .BeEquivalentTo(expectedAddressValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfAddressIsNotFoundAndLogItAsync()
        {
            //given
            Guid someAddressId = Guid.NewGuid();
            Address noAddress = null;

            var notFoundAddressException =
                new NotFoundAddressException(someAddressId);

            var expectedAddressValidationException =
                new AddressValidationException(
                    message: "Address validation errors occurred, please try again.",
                    innerException: notFoundAddressException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noAddress);

            //when
            ValueTask<Address> retrieveAddressByIdTask =
                this.addressService.RetrieveAddressByIdAsync(someAddressId);

            AddressValidationException actualAddressValidationException =
                await Assert.ThrowsAsync<AddressValidationException>(
                    retrieveAddressByIdTask.AsTask);

            //then
            actualAddressValidationException.Should().BeEquivalentTo(expectedAddressValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}