// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Models.Processings.AddressNormalisations.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.AddressNormalisations
{
    public partial class AddressNormalisationProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task
            ShouldThrowDependencyValidationExceptionOnGetNormalisedAddressIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var randomAddress = GetRandomString();
            string inputAddress = randomAddress;

            var expectedAddressNormalisationProcessingDependencyValidationException =
                new AddressNormalisationProcessingDependencyValidationException(
                    message: "Address normalisation processing dependency validation occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.addressNormalisationServiceMock.Setup(service =>
                service.GetNormalisedAddress(inputAddress))
                    .Throws(dependencyValidationException);

            // when
            ValueTask<AddressNormalisation> getNormalisedAddressTask =
                this.addressNormalisationProcessingService.GetNormalisedAddress(inputAddress);

            AddressNormalisationProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressNormalisationProcessingDependencyValidationException>(
                    getNormalisedAddressTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(
                expectedAddressNormalisationProcessingDependencyValidationException);

            this.addressNormalisationServiceMock.Verify(service =>
                service.GetNormalisedAddress(inputAddress),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedAddressNormalisationProcessingDependencyValidationException))),
                         Times.Once);

            this.addressNormalisationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyOnGetNormalisedAddressIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var randomAddress = GetRandomString();
            string inputAddress = randomAddress;
            var randomMessage = GetRandomString();

            var expectedAddressNormalisationProcessingDependencyException =
                new AddressNormalisationProcessingDependencyException(
                    message: "Address normalisation processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.addressNormalisationServiceMock.Setup(service =>
                service.GetNormalisedAddress(inputAddress))
                    .Throws(dependencyException);

            // when
            ValueTask<AddressNormalisation> addressNormalisationAddTask =
                this.addressNormalisationProcessingService.GetNormalisedAddress(inputAddress);

            AddressNormalisationProcessingDependencyException actualException =
                await Assert.ThrowsAsync<AddressNormalisationProcessingDependencyException>(
                    addressNormalisationAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressNormalisationProcessingDependencyException);

            this.addressNormalisationServiceMock.Verify(service =>
                service.GetNormalisedAddress(inputAddress),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedAddressNormalisationProcessingDependencyException))),
                         Times.Once);

            this.addressNormalisationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetNormalisedAddressIfServiceErrorOccursAsync()
        {
            // given
            var randomAddress = GetRandomString();
            string inputAddress = randomAddress;
            var serviceException = new Exception();

            var failedAddressNormalisationProcessingServiceException =
                new FailedAddressNormalisationProcessingServiceException(
                    message: "Failed address normalisation processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAddressNormalisationProcessingServiveException =
                new AddressNormalisationProcessingServiceException(
                    message: "Address normalisation processing service error occurred, please contact support.",
                    innerException: failedAddressNormalisationProcessingServiceException);

            this.addressNormalisationServiceMock.Setup(service =>
                service.GetNormalisedAddress(inputAddress))
                    .Throws(serviceException);

            // when
            ValueTask<AddressNormalisation> addAddressNormalisationTask =
                this.addressNormalisationProcessingService.GetNormalisedAddress(inputAddress);

            AddressNormalisationProcessingServiceException actualException =
                await Assert.ThrowsAsync<AddressNormalisationProcessingServiceException>(
                    addAddressNormalisationTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressNormalisationProcessingServiveException);

            this.addressNormalisationServiceMock.Verify(service =>
                service.GetNormalisedAddress(inputAddress),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedAddressNormalisationProcessingServiveException))),
                         Times.Once);

            this.addressNormalisationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
