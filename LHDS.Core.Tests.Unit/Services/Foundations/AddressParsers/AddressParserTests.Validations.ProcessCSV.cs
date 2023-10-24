// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
        [Fact]
        public async Task ShouldThrowValidationExceptionOnProcessCSVIfDataIsNullAndLogItAsync()
        {
            // given
            byte[] nullAddresses = null;

            var nullAddressesParserException =
                new NullAddressParserException(message: "Address parser is null.");

            var expectedAddressParserValidationException =
                new AddressParserValidationException(
                    message: "Address parser validation errors occurred, please try again.",
                    innerException: nullAddressesParserException);

            // when
            Task<List<Address>> processCSVAddressTask = this.addressParserService.ProcessCSVAsync(nullAddresses);

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
    }
}
