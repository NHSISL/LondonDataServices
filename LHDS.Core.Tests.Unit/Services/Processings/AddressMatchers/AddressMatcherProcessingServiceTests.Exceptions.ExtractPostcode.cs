// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Processings.AddressMatchers.Exceptions;
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
            string someAddress = "123 Christo Street, London, W1A 1AA, United Kingdom";
            var serviceException = new Exception();

            var failedAddressMatcherProcessingServiceException =
                new FailedAddressMatcherProcessingServiceException(
                    message: "Failed address matcher processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAddressMatcherProcessingServiceException =
                new AddressMatcherProcessingServiceException(
                    message: "Address matcher processing service error occurred, please contact support.",
                    innerException: failedAddressMatcherProcessingServiceException);

            this.loggingBrokerMock.Setup(broker =>
                broker.LogNothing()).
                    Throws(serviceException);

            // when
            Action extractAddressAction = () => this.addressMatcherProcessingService.ExtractPostCode(someAddress);

            AddressMatcherProcessingServiceException actualAddressMatcherProcessingServiceException =
                Assert.Throws<AddressMatcherProcessingServiceException>(extractAddressAction);

            // then
            actualAddressMatcherProcessingServiceException.Should().BeEquivalentTo(expectedAddressMatcherProcessingServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogNothing(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressMatcherProcessingServiceException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
