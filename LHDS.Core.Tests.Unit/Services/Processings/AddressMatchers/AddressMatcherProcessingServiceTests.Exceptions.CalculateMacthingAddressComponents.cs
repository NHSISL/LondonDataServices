// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressMatchers;
using LHDS.Core.Models.Processings.AddressMatchers.Exceptions;
using LHDS.Core.Services.Processings.AddressMatchers;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.AddressMatchers
{
    public partial class AddressMatcherProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnCalculateMacthesIfServiceErrorOccursAsync()
        {
            // given
            List<KeyValuePair<string, string>> someIncomingAddressComponents =
                new List<KeyValuePair<string, string>>();

            HashSet<AddressMatch> somePossibleAddresses = new HashSet<AddressMatch>();

            var mock = new Mock<AddressMatcherProcessingService>(loggingBrokerMock.Object) { CallBase = true };
            var serviceException = new Exception();

            mock.Setup(x => x.ValidateCalculateArguments(
                It.IsAny<IList<KeyValuePair<string, string>>>(),
                It.IsAny<HashSet<AddressMatch>>()))
                    .Throws(serviceException);

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
            ValueTask<HashSet<AddressMatch>> calculateMacthingAddressComponentsTask =
                addressMatcherProcessingService.CalculateMacthingAddressComponents(
                    incomingAddressComponents: someIncomingAddressComponents,
                    possibleAddresses: somePossibleAddresses);

            AddressMatcherProcessingServiceException actualAddressMatcherProcessingServiceException =
                await Assert.ThrowsAsync<AddressMatcherProcessingServiceException>(
                    calculateMacthingAddressComponentsTask.AsTask);

            // then
            actualAddressMatcherProcessingServiceException.Should()
                .BeEquivalentTo(expectedAddressMatcherProcessingServiceException);
        }
    }
}