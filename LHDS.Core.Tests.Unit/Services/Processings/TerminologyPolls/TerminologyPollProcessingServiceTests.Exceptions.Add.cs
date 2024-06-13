// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Processings.TerminologyPolls.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.TerminologyPolls
{
    public partial class TerminologyPollProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            TerminologyPoll someTerminologyPoll = CreateRandomTerminologyPoll();
            TerminologyPoll inputTerminologyPoll = someTerminologyPoll.DeepClone();

            var expectedTerminologyPollProcessingDependencyValidationException =
                new TerminologyPollProcessingDependencyValidationException(
                    message: "Terminology poll processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.terminologyPollServiceMock.Setup(service =>
                service.AddTerminologyPollAsync(inputTerminologyPoll))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<TerminologyPoll> terminologyAddTask =
                this.terminologyPollProcessingService.AddTerminologyPollAsync(inputTerminologyPoll);

            TerminologyPollProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<TerminologyPollProcessingDependencyValidationException>(
                    terminologyAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTerminologyPollProcessingDependencyValidationException);

            this.terminologyPollServiceMock.Verify(service =>
                service.AddTerminologyPollAsync(inputTerminologyPoll),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedTerminologyPollProcessingDependencyValidationException))),
                         Times.Once);

            this.terminologyPollServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnAddIfErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            TerminologyPoll someTerminologyPoll = CreateRandomTerminologyPoll();
            TerminologyPoll inputTerminologyPoll = someTerminologyPoll.DeepClone();

            var expectedTerminologyPollProcessingDependencyException =
                new TerminologyPollProcessingDependencyException(
                    message: "Terminology poll processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.terminologyPollServiceMock.Setup(service =>
                service.AddTerminologyPollAsync(inputTerminologyPoll))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<TerminologyPoll> terminologyAddTask =
                this.terminologyPollProcessingService.AddTerminologyPollAsync(inputTerminologyPoll);

            TerminologyPollProcessingDependencyException actualException =
                await Assert.ThrowsAsync<TerminologyPollProcessingDependencyException>(
                    terminologyAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTerminologyPollProcessingDependencyException);

            this.terminologyPollServiceMock.Verify(service =>
                service.AddTerminologyPollAsync(inputTerminologyPoll),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedTerminologyPollProcessingDependencyException))),
                         Times.Once);

            this.terminologyPollServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAsync()
        {
            // given
            TerminologyPoll someTerminologyPoll = CreateRandomTerminologyPoll();
            TerminologyPoll inputTerminologyPoll = someTerminologyPoll.DeepClone();
            var serviceException = new Exception();

            var failedTerminologyPollProcessingServiceException =
                new FailedTerminologyPollProcessingServiceException(
                    message: "Failed terminology poll processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedTerminologyPollProcessingServiceException =
                new TerminologyPollProcessingServiceException(
                    message: "Terminology poll processing service error occurred, please contact support.",
                    innerException: failedTerminologyPollProcessingServiceException);

            this.terminologyPollServiceMock.Setup(service =>
                service.AddTerminologyPollAsync(inputTerminologyPoll))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<TerminologyPoll> terminologyAddTask =
                this.terminologyPollProcessingService.AddTerminologyPollAsync(inputTerminologyPoll);

            TerminologyPollProcessingServiceException actualException =
                await Assert.ThrowsAsync<TerminologyPollProcessingServiceException>(
                    terminologyAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTerminologyPollProcessingServiceException);

            this.terminologyPollServiceMock.Verify(service =>
                service.AddTerminologyPollAsync(inputTerminologyPoll),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedTerminologyPollProcessingServiceException))),
                         Times.Once);

            this.terminologyPollServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
