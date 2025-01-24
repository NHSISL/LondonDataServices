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
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveOrModifyOrAddIfErrorOccursAndLogItAsync(
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
                service.RetrieveAllResolvedAddressesAsync())
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<ResolvedAddress> resolvedAddressModifyOrAddTask =
                this.resolvedAddressProcessingService.ModifyOrAddResolvedAddressAsync(inputResolvedAddress);

            ResolvedAddressProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<ResolvedAddressProcessingDependencyValidationException>(
                    resolvedAddressModifyOrAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressProcessingDependencyValidationException);

            this.resolvedAddressServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddressesAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedResolvedAddressProcessingDependencyValidationException))),
                         Times.Once);

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRetrieveOrModifyOrAddIfDependencyErrorOccursAndLogItAsync(
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
                service.RetrieveAllResolvedAddressesAsync())
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<ResolvedAddress> resolvedAddressModifyOrAddTask =
                this.resolvedAddressProcessingService.ModifyOrAddResolvedAddressAsync(inputResolvedAddress);

            ResolvedAddressProcessingDependencyException actualException =
                await Assert.ThrowsAsync<ResolvedAddressProcessingDependencyException>(
                    resolvedAddressModifyOrAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressProcessingDependencyException);

            this.resolvedAddressServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddressesAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedResolvedAddressProcessingDependencyException))),
                         Times.Once);

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveOrModifyOrAddIfServiceErrorOccursAsync()
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
                service.RetrieveAllResolvedAddressesAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<ResolvedAddress> addResolvedAddressTask =
                this.resolvedAddressProcessingService.ModifyOrAddResolvedAddressAsync(inputResolvedAddress);

            ResolvedAddressProcessingServiceException actualException =
                await Assert.ThrowsAsync<ResolvedAddressProcessingServiceException>(addResolvedAddressTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressProcessingServiveException);

            this.resolvedAddressServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddressesAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedResolvedAddressProcessingServiveException))),
                         Times.Once);

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}