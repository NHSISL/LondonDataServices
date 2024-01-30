// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressParsers.Exceptions;
using LHDS.Core.Models.Processings.AddressParsers.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.AddressParsers
{
    public partial class AddressParserProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnProcessCsvIfDataIsNullAndLogItAsync()
        {
            // given
            byte[] nullAddresses = null;

            var invalidArgumentAddressParserProcessingException =
                new InvalidArgumentAddressParserProcessingException(message: "Address parser is null.");

            var expectedAddressParserProcessingValidationException =
                new AddressParserProcessingValidationException(
                    message: "Address parser processing validation errors occurred, please try again.",
                    innerException: invalidArgumentAddressParserProcessingException);

            // when
            ValueTask<List<Address>> processCSVAddressTask =
                this.addressParserProcessingService.ProcessCsvAsync(nullAddresses);

            AddressParserProcessingValidationException actualAddressParserProcessingValidationException =
                await Assert.ThrowsAsync<AddressParserProcessingValidationException>(async () =>
                    await processCSVAddressTask);

            // then
            actualAddressParserProcessingValidationException.Should().BeEquivalentTo(expectedAddressParserProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressParserProcessingValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnProcessIDataIsInvalidAndLogItAsync(
        string invalidText)
        {
            // given
            string invalidAddress = invalidText;

            var invalidArgumentAddressParserProcessingException =
                new InvalidArgumentAddressParserProcessingException(
                    message: "Invalid address parser processing argument, please correct the errors and try again.");

            invalidArgumentAddressParserProcessingException.AddData(
                key: "address",
                values: "Text is required");

            var expectedAddressParserValidationException =
                new AddressParserProcessingValidationException(
                    message: "Address parser processing validation errors occurred, please try again.",
                    innerException: invalidArgumentAddressParserProcessingException);

            // when
            ValueTask<List<Address>> addressParserTask =
                addressParserProcessingService.ProcessCsvAsync(invalidAddress);

            AddressParserProcessingValidationException actualAddressParserValidationException =
                await Assert.ThrowsAsync<AddressParserProcessingValidationException>(() =>
                addressParserTask.AsTask());

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