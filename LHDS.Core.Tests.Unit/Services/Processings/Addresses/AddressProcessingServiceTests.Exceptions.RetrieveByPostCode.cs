// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Processings.Addresses.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Addresses
{
    public partial class AddressProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveByPostCodeIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            string somePostCode = GetRandomString();

            var expectedAddressProcessingDependencyValidationException =
                new AddressProcessingDependencyValidationException(
                    message: "Address processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.addressServiceMock.Setup(service =>
                service.RetrieveAddressesByPostCodeAsync(somePostCode))
                    .Throws(dependencyValidationException);

            // when
            ValueTask<List<Address>> addressRetrieveByPostCodeTask =
                this.addressProcessingService.RetrieveAddressesByPostCodeAsync(somePostCode);

            AddressProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressProcessingDependencyValidationException>(
                    addressRetrieveByPostCodeTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressProcessingDependencyValidationException);

            this.addressServiceMock.Verify(service =>
                service.RetrieveAddressesByPostCodeAsync(somePostCode),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedAddressProcessingDependencyValidationException))),
                         Times.Once);

            this.addressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByPostCodeIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            string somePostCode = GetRandomString();

            var expectedAddressProcessingDependencyException =
                new AddressProcessingDependencyException(
                    message: "Address processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.addressServiceMock.Setup(service =>
                service.RetrieveAddressesByPostCodeAsync(somePostCode))
                    .Throws(dependencyException);

            // when
            ValueTask<List<Address>> addressRetrieveByPostCodeTask =
                this.addressProcessingService.RetrieveAddressesByPostCodeAsync(somePostCode);

            AddressProcessingDependencyException actualException =
                await Assert.ThrowsAsync<AddressProcessingDependencyException>(addressRetrieveByPostCodeTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressProcessingDependencyException);

            this.addressServiceMock.Verify(service =>
                service.RetrieveAddressesByPostCodeAsync(somePostCode),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedAddressProcessingDependencyException))),
                         Times.Once);

            this.addressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByPostCodeIfServiceErrorOccursAsync()
        {
            // given
            string somePostCode = GetRandomString();
            var serviceException = new Exception();

            var failedAddressProcessingServiceException =
                new FailedAddressProcessingServiceException(
                    message: "Failed Address processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAddressProcessingServiveException =
                new AddressProcessingServiceException(
                    message: "Address processing service error occurred, please contact support.",
                    innerException: failedAddressProcessingServiceException);

            this.addressServiceMock.Setup(service =>
                service.RetrieveAddressesByPostCodeAsync(somePostCode))
                    .Throws(serviceException);

            // when
            ValueTask<List<Address>> addressRetrieveByPostCodeTask =
                this.addressProcessingService.RetrieveAddressesByPostCodeAsync(somePostCode);

            AddressProcessingServiceException actualException =
                await Assert.ThrowsAsync<AddressProcessingServiceException>(addressRetrieveByPostCodeTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressProcessingServiveException);

            this.addressServiceMock.Verify(service =>
                service.RetrieveAddressesByPostCodeAsync(somePostCode),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedAddressProcessingServiveException))),
                         Times.Once);

            this.addressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
