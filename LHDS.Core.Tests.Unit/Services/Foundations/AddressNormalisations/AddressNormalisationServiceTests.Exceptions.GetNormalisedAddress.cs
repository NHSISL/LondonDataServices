// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Clients.LibPostalClient.Exceptions;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Models.Foundations.AddressNormalisations.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressNormalisations
{
    public partial class AddressNormalisationServiceTests
    {
        [Theory]
        [MemberData(nameof(AddressNormalisationDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnAddressNormalisationIfDependencyValidationOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            string randomAddress = GetRandomString();

            var invalidLibPostalValidationException =
                    new InvalidLibPostalValidationException(
                        message: "Invalid lib poastal validation error occurred.",
                        innerException: dependencyValidationException,
                        data: dependencyValidationException.Data);

            var expectedDependencyException =
                new AddressNormalisationDependencyValidationException(
                    message: "Address normalisation dependency validation error occurred, " +
                        "fix the errors and try again.",
                    innerException: invalidLibPostalValidationException.InnerException as Xeption);

            this.addressNormalisationBrokerMock.Setup(broker =>
                broker.ExpandAddressAsync(randomAddress))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<AddressNormalisation> addAddressNormalisationTask =
               this.addressNormalisationService.GetNormalisedAddress(randomAddress);

            AddressNormalisationDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressNormalisationDependencyValidationException>(
                    addAddressNormalisationTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.addressNormalisationBrokerMock.Verify(broker =>
                 broker.ExpandAddressAsync(It.IsAny<string>()),
                     Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                        Times.Once);

            this.addressNormalisationBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(AddressNormalisationDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnAddressNormalisationIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            string randomAddress = GetRandomString();

            var invalidLibPostalDependencyException =
                    new InvalidLibPostalDependencyException(
                        message: "Invalid lib poastal dependency error occurred.",
                        innerException: dependencyException);

            var expectedDependencyException =
                new AddressNormalisationDependencyException(
                    message: "Address normalisation dependency error occurred, " +
                        "fix the errors and try again.",
                    innerException: invalidLibPostalDependencyException.InnerException as Xeption);

            this.addressNormalisationBrokerMock.Setup(broker =>
                broker.ExpandAddressAsync(randomAddress))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<AddressNormalisation> addAddressNormalisationTask =
               this.addressNormalisationService.GetNormalisedAddress(randomAddress);

            AddressNormalisationDependencyException actualException =
                await Assert.ThrowsAsync<AddressNormalisationDependencyException>(
                    addAddressNormalisationTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.addressNormalisationBrokerMock.Verify(broker =>
                 broker.ExpandAddressAsync(It.IsAny<string>()),
                     Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                        Times.Once);

            this.addressNormalisationBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();
            string randomAddress = GetRandomString();

            var failedAddressNormalisationServiceException =
                new FailedAddressNormalisationServiceException(
                    message: "Failed address normalisation service occurred, please contact support",
                    innerException: serviceException);

            var expectedAddressNormalisationServiceException =
                new AddressNormalisationServiceException(
                    message: "Address normalisation service error occurred, contact support.",
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