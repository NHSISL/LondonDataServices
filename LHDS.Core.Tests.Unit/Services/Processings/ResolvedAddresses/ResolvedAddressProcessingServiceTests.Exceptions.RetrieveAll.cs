// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
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
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveAllIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var expectedResolvedAddressProcessingDependencyValidationException =
                new ResolvedAddressProcessingDependencyValidationException(
                    message: "Resolved address processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            resolvedAddressServiceMock.Setup(service =>
                service.RetrieveAllResolvedAddressesAsync())
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<IQueryable<ResolvedAddress>> resolvedAddressRetrieveTask =
                resolvedAddressProcessingService.RetrieveAllResolvedAddressesAsync();

            ResolvedAddressProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<ResolvedAddressProcessingDependencyValidationException>(
                    testCode: resolvedAddressRetrieveTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressProcessingDependencyValidationException);

            resolvedAddressServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddressesAsync(),
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
        public async Task ShouldThrowDependencyExceptionOnRetrieveAllIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var expectedResolvedAddressProcessingDependencyException =
                new ResolvedAddressProcessingDependencyException(
                    message: "Resolved address processing dependency error occurred, please contact support.",
                    innerException: dependencyException.InnerException as Xeption);

            resolvedAddressServiceMock.Setup(service =>
                service.RetrieveAllResolvedAddressesAsync())
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<IQueryable<ResolvedAddress>> resolvedAddressRetrieveTask =
                resolvedAddressProcessingService.RetrieveAllResolvedAddressesAsync();

            ResolvedAddressProcessingDependencyException actualException =
                await Assert.ThrowsAsync<ResolvedAddressProcessingDependencyException>(
                    testCode: resolvedAddressRetrieveTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressProcessingDependencyException);

            resolvedAddressServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddressesAsync(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedResolvedAddressProcessingDependencyException))),
                         Times.Once);

            resolvedAddressServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAsync()
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
                service.RetrieveAllResolvedAddressesAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<IQueryable<ResolvedAddress>> resolvedAddressRetrieveTask =
                resolvedAddressProcessingService.RetrieveAllResolvedAddressesAsync();

            ResolvedAddressProcessingServiceException actualException =
                await Assert.ThrowsAsync<ResolvedAddressProcessingServiceException>(
                    testCode: resolvedAddressRetrieveTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressProcessingServiveException);

            resolvedAddressServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddressesAsync(),
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