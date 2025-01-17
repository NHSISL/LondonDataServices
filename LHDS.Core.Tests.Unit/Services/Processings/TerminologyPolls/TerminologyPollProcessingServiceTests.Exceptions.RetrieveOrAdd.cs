// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
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
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveOrAddIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            string resouceType = GetRandomString();

            var expectedTerminologyPollProcessingDependencyValidationException =
                new TerminologyPollProcessingDependencyValidationException(
                    message: "Terminology poll processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.terminologyPollServiceMock.Setup(service =>
                service.RetrieveAllTerminologyPollsAsync())
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<TerminologyPoll> terminologyRetrieveOrAddTask =
                this.terminologyPollProcessingService.RetrieveOrAddTerminologyPollAsync(resouceType);

            TerminologyPollProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<TerminologyPollProcessingDependencyValidationException>(
                    terminologyRetrieveOrAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTerminologyPollProcessingDependencyValidationException);

            this.terminologyPollServiceMock.Verify(service =>
                service.RetrieveAllTerminologyPollsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedTerminologyPollProcessingDependencyValidationException))),
                         Times.Once);

            this.terminologyPollServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRetrieveOrAddIfErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            string resouceType = GetRandomString();

            var expectedTerminologyPollProcessingDependencyException =
                new TerminologyPollProcessingDependencyException(
                    message: "Terminology poll processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.terminologyPollServiceMock.Setup(service =>
                service.RetrieveAllTerminologyPollsAsync())
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<TerminologyPoll> terminologyRetrieveOrAddTask =
                this.terminologyPollProcessingService.RetrieveOrAddTerminologyPollAsync(resouceType);

            TerminologyPollProcessingDependencyException actualException =
                await Assert.ThrowsAsync<TerminologyPollProcessingDependencyException>(
                    terminologyRetrieveOrAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTerminologyPollProcessingDependencyException);

            this.terminologyPollServiceMock.Verify(service =>
                service.RetrieveAllTerminologyPollsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedTerminologyPollProcessingDependencyException))),
                         Times.Once);

            this.terminologyPollServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveOrAddIfServiceErrorOccursAsync()
        {
            // given
            string resouceType = GetRandomString();
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
                service.RetrieveAllTerminologyPollsAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<TerminologyPoll> terminologyRetrieveOrAddTask =
                this.terminologyPollProcessingService.RetrieveOrAddTerminologyPollAsync(resouceType);

            TerminologyPollProcessingServiceException actualException =
                await Assert.ThrowsAsync<TerminologyPollProcessingServiceException>(
                    terminologyRetrieveOrAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTerminologyPollProcessingServiceException);

            this.terminologyPollServiceMock.Verify(service =>
                service.RetrieveAllTerminologyPollsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedTerminologyPollProcessingServiceException))),
                         Times.Once);

            this.terminologyPollServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}
