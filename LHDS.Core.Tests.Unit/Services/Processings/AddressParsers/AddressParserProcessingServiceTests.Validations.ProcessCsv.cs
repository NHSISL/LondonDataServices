// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressParsers.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.AddressParsers
{
    public partial class AddressParserProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnProcessIfAddressIsNullAndLogItAsync()
        {
            // given
            byte[] nullAddresses = null;

            var nullAddressParserException =
                new NullAddressParserException(message: "Address parser is null.");

            var expectedAddressParserValidationException =
                new AddressParserValidationException(
                    message: "Address parser validation errors occurred, please try again.",
                    innerException: nullAddressParserException);

            // when
            ValueTask<List<Address>> addAddressParserProcessingTask =
                this.addressParserProcessingService.ProcessCsvAsync(nullAddresses);

            AddressParserValidationException actualAddressParserValidationException =
                await Assert.ThrowsAsync<AddressParserValidationException>(() =>
                    addAddressParserProcessingTask.AsTask());

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