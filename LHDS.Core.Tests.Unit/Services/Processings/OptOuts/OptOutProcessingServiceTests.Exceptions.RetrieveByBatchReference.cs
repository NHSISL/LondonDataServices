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
        public async Task
            ShouldThrowDependencyValidationExceptionOnRetrieveByBatchReferenceIfDependencyValidationErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            string batchRefernce = GetRandomString();

            var expectedOptOutProcessingDependencyValidationException =
                new OptOutProcessingDependencyValidationException(
                    message: "Opt out processing dependency validation error occurred, please contact support.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.optOutServiceMock.Setup(service =>
                service.RetrieveAllOptOutsAsync())
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<OptOut> optOutRetrieveByNhsNumberTask =
                this.optOutProcessingService.RetrieveOptOutByNhsNumberAsync(batchRefernce);

            OptOutProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<OptOutProcessingDependencyValidationException>(optOutRetrieveByNhsNumberTask.AsTask);

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
        public async Task ShouldThrowDependencyOnRetrieveByBatchReferenceIfDependencyErrorOccursAndLogItAsync(
        Xeption dependencyException)
        {
            // given
            string batchRefernce = GetRandomString();

            var expectedOptOutProcessingDependencyException =
                new OptOutProcessingDependencyException(
                    message: "Opt out processing dependency error occurred, please contact support.",
                    dependencyException.InnerException as Xeption);

            this.optOutServiceMock.Setup(service =>
                service.RetrieveAllOptOutsAsync())
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<OptOut> optOutRetrieveTask =
                this.optOutProcessingService.RetrieveOptOutByNhsNumberAsync(batchRefernce);

            OptOutProcessingDependencyException actualException =
                await Assert.ThrowsAsync<OptOutProcessingDependencyException>(optOutRetrieveTask.AsTask);

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
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByBatchReferenceIfServiceErrorOccursAsync()
        {
            // given
            string batchRefernce = GetRandomString();

            var serviceException = new Exception();

            var failedOptOutProcessingServiceException =
                new FailedOptOutProcessingServiceException(
                    message: "Failed opt out processing service error occurred, please contact support.",
                    serviceException);

            var expectedOptOutProcessingServiveException =
                new OptOutProcessingServiceException(
                    failedOptOutProcessingServiceException);

            this.optOutServiceMock.Setup(service =>
                service.RetrieveAllOptOutsAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<OptOut> optOutRetrieveTask =
                this.optOutProcessingService.RetrieveOptOutByNhsNumberAsync(batchRefernce);

            OptOutProcessingServiceException actualException =
                await Assert.ThrowsAsync<OptOutProcessingServiceException>(optOutRetrieveTask.AsTask);

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