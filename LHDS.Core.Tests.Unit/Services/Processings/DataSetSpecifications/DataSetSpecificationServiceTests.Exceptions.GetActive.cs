// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Processings.DataSetSpecifications.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.DataSetSpecifications
{
    public partial class DataSetSpecificationProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnGetActiveIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            Guid someId = Guid.NewGuid();

            var expectedDataSetSpecificationProcessingDependencyValidationException =
                new DataSetSpecificationProcessingDependencyValidationException(
                    message: "DataSetSpecification processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.dataSetSpecificationServiceMock.Setup(service =>
                service.RetrieveAllDataSetSpecificationsAsync())
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<DataSetSpecification> dataSetSpecificationGetActiveTask =
                 this.dataSetSpecificationProcessingService.GetActiveDataSetSpecification(someId);

            DataSetSpecificationProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<DataSetSpecificationProcessingDependencyValidationException>(
                    dataSetSpecificationGetActiveTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationProcessingDependencyValidationException);

            this.dataSetSpecificationServiceMock.Verify(service =>
                service.RetrieveAllDataSetSpecificationsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedDataSetSpecificationProcessingDependencyValidationException))),
                         Times.Once);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnGetActiveIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            Guid someId = Guid.NewGuid();

            var expectedDataSetSpecificationProcessingDependencyException =
                new DataSetSpecificationProcessingDependencyException(
                    message: "DataSetSpecification processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.dataSetSpecificationServiceMock.Setup(service =>
                service.RetrieveAllDataSetSpecificationsAsync())
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<DataSetSpecification> dataSetSpecificationGetActiveTask =
                this.dataSetSpecificationProcessingService.GetActiveDataSetSpecification(someId);

            DataSetSpecificationProcessingDependencyException actualException =
                await Assert.ThrowsAsync<DataSetSpecificationProcessingDependencyException>(
                    dataSetSpecificationGetActiveTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDataSetSpecificationProcessingDependencyException);

            this.dataSetSpecificationServiceMock.Verify(service =>
                service.RetrieveAllDataSetSpecificationsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedDataSetSpecificationProcessingDependencyException))),
                         Times.Once);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetActiveIfServiceErrorOccursAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedDataSetSpecificationProcessingServiceException =
                new FailedDataSetSpecificationProcessingServiceException(
                    message: "Failed DataSetSpecification processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedDataSetSpecificationProcessingServiveException =
                new DataSetSpecificationProcessingServiceException(
                    message: "DataSetSpecification processing service error occurred, please contact support.",
                    innerException: failedDataSetSpecificationProcessingServiceException);

            this.dataSetSpecificationServiceMock.Setup(service =>
                service.RetrieveAllDataSetSpecificationsAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<DataSetSpecification> dataSetSpecificationRetrieveByIdTask =
                this.dataSetSpecificationProcessingService.GetActiveDataSetSpecification(someId);

            DataSetSpecificationProcessingServiceException actualException =
                await Assert.ThrowsAsync<DataSetSpecificationProcessingServiceException>(
                    dataSetSpecificationRetrieveByIdTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDataSetSpecificationProcessingServiveException);

            this.dataSetSpecificationServiceMock.Verify(service =>
                service.RetrieveAllDataSetSpecificationsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedDataSetSpecificationProcessingServiveException))),
                         Times.Once);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
