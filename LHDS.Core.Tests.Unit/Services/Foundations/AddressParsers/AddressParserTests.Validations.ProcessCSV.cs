// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressParsers.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressParsers
{
    public partial class AddressParserTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnProcessCsvIfDataIsNullAndLogItAsync(string invalidText)
        {
            // given
            byte[] nullData = null;
            string invalidFileName = invalidText;

            var invalidArgumentAddressParserException =
                new InvalidArgumentAddressParserException(
                    message: "Invalid arguments. Please correct the errors and try again.");

            invalidArgumentAddressParserException.AddData(
                key: "data",
                values: "Data is required");

            invalidArgumentAddressParserException.AddData(
                key: "filename",
                values: "Text is required");

            var expectedAddressParserValidationException =
                new AddressParserValidationException(
                    message: "Address parser validation errors occurred, please try again.",
                    innerException: invalidArgumentAddressParserException);

            // when
            ValueTask<List<Address>> processCSVAddressTask =
                this.addressParserService.ProcessCsvAsync(nullData, invalidFileName);

            AddressParserValidationException actualAddressParserValidationException =
                await Assert.ThrowsAsync<AddressParserValidationException>(async () =>
                    await processCSVAddressTask);

            // then
            actualAddressParserValidationException.Should().BeEquivalentTo(expectedAddressParserValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressParserValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnProcessCsvIfDataIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            string invalidAddress = invalidText;
            string invalidFileName = invalidText;

            var invalidArgumentAddressParserException =
                new InvalidArgumentAddressParserException(
                    message: "Invalid arguments. Please correct the errors and try again.");

            invalidArgumentAddressParserException.AddData(
                key: "data",
                values: "Text is required");

            invalidArgumentAddressParserException.AddData(
                key: "filename",
                values: "Text is required");

            var expectedAddressParserValidationException =
                new AddressParserValidationException(
                    message: "Address parser validation errors occurred, please try again.",
                    innerException: invalidArgumentAddressParserException);

            // when
            ValueTask<List<Address>> processCSVAddressTask =
                this.addressParserService.ProcessCsvAsync(invalidAddress, invalidFileName);

            AddressParserValidationException actualAddressParserValidationException =
                await Assert.ThrowsAsync<AddressParserValidationException>(async () =>
                    await processCSVAddressTask);

            // then
            actualAddressParserValidationException.Should()
                .BeEquivalentTo(expectedAddressParserValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressParserValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
