// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddressParsers.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddressParsers
{
    public partial class ResolvedAddressParserTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnProcessCsvIfDataIsNullAndLogItAsync(string invalidText)
        {
            // given
            byte[] nullResolvedAddresses = null;
            string invalidFileName = invalidText;

            var invalidArgumentResolvedAddressParserException =
                new InvalidArgumentResolvedAddressParserException(
                    message: "Invalid argument. Please correct the errors and try again.");

            invalidArgumentResolvedAddressParserException.AddData(
                key: "data",
                values: "Data is required");

            invalidArgumentResolvedAddressParserException.AddData(
                key: "filename",
                values: "Text is required");

            var expectedResolvedAddressParserValidationException =
                new ResolvedAddressParserValidationException(
                    message: "ResolvedAddress parser validation errors occurred, please try again.",
                    innerException: invalidArgumentResolvedAddressParserException);

            // when
            ValueTask<List<ResolvedAddress>> processCSVResolvedAddressTask =
                this.addressParserService.ProcessCsvAsync(nullResolvedAddresses, invalidFileName);

            ResolvedAddressParserValidationException actualResolvedAddressParserValidationException =
                await Assert.ThrowsAsync<ResolvedAddressParserValidationException>(async () =>
                    await processCSVResolvedAddressTask);

            // then
            actualResolvedAddressParserValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressParserValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedResolvedAddressParserValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.csvMapperBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnProcessCsvIfDataIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            string invalidResolvedAddress = invalidText;
            string invalidFileName = invalidText;

            var invalidArgumentResolvedAddressParserException =
                new InvalidArgumentResolvedAddressParserException(
                    message: "Invalid argument. Please correct the errors and try again.");

            invalidArgumentResolvedAddressParserException.AddData(
                key: "data",
                values: "Text is required");

            invalidArgumentResolvedAddressParserException.AddData(
                key: "filename",
                values: "Text is required");

            var expectedResolvedAddressParserValidationException =
                new ResolvedAddressParserValidationException(
                    message: "ResolvedAddress parser validation errors occurred, please try again.",
                    innerException: invalidArgumentResolvedAddressParserException);

            // when
            ValueTask<List<ResolvedAddress>> processCSVResolvedAddressTask =
                this.addressParserService.ProcessCsvAsync(invalidResolvedAddress, invalidFileName);

            ResolvedAddressParserValidationException actualResolvedAddressParserValidationException =
                await Assert.ThrowsAsync<ResolvedAddressParserValidationException>(async () =>
                    await processCSVResolvedAddressTask);

            // then
            actualResolvedAddressParserValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressParserValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedResolvedAddressParserValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.csvMapperBrokerMock.VerifyNoOtherCalls();
        }
    }
}
