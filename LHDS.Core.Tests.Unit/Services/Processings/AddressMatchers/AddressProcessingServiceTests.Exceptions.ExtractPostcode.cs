// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Processings.AddressMatchers.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.AddressMatchers
{
    public partial class AddressMatcherProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnExtractPostcodeIfServiceErrorOccursAsync()
        {
            // given
            string someAddress = GetRandomString();
            var serviceException = new Exception();

            var failedAddressMartcherProcessingServiceException =
                new FailedAddressMatcherProcessingServiceException(
                    message: "Failed Address matcher processing service error occurred, contact support.",
                    innerException: serviceException);

            var expectedAddressMatcherProcessingServiveException =
                new AddressMatcherProcessingServiceException(
                    message: "Address matcher processing service error occurred, contact support.",
                    innerException: failedAddressMartcherProcessingServiceException);

            // when
            Action addAddressMatcherTask = () =>
                addressMatcherProcessingService.ExtractPostCode(someAddress);

            AddressMatcherProcessingServiceException actualAddressMatcherProcessingServiceException =
                Assert.Throws<AddressMatcherProcessingServiceException>(addAddressMatcherTask);

            // then
            actualAddressMatcherProcessingServiceException.Should()
                .BeEquivalentTo(expectedAddressMatcherProcessingServiveException);
        }
    }
}
