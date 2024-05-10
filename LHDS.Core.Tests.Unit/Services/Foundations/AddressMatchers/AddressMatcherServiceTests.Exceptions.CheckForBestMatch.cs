// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressMatchers;
using LHDS.Core.Models.Foundations.AddressMatchers.Exceptions;
using LHDS.Core.Models.Processings.AddressMatchers.Exceptions;
using LHDS.Core.Services.Foundations.AddressMatchers;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressMatchers
{
    public partial class AddressMatcherServiceTests
    {
        [Fact]
        public void ShouldThrowServiceExceptionOnCheckForBestMatchIfServiceErrorOccurs()
        {
            // given
            List<KeyValuePair<string, string>> someIncomingAddressComponents =
                new List<KeyValuePair<string, string>>();

            HashSet<AddressMatch> somePossibleAddresses = new HashSet<AddressMatch>();

            var mock = new Mock<AddressMatcherService>(loggingBrokerMock.Object) { CallBase = true };
            var serviceException = new Exception();

            mock.Setup(x => x.ValidateCheckForBestMatchArguments(
                It.IsAny<HashSet<AddressMatch>>()))
                    .Throws(serviceException);

            AddressMatcherService addressMatcherService = mock.Object;

            var failedAddressMatcherServiceException =
                new FailedAddressMatcherServiceException(
                    message: "Failed address matcher service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAddressMatcherServiceException =
                new AddressMatcherServiceException(
                    message: "Address matcher service error occurred, please contact support.",
                    innerException: failedAddressMatcherServiceException);

            // when
            Action checkForBestMatchAction = () =>
                addressMatcherService.CheckForBestMatch(matchedAddresses: somePossibleAddresses);

            AddressMatcherServiceException actualAddressMatcherServiceException =
                Assert.Throws<AddressMatcherServiceException>(
                    checkForBestMatchAction);

            // then
            actualAddressMatcherServiceException.Should()
                .BeEquivalentTo(expectedAddressMatcherServiceException);
        }
    }
}

