// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
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
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveOrAddIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            ResolvedAddress someResolvedAddress = CreateRandomResolvedAddress();
            ResolvedAddress inputResolvedAddress = someResolvedAddress;

            var expectedResolvedAddressProcessingDependencyValidationException =
                new ResolvedAddressProcessingDependencyValidationException(
                    message: "Resolved address processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.resolvedAddressServiceMock.Setup(service =>
                service.RetrieveResolvedAddressByIdAsync(inputResolvedAddress.Id))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<ResolvedAddress> resolvedAddressRetrieveOrAddTask =
                this.resolvedAddressProcessingService.RetrieveOrAddResolvedAddressAsync(inputResolvedAddress);

            ResolvedAddressProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<ResolvedAddressProcessingDependencyValidationException>(
                    resolvedAddressRetrieveOrAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressProcessingDependencyValidationException);

            this.resolvedAddressServiceMock.Verify(service =>
                service.RetrieveResolvedAddressByIdAsync(inputResolvedAddress.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedResolvedAddressProcessingDependencyValidationException))),
                         Times.Once);

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRetrieveOrAddIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            ResolvedAddress someResolvedAddress = CreateRandomResolvedAddress();
            ResolvedAddress inputResolvedAddress = someResolvedAddress;

            var expectedResolvedAddressProcessingDependencyException =
                new ResolvedAddressProcessingDependencyException(
                    message: "Resolved address processing dependency error occurred, please contact support.",
                    innerException: dependencyException.InnerException as Xeption);

            this.resolvedAddressServiceMock.Setup(service =>
                service.RetrieveResolvedAddressByIdAsync(inputResolvedAddress.Id))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<ResolvedAddress> resolvedAddressRetrieveOrAddTask =
                this.resolvedAddressProcessingService.RetrieveOrAddResolvedAddressAsync(inputResolvedAddress);

            ResolvedAddressProcessingDependencyException actualException =
                await Assert.ThrowsAsync<ResolvedAddressProcessingDependencyException>(
                    resolvedAddressRetrieveOrAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressProcessingDependencyException);

            this.resolvedAddressServiceMock.Verify(service =>
                service.RetrieveResolvedAddressByIdAsync(inputResolvedAddress.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedResolvedAddressProcessingDependencyException))),
                         Times.Once);

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveOrAddIfServiceErrorOccursAsync()
        {
            // given
            ResolvedAddress someResolvedAddress = CreateRandomResolvedAddress();
            ResolvedAddress inputResolvedAddress = someResolvedAddress;

            var serviceException = new Exception();

            var failedResolvedAddressProcessingServiceException =
                new FailedResolvedAddressProcessingServiceException(
                    message: "Failed resolved address processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedResolvedAddressProcessingServiveException =
                new ResolvedAddressProcessingServiceException(
                    message: "Resolved address processing service error occurred, please contact support.",
                    innerException: failedResolvedAddressProcessingServiceException);

            this.resolvedAddressServiceMock.Setup(service =>
                service.RetrieveResolvedAddressByIdAsync(inputResolvedAddress.Id))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<ResolvedAddress> addResolvedAddressTask =
                this.resolvedAddressProcessingService.RetrieveOrAddResolvedAddressAsync(inputResolvedAddress);

            ResolvedAddressProcessingServiceException actualException =
                await Assert.ThrowsAsync<ResolvedAddressProcessingServiceException>(addResolvedAddressTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressProcessingServiveException);

            this.resolvedAddressServiceMock.Verify(service =>
                service.RetrieveResolvedAddressByIdAsync(inputResolvedAddress.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedResolvedAddressProcessingServiveException))),
                         Times.Once);

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}