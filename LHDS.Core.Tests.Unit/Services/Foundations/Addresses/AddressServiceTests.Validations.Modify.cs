using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.Addresses.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Addresses
{
    public partial class AddressServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfAddressIsNullAndLogItAsync()
        {
            // given
            Address nullAddress = null;
            var nullAddressException = new NullAddressException(message: "Address is null.");

            var expectedAddressValidationException =
                new AddressValidationException(
                    message: "Address validation errors occurred, please try again.",
                    innerException: nullAddressException);

            // when
            ValueTask<Address> modifyAddressTask =
                this.addressService.ModifyAddressAsync(nullAddress);

            AddressValidationException actualAddressValidationException =
                await Assert.ThrowsAsync<AddressValidationException>(
                    modifyAddressTask.AsTask);

            // then
            actualAddressValidationException.Should()
                .BeEquivalentTo(expectedAddressValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressAsync(It.IsAny<Address>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfAddressIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            var invalidAddress = new Address
            {
                // TODO:  Add default values for your properties i.e. Name = invalidText
            };

            var invalidAddressException = 
                new InvalidAddressException(
                        message: "Invalid address. Please correct the errors and try again.");

            invalidAddressException.AddData(
                key: nameof(Address.Id),
                values: "Id is required");

            //invalidAddressException.AddData(
            //    key: nameof(Address.Name),
            //    values: "Text is required");

            // TODO: Add or remove data here to suit the validation needs for the Address model

            invalidAddressException.AddData(
                key: nameof(Address.CreatedDate),
                values: "Date is required");

            invalidAddressException.AddData(
                key: nameof(Address.CreatedBy),
                values: "Text is required");

            invalidAddressException.AddData(
                key: nameof(Address.UpdatedDate),
                values: "Date is required");

            invalidAddressException.AddData(
                key: nameof(Address.UpdatedBy),
                values: "Text is required");

            var expectedAddressValidationException =
                new AddressValidationException(
                    message: "Address validation errors occurred, please try again.",
                    innerException: invalidAddressException);

            // when
            ValueTask<Address> modifyAddressTask =
                this.addressService.ModifyAddressAsync(invalidAddress);

            AddressValidationException actualAddressValidationException =
                await Assert.ThrowsAsync<AddressValidationException>(
                    modifyAddressTask.AsTask);

            //then
            actualAddressValidationException.Should().BeEquivalentTo(expectedAddressValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressAsync(It.IsAny<Address>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}