// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowDependencyValidationExceptionOnIsMatchIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            string someAddress = GetRandomString();

            var expectedResolvedAddressProcessingDependencyValidationException =
                new ResolvedAddressProcessingDependencyValidationException(
                    message: "ResolvedAddress processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.resolvedAddressServiceMock.Setup(service =>
                service.RetrieveAllResolvedAddresses())
                    .Throws(dependencyValidationException);

            // when
            ValueTask<bool> isMatchTask =
                this.resolvedAddressProcessingService.IsExactMatchForResolvedAddressAsync(someAddress);

            ResolvedAddressProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<ResolvedAddressProcessingDependencyValidationException>(
                    isMatchTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressProcessingDependencyValidationException);

            this.resolvedAddressServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddresses(),
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
        public async Task ShouldThrowDependencyExceptionOnIsMatchIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            string someAddress = GetRandomString();

            var expectedResolvedAddressProcessingDependencyException =
                new ResolvedAddressProcessingDependencyException(
                    message: "ResolvedAddress processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.resolvedAddressServiceMock.Setup(service =>
                service.RetrieveAllResolvedAddresses())
                    .Throws(dependencyException);

            // when
            ValueTask<bool> isMatchTask =
                this.resolvedAddressProcessingService.IsExactMatchForResolvedAddressAsync(someAddress);

            ResolvedAddressProcessingDependencyException actualException =
                await Assert.ThrowsAsync<ResolvedAddressProcessingDependencyException>(isMatchTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressProcessingDependencyException);

            this.resolvedAddressServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddresses(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedResolvedAddressProcessingDependencyException))),
                         Times.Once);

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnIsMatchIfServiceErrorOccursAsync()
        {
            // given
            string someAddress = GetRandomString();

            var serviceException = new Exception();

            var failedResolvedAddressProcessingServiceException =
                new FailedResolvedAddressProcessingServiceException(
                    message: "Failed ResolvedAddress processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedResolvedAddressProcessingServiveException =
                new ResolvedAddressProcessingServiceException(
                    message: "ResolvedAddress processing service error occurred, please contact support.",
                    innerException: failedResolvedAddressProcessingServiceException);

            this.resolvedAddressServiceMock.Setup(service =>
                service.RetrieveAllResolvedAddresses())
                    .Throws(serviceException);

            // when
            ValueTask<bool> isMatchTask =
                this.resolvedAddressProcessingService.IsExactMatchForResolvedAddressAsync(someAddress);

            ResolvedAddressProcessingServiceException actualException =
                await Assert.ThrowsAsync<ResolvedAddressProcessingServiceException>(isMatchTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressProcessingServiveException);

            this.resolvedAddressServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddresses(),
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