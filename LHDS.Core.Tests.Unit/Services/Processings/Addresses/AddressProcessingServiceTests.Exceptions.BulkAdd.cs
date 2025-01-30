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
        public async Task ShouldThrowDependencyValidationExceptionOnBulkAddIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            string someFileName = GetRandomString();
            List<Address> someAddresses = new List<Address> { CreateRandomAddress() };
            List<Address> inputAddresses = someAddresses;

            var expectedAddressProcessingDependencyValidationException =
                new AddressProcessingDependencyValidationException(
                    message: "Address processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.addressServiceMock.Setup(service =>
                service.BulkAddAddressesAsync(inputAddresses, someFileName))
                    .Throws(dependencyValidationException);

            // when
            ValueTask addressAddTask = this.addressProcessingService
                .BulkAddAddressesAsync(addresses: inputAddresses, fileName: someFileName);

            AddressProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressProcessingDependencyValidationException>(
                    addressAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressProcessingDependencyValidationException);

            this.addressServiceMock.Verify(service =>
                service.BulkAddAddressesAsync(inputAddresses, someFileName),
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
        public async Task ShouldThrowDependencyExceptionOnBulkAddIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            string someFileName = GetRandomString();
            List<Address> someAddresses = new List<Address> { CreateRandomAddress() };
            List<Address> inputAddresses = someAddresses;

            var expectedAddressProcessingDependencyException =
                new AddressProcessingDependencyException(
                    message: "Address processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.addressServiceMock.Setup(service =>
                service.BulkAddAddressesAsync(inputAddresses, someFileName))
                    .Throws(dependencyException);

            // when
            ValueTask addressAddTask = this.addressProcessingService
                .BulkAddAddressesAsync(addresses: inputAddresses, fileName: someFileName);

            AddressProcessingDependencyException actualException =
                await Assert.ThrowsAsync<AddressProcessingDependencyException>(addressAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressProcessingDependencyException);

            this.addressServiceMock.Verify(service =>
                service.BulkAddAddressesAsync(inputAddresses, someFileName),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedAddressProcessingDependencyException))),
                         Times.Once);

            this.addressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnBulkAddIfServiceErrorOccursAsync()
        {
            // given
            string someFileName = GetRandomString();
            List<Address> someAddresses = new List<Address> { CreateRandomAddress() };
            List<Address> inputAddresses = someAddresses;

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
                service.BulkAddAddressesAsync(inputAddresses, someFileName))
                    .Throws(serviceException);

            // when
            ValueTask addressAddTask = this.addressProcessingService
                .BulkAddAddressesAsync(addresses: inputAddresses, fileName: someFileName);

            AddressProcessingServiceException actualException =
                await Assert.ThrowsAsync<AddressProcessingServiceException>(addressAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressProcessingServiveException);

            this.addressServiceMock.Verify(service =>
                service.BulkAddAddressesAsync(inputAddresses, someFileName),
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
