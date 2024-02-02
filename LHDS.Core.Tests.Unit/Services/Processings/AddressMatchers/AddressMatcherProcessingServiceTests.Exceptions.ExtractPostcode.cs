// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Processings.AddressMatchers.Exceptions;
using LHDS.Core.Services.Processings.AddressMatchers;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.AddressMatchers
{
    public partial class AddressMatcherProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnExtractPostcodeIfServiceErrorOccursAsync()
        {
            // given
            var mock = new Mock<AddressMatcherProcessingService>(
                addressMatcherServiceMock.Object,
                loggingBrokerMock.Object)
            { CallBase = true };

            string someAddress = "123 Christo Street, London, W1A 1AA, United Kingdom";
            var serviceException = new Exception();
            mock.Setup(x => x.ValidateAddress(It.IsAny<string>())).Throws(serviceException);
            AddressMatcherProcessingService addressMatcherProcessingService = mock.Object;

            var failedAddressMatcherProcessingServiceException =
                new FailedAddressMatcherProcessingServiceException(
                    message: "Failed address matcher processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAddressMatcherProcessingServiceException =
                new AddressMatcherProcessingServiceException(
                    message: "Address matcher processing service error occurred, please contact support.",
                    innerException: failedAddressMatcherProcessingServiceException);

            // when
            Action extractAddressAction = () => addressMatcherProcessingService.ExtractPostCode(someAddress);

            AddressMatcherProcessingServiceException actualAddressMatcherProcessingServiceException =
                Assert.Throws<AddressMatcherProcessingServiceException>(extractAddressAction);

            // then
            actualAddressMatcherProcessingServiceException.Should()
                .BeEquivalentTo(expectedAddressMatcherProcessingServiceException);
        }
    }
}