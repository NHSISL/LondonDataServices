// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Models.Foundations.AddressNormalisations.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressNormalisations
{
    public partial class AddressNormalisationServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();
            string randomAddress = GetRandomString();

            var failedAddressNormalisationServiceException =
                new FailedAddressNormalisationServiceException(
                    message: "Failed address normalisation service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAddressNormalisationServiceException =
                new AddressNormalisationServiceException(
                    message: "Address normalisation service error occurred, please contact support.",
                    innerException: failedAddressNormalisationServiceException);

            this.addressNormalisationBrokerMock.Setup(broker =>
                broker.ExpandAddressAsync(randomAddress))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<AddressNormalisation> addAddressNormalisationTask =
                this.addressNormalisationService.GetNormalisedAddress(randomAddress);

            AddressNormalisationServiceException actualAddressNormalisationServiceException =
                await Assert.ThrowsAsync<AddressNormalisationServiceException>(
                    addAddressNormalisationTask.AsTask);

            // then
            actualAddressNormalisationServiceException.Should()
                .BeEquivalentTo(expectedAddressNormalisationServiceException);

            this.addressNormalisationBrokerMock.Verify(broker =>
                broker.ExpandAddressAsync(It.IsAny<string>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressNormalisationServiceException))),
                        Times.Once);

            this.addressNormalisationBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}