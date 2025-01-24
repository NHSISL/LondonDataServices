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
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveByIdIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            Guid someId = Guid.NewGuid();

            var expectedResolvedAddressProcessingDependencyValidationException =
                new ResolvedAddressProcessingDependencyValidationException(
                    message: "Resolved address processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.resolvedAddressServiceMock.Setup(service =>
                service.RetrieveResolvedAddressByIdAsync(someId))
                    .Throws(dependencyValidationException);

            // when
            ValueTask<ResolvedAddress> resolvedAddressRetrieveByIdTask =
                this.resolvedAddressProcessingService.RetrieveResolvedAddressByIdAsync(someId);

            ResolvedAddressProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<ResolvedAddressProcessingDependencyValidationException>(
                    resolvedAddressRetrieveByIdTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressProcessingDependencyValidationException);

            this.resolvedAddressServiceMock.Verify(service =>
                service.RetrieveResolvedAddressByIdAsync(someId),
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
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            Guid someId = Guid.NewGuid();

            var expectedResolvedAddressProcessingDependencyException =
                new ResolvedAddressProcessingDependencyException(
                    message: "Resolved address processing dependency error occurred, please contact support.",
                    innerException: dependencyException.InnerException as Xeption);

            this.resolvedAddressServiceMock.Setup(service =>
                service.RetrieveResolvedAddressByIdAsync(someId))
                    .Throws(dependencyException);

            // when
            ValueTask<ResolvedAddress> resolvedAddressRetrieveByIdTask =
                this.resolvedAddressProcessingService.RetrieveResolvedAddressByIdAsync(someId);

            ResolvedAddressProcessingDependencyException actualException =
                await Assert.ThrowsAsync<ResolvedAddressProcessingDependencyException>(resolvedAddressRetrieveByIdTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressProcessingDependencyException);

            this.resolvedAddressServiceMock.Verify(service =>
                service.RetrieveResolvedAddressByIdAsync(someId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedResolvedAddressProcessingDependencyException))),
                         Times.Once);

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAsync()
        {
            // given
            Guid someId = Guid.NewGuid();

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
                service.RetrieveResolvedAddressByIdAsync(someId))
                    .Throws(serviceException);

            // when
            ValueTask<ResolvedAddress> addResolvedAddressTask =
                this.resolvedAddressProcessingService.RetrieveResolvedAddressByIdAsync(someId);

            ResolvedAddressProcessingServiceException actualException =
                await Assert.ThrowsAsync<ResolvedAddressProcessingServiceException>(addResolvedAddressTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressProcessingServiveException);

            this.resolvedAddressServiceMock.Verify(service =>
                service.RetrieveResolvedAddressByIdAsync(someId),
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
