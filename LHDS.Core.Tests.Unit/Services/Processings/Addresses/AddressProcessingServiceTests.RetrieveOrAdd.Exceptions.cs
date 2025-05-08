// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveOrAddIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            Address someAddress = CreateRandomAddress();
            Address inputAddress = someAddress;

            var expectedAddressProcessingDependencyValidationException =
                new AddressProcessingDependencyValidationException(
                    message: "Address processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.addressServiceMock.Setup(service =>
                service.RetrieveAddressByIdAsync(inputAddress.Id))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<Address> addressRetrieveOrAddTask =
                this.addressProcessingService.RetrieveOrAddAddressAsync(inputAddress);

            AddressProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressProcessingDependencyValidationException>(
                    addressRetrieveOrAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressProcessingDependencyValidationException);

            this.addressServiceMock.Verify(service =>
                service.RetrieveAddressByIdAsync(inputAddress.Id),
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
        public async Task ShouldThrowDependencyExceptionOnRetrieveOrAddIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            Address someAddress = CreateRandomAddress();
            Address inputAddress = someAddress;

            var expectedAddressProcessingDependencyException =
                new AddressProcessingDependencyException(
                    message: "Address processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.addressServiceMock.Setup(service =>
                service.RetrieveAddressByIdAsync(inputAddress.Id))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<Address> addressRetrieveOrAddTask =
                this.addressProcessingService.RetrieveOrAddAddressAsync(inputAddress);

            AddressProcessingDependencyException actualException =
                await Assert.ThrowsAsync<AddressProcessingDependencyException>(
                    addressRetrieveOrAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressProcessingDependencyException);

            this.addressServiceMock.Verify(service =>
                service.RetrieveAddressByIdAsync(inputAddress.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedAddressProcessingDependencyException))),
                         Times.Once);

            this.addressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveOrAddIfServiceErrorOccursAsync()
        {
            // given
            Address someAddress = CreateRandomAddress();
            Address inputAddress = someAddress;

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
                service.RetrieveAddressByIdAsync(inputAddress.Id))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Address> addAddressTask =
                this.addressProcessingService.RetrieveOrAddAddressAsync(inputAddress);

            AddressProcessingServiceException actualException =
                await Assert.ThrowsAsync<AddressProcessingServiceException>(addAddressTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressProcessingServiveException);

            this.addressServiceMock.Verify(service =>
                service.RetrieveAddressByIdAsync(inputAddress.Id),
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
