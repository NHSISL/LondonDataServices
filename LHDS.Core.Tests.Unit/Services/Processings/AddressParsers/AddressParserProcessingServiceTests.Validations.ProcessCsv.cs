// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Processings.AddressParsers.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.AddressParsers
{
    public partial class AddressParserProcessingServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnProcessIfDataIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            byte[] invalidData = null;
            string invalidFilename = invalidText;

            var invalidArgumentAddressParserProcessingException =
                new InvalidArgumentAddressParserProcessingException(
                    message: "Invalid address parser processing argument, please correct the errors and try again.");

            invalidArgumentAddressParserProcessingException.AddData(
                key: "data",
                values: "Data is required");

            invalidArgumentAddressParserProcessingException.AddData(
                key: "filename",
                values: "Text is required");

            var expectedAddressParserValidationException =
                new AddressParserProcessingValidationException(
                    message: "Address parser processing validation errors occurred, please try again.",
                    innerException: invalidArgumentAddressParserProcessingException);

            // when
            ValueTask<List<Address>> addressParserTask =
                addressParserProcessingService.ProcessCsvAsync(invalidData, invalidFilename);

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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnProcessIfAddressIsInvalidAndLogItAsync(
        string invalidText)
        {
            // given
            string invalidAddress = invalidText;
            string invalidFilename = invalidText;

            var invalidArgumentAddressParserProcessingException =
                new InvalidArgumentAddressParserProcessingException(
                    message: "Invalid address parser processing argument, please correct the errors and try again.");

            invalidArgumentAddressParserProcessingException.AddData(
                key: "address",
                values: "Text is required");

            invalidArgumentAddressParserProcessingException.AddData(
                key: "filename",
                values: "Text is required");

            var expectedAddressParserValidationException =
                new AddressParserProcessingValidationException(
                    message: "Address parser processing validation errors occurred, please try again.",
                    innerException: invalidArgumentAddressParserProcessingException);

            // when
            ValueTask<List<Address>> addressParserTask =
                addressParserProcessingService.ProcessCsvAsync(invalidAddress, invalidFilename);

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