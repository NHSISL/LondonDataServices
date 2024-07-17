// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
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
        public async Task ShouldThrowDependencyValidationExceptionOnBulkAddIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            string someFileName = GetRandomString();
            List<ResolvedAddress> someResolvedAddresses = new List<ResolvedAddress> { CreateRandomResolvedAddress() };
            List<ResolvedAddress> inputResolvedAddresses = someResolvedAddresses;

            var expectedResolvedAddressProcessingDependencyValidationException =
                new ResolvedAddressProcessingDependencyValidationException(
                    message: "Resolved address processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.resolvedAddressServiceMock.Setup(service =>
                service.BulkAddResolvedAddressesAsync(inputResolvedAddresses, someFileName))
                    .Throws(dependencyValidationException);

            // when
            ValueTask bulkAddTask = this.resolvedAddressProcessingService
                .BulkAddResolvedAddressesAsync(resolvedAddresses: inputResolvedAddresses, fileName: someFileName);

            ResolvedAddressProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<ResolvedAddressProcessingDependencyValidationException>(
                    bulkAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressProcessingDependencyValidationException);

            this.resolvedAddressServiceMock.Verify(service =>
                service.BulkAddResolvedAddressesAsync(inputResolvedAddresses, someFileName),
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
        public async Task ShouldThrowDependencyExceptionOnBulkAddIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            string someFileName = GetRandomString();
            List<ResolvedAddress> someResolvedAddresses = new List<ResolvedAddress> { CreateRandomResolvedAddress() };
            List<ResolvedAddress> inputResolvedAddresses = someResolvedAddresses;

            var expectedResolvedAddressProcessingDependencyException =
                new ResolvedAddressProcessingDependencyException(
                    message: "Resolved address processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.resolvedAddressServiceMock.Setup(service =>
                service.BulkAddResolvedAddressesAsync(inputResolvedAddresses, someFileName))
                    .Throws(dependencyException);

            // when
            ValueTask bulkAddTask = this.resolvedAddressProcessingService
                .BulkAddResolvedAddressesAsync(resolvedAddresses: inputResolvedAddresses, fileName: someFileName);

            ResolvedAddressProcessingDependencyException actualException =
                await Assert.ThrowsAsync<ResolvedAddressProcessingDependencyException>(bulkAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressProcessingDependencyException);

            this.resolvedAddressServiceMock.Verify(service =>
                service.BulkAddResolvedAddressesAsync(inputResolvedAddresses, someFileName),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedResolvedAddressProcessingDependencyException))),
                         Times.Once);

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnBulkAddIfServiceErrorOccursAsync()
        {
            // given
            string someFileName = GetRandomString();
            List<ResolvedAddress> someResolvedAddresses = new List<ResolvedAddress> { CreateRandomResolvedAddress() };
            List<ResolvedAddress> inputResolvedAddresses = someResolvedAddresses;

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
                service.BulkAddResolvedAddressesAsync(inputResolvedAddresses, someFileName))
                    .Throws(serviceException);

            // when
            ValueTask bulkAddTask = this.resolvedAddressProcessingService
                .BulkAddResolvedAddressesAsync(resolvedAddresses: inputResolvedAddresses, fileName: someFileName);

            ResolvedAddressProcessingServiceException actualException =
                await Assert.ThrowsAsync<ResolvedAddressProcessingServiceException>(bulkAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressProcessingServiveException);

            this.resolvedAddressServiceMock.Verify(service =>
                service.BulkAddResolvedAddressesAsync(inputResolvedAddresses, someFileName),
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