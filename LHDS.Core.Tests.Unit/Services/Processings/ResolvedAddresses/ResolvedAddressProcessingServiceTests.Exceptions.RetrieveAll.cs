// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using FluentAssertions;
using LHDS.Core.Models.Processings.ResolvedAddresses.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.ResolvedAddresses
{
    public partial class ResolvedAddressProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public void ShouldThrowDependencyValidationExceptionOnRetrieveAllIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var expectedResolvedAddressProcessingDependencyValidationException =
                new ResolvedAddressProcessingDependencyValidationException(
                    message: "Resolved address processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            resolvedAddressServiceMock.Setup(service =>
                service.RetrieveAllResolvedAddresses())
                    .Throws(dependencyValidationException);

            // when
            Action resolvedAddressRetrieveAction = () =>
                resolvedAddressProcessingService.RetrieveAllResolvedAddresses();

            ResolvedAddressProcessingDependencyValidationException actualException =
                Assert.Throws<ResolvedAddressProcessingDependencyValidationException>(resolvedAddressRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressProcessingDependencyValidationException);

            resolvedAddressServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddresses(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedResolvedAddressProcessingDependencyValidationException))),
                         Times.Once);

            resolvedAddressServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public void ShouldThrowDependencyExceptionOnRetrieveAllIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var expectedResolvedAddressProcessingDependencyException =
                new ResolvedAddressProcessingDependencyException(
                    message: "Resolved address processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            resolvedAddressServiceMock.Setup(service =>
                service.RetrieveAllResolvedAddresses())
                    .Throws(dependencyException);

            // when
            Action resolvedAddressRetrieveAction = () =>
                resolvedAddressProcessingService.RetrieveAllResolvedAddresses();

            ResolvedAddressProcessingDependencyException actualException =
                Assert.Throws<ResolvedAddressProcessingDependencyException>(resolvedAddressRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressProcessingDependencyException);

            resolvedAddressServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddresses(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedResolvedAddressProcessingDependencyException))),
                         Times.Once);

            resolvedAddressServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAsync()
        {
            // given
            var serviceException = new Exception();

            var failedResolvedAddressProcessingServiceException =
                new FailedResolvedAddressProcessingServiceException(
                    message: "Failed resolved address processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedResolvedAddressProcessingServiveException =
                new ResolvedAddressProcessingServiceException(
                    message: "Resolved address processing service error occurred, please contact support.",
                    innerException: failedResolvedAddressProcessingServiceException);

            resolvedAddressServiceMock.Setup(service =>
                service.RetrieveAllResolvedAddresses())
                    .Throws(serviceException);

            // when
            Action resolvedAddressRetrieveAction = () =>
                resolvedAddressProcessingService.RetrieveAllResolvedAddresses();

            ResolvedAddressProcessingServiceException actualException =
                Assert.Throws<ResolvedAddressProcessingServiceException>(resolvedAddressRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressProcessingServiveException);

            resolvedAddressServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddresses(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedResolvedAddressProcessingServiveException))),
                         Times.Once);

            resolvedAddressServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}