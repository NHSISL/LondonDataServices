using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddresses
{
    public partial class ResolvedAddressServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfResolvedAddressIsNullAndLogItAsync()
        {
            // given
            ResolvedAddress nullResolvedAddress = null;
            var nullResolvedAddressException = new NullResolvedAddressException(message: "ResolvedAddress is null.");

            var expectedResolvedAddressValidationException =
                new ResolvedAddressValidationException(
                    message: "ResolvedAddress validation errors occurred, please try again.",
                    innerException: nullResolvedAddressException);

            // when
            ValueTask<ResolvedAddress> modifyResolvedAddressTask =
                this.resolvedAddressService.ModifyResolvedAddressAsync(nullResolvedAddress);

            ResolvedAddressValidationException actualResolvedAddressValidationException =
                await Assert.ThrowsAsync<ResolvedAddressValidationException>(
                    modifyResolvedAddressTask.AsTask);

            // then
            actualResolvedAddressValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedResolvedAddressValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateResolvedAddressAsync(It.IsAny<ResolvedAddress>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfResolvedAddressIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            var invalidResolvedAddress = new ResolvedAddress
            {
                // TODO:  Add default values for your properties i.e. Name = invalidText
            };

            var invalidResolvedAddressException = 
                new InvalidResolvedAddressException(
                    message: "Invalid resolvedAddress. Please correct the errors and try again.");

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.Id),
                values: "Id is required");

            //invalidResolvedAddressException.AddData(
            //    key: nameof(ResolvedAddress.Name),
            //    values: "Text is required");

            // TODO: Add or remove data here to suit the validation needs for the ResolvedAddress model

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.CreatedDate),
                values: "Date is required");

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.CreatedBy),
                values: "Text is required");

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.UpdatedDate),
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(ResolvedAddress.CreatedDate)}"
                });

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.UpdatedBy),
                values: "Text is required");

            var expectedResolvedAddressValidationException =
                new ResolvedAddressValidationException(
                    message: "ResolvedAddress validation errors occurred, please try again.",
                    innerException: invalidResolvedAddressException);

            // when
            ValueTask<ResolvedAddress> modifyResolvedAddressTask =
                this.resolvedAddressService.ModifyResolvedAddressAsync(invalidResolvedAddress);

            ResolvedAddressValidationException actualResolvedAddressValidationException =
                await Assert.ThrowsAsync<ResolvedAddressValidationException>(
                    modifyResolvedAddressTask.AsTask);

            //then
            actualResolvedAddressValidationException.Should().BeEquivalentTo(expectedResolvedAddressValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedResolvedAddressValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateResolvedAddressAsync(It.IsAny<ResolvedAddress>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress(randomDateTimeOffset);
            ResolvedAddress invalidResolvedAddress = randomResolvedAddress;
            
            var invalidResolvedAddressException = 
                new InvalidResolvedAddressException(
                    message: "Invalid resolvedAddress. Please correct the errors and try again.");

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.UpdatedDate),
                values: $"Date is the same as {nameof(ResolvedAddress.CreatedDate)}");

            var expectedResolvedAddressValidationException =
                new ResolvedAddressValidationException(
                    message: "ResolvedAddress validation errors occurred, please try again.",
                    innerException: invalidResolvedAddressException);

            // when
            ValueTask<ResolvedAddress> modifyResolvedAddressTask =
                this.resolvedAddressService.ModifyResolvedAddressAsync(invalidResolvedAddress);

            ResolvedAddressValidationException actualResolvedAddressValidationException =
                await Assert.ThrowsAsync<ResolvedAddressValidationException>(
                    modifyResolvedAddressTask.AsTask);

            // then
            actualResolvedAddressValidationException.Should().BeEquivalentTo(expectedResolvedAddressValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedResolvedAddressValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressByIdAsync(invalidResolvedAddress.Id),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}