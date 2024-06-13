// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Processings.OptOuts.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.OptOuts
{
    public partial class OptOutProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveIfDependencyValidationErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            OptOut someOptOut = CreateRandomOptOut();
            OptOut inputOptOut = someOptOut;

            var expectedOptOutProcessingDependencyValidationException =
                new OptOutProcessingDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.optOutServiceMock.Setup(service =>
                service.RetrieveAllOptOuts())
                    .Throws(dependencyValidationException);

            // when
            ValueTask<OptOut> optOutRetrieveOrAddTask =
                this.optOutProcessingService.RetrieveOrAddOptOutAsync(inputOptOut);

            OptOutProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<OptOutProcessingDependencyValidationException>(optOutRetrieveOrAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(
                expectedOptOutProcessingDependencyValidationException);

            this.optOutServiceMock.Verify(service =>
                service.RetrieveAllOptOuts(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedOptOutProcessingDependencyValidationException))),
                         Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDependencyValidationErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            OptOut someOptOut = CreateRandomOptOut();
            OptOut inputOptOut = someOptOut;

            var expectedOptOutProcessingDependencyValidationException =
                new OptOutProcessingDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.optOutServiceMock.Setup(service =>
                service.RetrieveAllOptOuts())
                    .Throws(dependencyValidationException);

            // when
            ValueTask<OptOut> optOutRetrieveOrAddTask =
                this.optOutProcessingService.RetrieveOrAddOptOutAsync(inputOptOut);

            OptOutProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<OptOutProcessingDependencyValidationException>(optOutRetrieveOrAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(
                expectedOptOutProcessingDependencyValidationException);

            this.optOutServiceMock.Verify(service =>
                service.RetrieveAllOptOuts(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedOptOutProcessingDependencyValidationException))),
                         Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRetrieveIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            OptOut someOptOut = CreateRandomOptOut();
            OptOut inputOptOut = someOptOut;

            var expectedOptOutProcessingDependencyException =
                new OptOutProcessingDependencyException(
                    dependencyValidationException.InnerException as Xeption);

            this.optOutServiceMock.Setup(service =>
                service.RetrieveAllOptOuts())
                    .Throws(dependencyValidationException);

            // when
            ValueTask<OptOut> optOutRetrieveOrAddTask =
                this.optOutProcessingService.RetrieveOrAddOptOutAsync(inputOptOut);

            OptOutProcessingDependencyException actualException =
                await Assert.ThrowsAsync<OptOutProcessingDependencyException>(optOutRetrieveOrAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(
                expectedOptOutProcessingDependencyException);

            this.optOutServiceMock.Verify(service =>
                service.RetrieveAllOptOuts(),
                    Times.Once);

            this.optOutServiceMock.Verify(service =>
                service.AddOptOutAsync(It.IsAny<OptOut>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedOptOutProcessingDependencyException))),
                         Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }


        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnAddIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            OptOut someOptOut = CreateRandomOptOut();
            OptOut inputOptOut = someOptOut;

            var expectedOptOutProcessingDependencyException =
                new OptOutProcessingDependencyException(
                    dependencyValidationException.InnerException as Xeption);

            this.optOutServiceMock.Setup(service =>
                service.RetrieveAllOptOuts())
                    .Throws(dependencyValidationException);

            // when
            ValueTask<OptOut> optOutRetrieveOrAddTask =
                this.optOutProcessingService.RetrieveOrAddOptOutAsync(inputOptOut);

            OptOutProcessingDependencyException actualException =
                await Assert.ThrowsAsync<OptOutProcessingDependencyException>(optOutRetrieveOrAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(
                expectedOptOutProcessingDependencyException);

            this.optOutServiceMock.Verify(service =>
                service.RetrieveAllOptOuts(),
                    Times.Once);

            this.optOutServiceMock.Verify(service =>
                service.AddOptOutAsync(inputOptOut),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedOptOutProcessingDependencyException))),
                         Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveIfServiceErrorOccursAsync()
        {
            // given
            OptOut someOptOut = CreateRandomOptOut();
            OptOut inputOptOut = someOptOut;

            var serviceException = new Exception();

            var failedOptOutProcessingServiceException =
                new FailedOptOutProcessingServiceException(
                    message: "Failed opt out processing service error occurred, please contact support.",
                    serviceException);

            var expectedOptOutProcessingServiveException =
                new OptOutProcessingServiceException(
                    failedOptOutProcessingServiceException);

            this.optOutServiceMock.Setup(service =>
                service.RetrieveAllOptOuts())
                    .Throws(serviceException);

            // when
            ValueTask<OptOut> OptOutRetrieveOrAddTask =
                this.optOutProcessingService.RetrieveOrAddOptOutAsync(inputOptOut);

            OptOutProcessingServiceException actualException =
                await Assert.ThrowsAsync<OptOutProcessingServiceException>(OptOutRetrieveOrAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedOptOutProcessingServiveException);

            this.optOutServiceMock.Verify(service =>
                service.RetrieveAllOptOuts(),
                    Times.Once);

            this.optOutServiceMock.Verify(service =>
                service.AddOptOutAsync(It.IsAny<OptOut>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedOptOutProcessingServiveException))),
                         Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAsync()
        {
            // given
            OptOut someOptOut = CreateRandomOptOut();
            OptOut inputOptOut = someOptOut;

            var serviceException = new Exception();

            var failedOptOutProcessingServiceException =
                new FailedOptOutProcessingServiceException(
                    message: "Failed opt out processing service error occurred, please contact support.",
                    serviceException);

            var expectedOptOutProcessingServiveException =
                new OptOutProcessingServiceException(
                    failedOptOutProcessingServiceException);

            this.optOutServiceMock.Setup(service =>
                service.RetrieveAllOptOuts())
                    .Throws(serviceException);

            // when
            ValueTask<OptOut> OptOutRetrieveOrAddTask =
                this.optOutProcessingService.RetrieveOrAddOptOutAsync(inputOptOut);

            OptOutProcessingServiceException actualException =
                await Assert.ThrowsAsync<OptOutProcessingServiceException>(OptOutRetrieveOrAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedOptOutProcessingServiveException);

            this.optOutServiceMock.Verify(service =>
                service.RetrieveAllOptOuts(),
                    Times.Once);

            this.optOutServiceMock.Verify(service =>
                service.AddOptOutAsync(inputOptOut),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedOptOutProcessingServiveException))),
                         Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}