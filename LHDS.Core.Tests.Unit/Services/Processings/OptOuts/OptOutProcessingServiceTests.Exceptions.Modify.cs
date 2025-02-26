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
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDependencyValidationErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            OptOut someOptOut = CreateRandomOptOut();
            OptOut inputOptOut = someOptOut;

            var expectedOptOutProcessingDependencyValidationException =
                new OptOutProcessingDependencyValidationException(
                    message: "Opt out processing dependency validation error occurred, please contact support.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.optOutServiceMock.Setup(service =>
                service.RetrieveAllOptOutsAsync())
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<OptOut> optOutModifyTask =
                this.optOutProcessingService.AddOrModifyOptOutAsync(inputOptOut);

            OptOutProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<OptOutProcessingDependencyValidationException>(optOutModifyTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(
                expectedOptOutProcessingDependencyValidationException);

            this.optOutServiceMock.Verify(service =>
                service.RetrieveAllOptOutsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedOptOutProcessingDependencyValidationException))),
                         Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyOnAddIfDependencyErrorOccursAndLogItAsync(
             Xeption dependencyException)
        {
            // given
            OptOut someOptOut = CreateRandomOptOut();
            OptOut inputOptOut = someOptOut;

            var expectedOptOutProcessingDependencyException =
                new OptOutProcessingDependencyException(
                    message: "Opt out processing dependency error occurred, please contact support.",
                    dependencyException.InnerException as Xeption);

            this.optOutServiceMock.Setup(service =>
                service.RetrieveAllOptOutsAsync())
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<OptOut> optOutModifyTask =
                this.optOutProcessingService.AddOrModifyOptOutAsync(inputOptOut);

            OptOutProcessingDependencyException actualException =
                await Assert.ThrowsAsync<OptOutProcessingDependencyException>(optOutModifyTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedOptOutProcessingDependencyException);

            this.optOutServiceMock.Verify(service =>
                service.RetrieveAllOptOutsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedOptOutProcessingDependencyException))),
                         Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAsync()
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
                    message: "Opt out processing service error occurred, please contact support.",
                    failedOptOutProcessingServiceException);

            this.optOutServiceMock.Setup(service =>
                service.RetrieveAllOptOutsAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<OptOut> optOutModifyTask =
                this.optOutProcessingService.AddOrModifyOptOutAsync(inputOptOut);

            OptOutProcessingServiceException actualException =
                await Assert.ThrowsAsync<OptOutProcessingServiceException>(optOutModifyTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedOptOutProcessingServiveException);

            this.optOutServiceMock.Verify(service =>
                service.RetrieveAllOptOutsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedOptOutProcessingServiveException))),
                         Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}