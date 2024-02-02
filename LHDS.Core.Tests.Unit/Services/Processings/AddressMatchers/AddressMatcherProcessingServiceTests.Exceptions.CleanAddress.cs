// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public void ShouldThrowServiceExceptionOnCleanAddressIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var mock = new Mock<AddressMatcherProcessingService>(
                addressMatcherServiceMock.Object,
                loggingBrokerMock.Object)
            { CallBase = true };

            string address = GetRandomString();
            string exceptionMessage = GetRandomString();
            var serviceException = new Exception(exceptionMessage);
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
            Action cleanAddressAction = () => addressMatcherProcessingService.CleanAddress(address);

            AddressMatcherProcessingServiceException actualAddressMatcherProcessingServiceException =
                Assert.Throws<AddressMatcherProcessingServiceException>(cleanAddressAction);

            // then
            actualAddressMatcherProcessingServiceException.Should()
                .BeEquivalentTo(expectedAddressMatcherProcessingServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressMatcherProcessingServiceException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}