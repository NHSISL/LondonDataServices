// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Processings.IngestionTrackings.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.IngestionTrackings
{
    public partial class IngestionTrackingProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyOrAddIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            IngestionTracking someIngestionTracking = CreateRandomIngestionTracking();
            IngestionTracking inputIngestionTracking = someIngestionTracking;

            var expectedIngestionTrackingProcessingDependencyValidationException =
                new IngestionTrackingProcessingDependencyValidationException(
                    message: "IngestionTracking processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingByIdAsync(inputIngestionTracking.Id))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<IngestionTracking> ingestionTrackingModifyOrAddTask =
                this.ingestionTrackingProcessingService.ModifyOrAddIngestionTrackingAsync(inputIngestionTracking);

            IngestionTrackingProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<IngestionTrackingProcessingDependencyValidationException>(
                    ingestionTrackingModifyOrAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedIngestionTrackingProcessingDependencyValidationException);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByIdAsync(inputIngestionTracking.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedIngestionTrackingProcessingDependencyValidationException))),
                         Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnModifyOrAddIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            IngestionTracking someIngestionTracking = CreateRandomIngestionTracking();
            IngestionTracking inputIngestionTracking = someIngestionTracking;

            var expectedIngestionTrackingProcessingDependencyException =
                new IngestionTrackingProcessingDependencyException(
                    message: "IngestionTracking processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingByIdAsync(inputIngestionTracking.Id))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<IngestionTracking> ingestionTrackingModifyOrAddTask =
                this.ingestionTrackingProcessingService.ModifyOrAddIngestionTrackingAsync(inputIngestionTracking);

            IngestionTrackingProcessingDependencyException actualException =
                await Assert.ThrowsAsync<IngestionTrackingProcessingDependencyException>(
                    ingestionTrackingModifyOrAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedIngestionTrackingProcessingDependencyException);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByIdAsync(inputIngestionTracking.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedIngestionTrackingProcessingDependencyException))),
                         Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyOrAddIfServiceErrorOccursAsync()
        {
            // given
            IngestionTracking someIngestionTracking = CreateRandomIngestionTracking();
            IngestionTracking inputIngestionTracking = someIngestionTracking;

            var serviceException = new Exception();

            var failedIngestionTrackingProcessingServiceException =
                new FailedIngestionTrackingProcessingServiceException(
                    message: "Failed IngestionTracking processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedIngestionTrackingProcessingServiveException =
                new IngestionTrackingProcessingServiceException(
                    message: "IngestionTracking processing service error occurred, please contact support.",
                    innerException: failedIngestionTrackingProcessingServiceException);

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingByIdAsync(inputIngestionTracking.Id))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<IngestionTracking> ingestionTrackingModifyOrAddTask =
                this.ingestionTrackingProcessingService.ModifyOrAddIngestionTrackingAsync(inputIngestionTracking);

            IngestionTrackingProcessingServiceException actualException =
                await Assert.ThrowsAsync<IngestionTrackingProcessingServiceException>(
                    ingestionTrackingModifyOrAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedIngestionTrackingProcessingServiveException);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByIdAsync(inputIngestionTracking.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedIngestionTrackingProcessingServiveException))),
                         Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
