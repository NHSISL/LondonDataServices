// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using FluentAssertions;
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
        public void ShouldThrowDependencyValidationExceptionOnRetrieveAllIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var expectedTerminologyPollProcessingDependencyValidationException =
                new TerminologyPollProcessingDependencyValidationException(
                    message: "Terminology poll processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.terminologyPollServiceMock.Setup(service =>
                service.RetrieveAllTerminologyPolls())
                    .Throws(dependencyValidationException);

            // when
            Action terminologyPollRetrieveAction = () =>
                this.terminologyPollProcessingService.RetrieveAllTerminologyPolls();

            TerminologyPollProcessingDependencyValidationException actualException =
                Assert.Throws<TerminologyPollProcessingDependencyValidationException>(
                    terminologyPollRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedTerminologyPollProcessingDependencyValidationException);

            this.terminologyPollServiceMock.Verify(service =>
                service.RetrieveAllTerminologyPolls(),
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
        public void ShouldThrowDependencyExceptionOnetrieveAllIfErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var expectedTerminologyPollProcessingDependencyException =
                new TerminologyPollProcessingDependencyException(
                    message: "Terminology poll processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.terminologyPollServiceMock.Setup(service =>
                 service.RetrieveAllTerminologyPolls())
                     .Throws(dependencyException);

            // when
            Action terminologyPollRetrieveAction = () =>
                this.terminologyPollProcessingService.RetrieveAllTerminologyPolls();

            TerminologyPollProcessingDependencyException actualException =
                Assert.Throws<TerminologyPollProcessingDependencyException>(
                    terminologyPollRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedTerminologyPollProcessingDependencyException);

            this.terminologyPollServiceMock.Verify(service =>
                service.RetrieveAllTerminologyPolls(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedTerminologyPollProcessingDependencyException))),
                         Times.Once);

            this.terminologyPollServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnetrieveAllIfServiceErrorOccursAsync()
        {
            // given
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
                 service.RetrieveAllTerminologyPolls())
                     .Throws(serviceException);

            // when
            Action terminologyPollRetrieveAction = () =>
                this.terminologyPollProcessingService.RetrieveAllTerminologyPolls();

            TerminologyPollProcessingServiceException actualException =
                Assert.Throws<TerminologyPollProcessingServiceException>(
                    terminologyPollRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedTerminologyPollProcessingServiceException);

            this.terminologyPollServiceMock.Verify(service =>
                service.RetrieveAllTerminologyPolls(),
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
