// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.ResolvedAddresses.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.ResolvedAddresses
{
    public partial class ResolvedAddressOrchestrationTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task
           ShouldThrowDependencyValidationExceptionOnExportResolvedAddressIfDependencyValidationErrorOccursAndLogItAsync(
           Xeption dependencyValidationException)
        {
            // given
            var expectedResolvedAddressOrchestrationDependencyValidationException =
                new ResolvedAddressOrchestrationDependencyValidationException(
                    message: "Resolved address orchestration dependency validation errors occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.resolvedAddressProcessingServiceMock.Setup(service =>
                service.RetrieveAllResolvedAddresses())
                    .Throws(dependencyValidationException);

            // when
            ValueTask action = this.resolvedAddressOrchestrationService.MatchAddressDataAsync();

            ResolvedAddressOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<ResolvedAddressOrchestrationDependencyValidationException>(
                    action.AsTask);

            // then
            actualException.Should().
                BeEquivalentTo(expectedResolvedAddressOrchestrationDependencyValidationException);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddresses(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedResolvedAddressOrchestrationDependencyValidationException))),
                         Times.Once);

            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.assignProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnnExportResolvedAddressIfDependencyErrorOccursAndLogItAsync(
          Xeption dependencyException)
        {
            // given
            var expectedResolvedAddressOrchestrationDependencyException =
                new ResolvedAddressOrchestrationDependencyException(
                    message: "Resolved address orchestration dependency errors occurred, please contact support.",
                    innerException: dependencyException.InnerException as Xeption);

            this.resolvedAddressProcessingServiceMock.Setup(service =>
                service.RetrieveAllResolvedAddresses())
                    .Throws(dependencyException);

            // when
            ValueTask action = this.resolvedAddressOrchestrationService.MatchAddressDataAsync();

            ResolvedAddressOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<ResolvedAddressOrchestrationDependencyException>(
                    action.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressOrchestrationDependencyException);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddresses(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedResolvedAddressOrchestrationDependencyException))),
                         Times.Once);

            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.assignProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnExportResolvedAddressIfServiceErrorOccursAsync()
        {
            // given
            var serviceException = new Exception();

            var failedResolvedAddressOrchestrationServiceException =
                new FailedResolvedAddressOrchestrationServiceException(
                    message: "Failed resolved address orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedResolvedAddressOrchestrationServiveException =
                new ResolvedAddressOrchestrationServiceException(
                    message: "Resolved address orchestration service error occurred, please contact support.",
                    innerException: failedResolvedAddressOrchestrationServiceException);

            this.resolvedAddressProcessingServiceMock.Setup(service =>
                service.RetrieveAllResolvedAddresses())
                    .Throws(serviceException);

            // when
            ValueTask action = this.resolvedAddressOrchestrationService.MatchAddressDataAsync();

            ResolvedAddressOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<ResolvedAddressOrchestrationServiceException>(action.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressOrchestrationServiveException);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddresses(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedResolvedAddressOrchestrationServiveException))),
                         Times.Once);

            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.assignProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }


    }
}
