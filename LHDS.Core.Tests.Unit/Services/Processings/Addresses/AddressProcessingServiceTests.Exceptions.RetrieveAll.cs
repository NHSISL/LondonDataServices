// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using FluentAssertions;
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
        public void ShouldThrowDependencyValidationExceptionOnRetrieveAllIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var expectedAddressProcessingDependencyValidationException =
                new AddressProcessingDependencyValidationException(
                    message: "Address processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            addressServiceMock.Setup(service =>
                service.RetrieveAllAddresses())
                    .Throws(dependencyValidationException);

            // when
            Action addressRetrieveAction = () =>
                addressProcessingService.RetrieveAllAddresses();

            AddressProcessingDependencyValidationException actualException =
                Assert.Throws<AddressProcessingDependencyValidationException>(addressRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressProcessingDependencyValidationException);

            addressServiceMock.Verify(service =>
                service.RetrieveAllAddresses(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedAddressProcessingDependencyValidationException))),
                         Times.Once);

            addressServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public void ShouldThrowDependencyExceptionOnRetrieveAllIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var expectedAddressProcessingDependencyException =
                new AddressProcessingDependencyException(
                    message: "Address processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            addressServiceMock.Setup(service =>
                service.RetrieveAllAddresses())
                    .Throws(dependencyException);

            // when
            Action addressRetrieveAction = () =>
                addressProcessingService.RetrieveAllAddresses();

            AddressProcessingDependencyException actualException =
                Assert.Throws<AddressProcessingDependencyException>(addressRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressProcessingDependencyException);

            addressServiceMock.Verify(service =>
                service.RetrieveAllAddresses(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedAddressProcessingDependencyException))),
                         Times.Once);

            addressServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAsync()
        {
            // given
            var serviceException = new Exception();

            var failedAddressProcessingServiceException =
                new FailedAddressProcessingServiceException(
                    message: "Failed Address processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAddressProcessingServiveException =
                new AddressProcessingServiceException(
                    message: "Address processing service error occurred, please contact support.",
                    innerException: failedAddressProcessingServiceException);

            addressServiceMock.Setup(service =>
                service.RetrieveAllAddresses())
                    .Throws(serviceException);

            // when
            Action addressRetrieveAction = () =>
                addressProcessingService.RetrieveAllAddresses();

            AddressProcessingServiceException actualException =
                Assert.Throws<AddressProcessingServiceException>(addressRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressProcessingServiveException);

            addressServiceMock.Verify(service =>
                service.RetrieveAllAddresses(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedAddressProcessingServiveException))),
                         Times.Once);

            addressServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
