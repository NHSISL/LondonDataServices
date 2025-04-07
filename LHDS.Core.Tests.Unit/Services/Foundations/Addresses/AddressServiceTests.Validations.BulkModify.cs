// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
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
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnBulkModifyIfAddressIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            List<Address> invalidAddresses = null;
            string invalidFileName = invalidText;

            var invalidAddressException =
                new InvalidAddressException(
                    message: "Invalid address. Please correct the errors and try again.");

            invalidAddressException.AddData(
                key: "addresses",
                values: "Addresses is required");

            invalidAddressException.AddData(
                key: "fileName",
                values: "Text is required");

            var expectedAddressValidationException =
                new AddressValidationException(
                    message: "Address validation errors occurred, please try again.",
                    innerException: invalidAddressException);

            // when
            ValueTask bulkModifyAddressTask =
                this.addressService.BulkModifyAddressesAsync(addresses: invalidAddresses, fileName: invalidFileName);

            AddressValidationException actualAddressValidationException =
                await Assert.ThrowsAsync<AddressValidationException>(bulkModifyAddressTask.AsTask);

            // then
            actualAddressValidationException.Should()
                .BeEquivalentTo(expectedAddressValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressAsync(It.IsAny<Address>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}