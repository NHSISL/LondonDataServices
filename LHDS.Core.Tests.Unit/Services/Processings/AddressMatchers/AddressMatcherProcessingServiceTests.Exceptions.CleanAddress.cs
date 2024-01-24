// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using FluentAssertions;
using LHDS.Core.Models.Processings.AddressMatchers.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.AddressMatchers
{
    public partial class AddressMatcherProcessingServiceTests
    {
        [Fact]
        public void ShouldThrowServiceExceptionOnCleanAddressIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string address = GetRandomString();
            string exceptionMessage = GetRandomString();
            var serviceException = new Exception(exceptionMessage);

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
            Action cleanAddressAction = () => this.addressMatcherProcessingService.CleanAddress(address);

            AddressMatcherProcessingServiceException actualAddressMatcherProcessingServiceException =
                Assert.Throws<AddressMatcherProcessingServiceException>(cleanAddressAction);

            // then
            actualAddressMatcherProcessingServiceException.Should()
                .BeEquivalentTo(expectedAddressMatcherProcessingServiceException);

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