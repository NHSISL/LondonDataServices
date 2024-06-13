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
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveOrAddIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            DataSetSpecification someDataSetSpecification = CreateRandomDataSetSpecification();
            DataSetSpecification inputDataSetSpecification = someDataSetSpecification;

            var expectedDataSetSpecificationProcessingDependencyValidationException =
                new DataSetSpecificationProcessingDependencyValidationException(
                    message: "DataSetSpecification processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.dataSetSpecificationServiceMock.Setup(service =>
                service.RetrieveDataSetSpecificationByIdAsync(inputDataSetSpecification.Id))
                    .Throws(dependencyValidationException);

            // when
            ValueTask<DataSetSpecification> dataSetSpecificationRetrieveOrAddTask =
                this.dataSetSpecificationProcessingService.RetrieveOrAddDataSetSpecificationAsync(inputDataSetSpecification);

            DataSetSpecificationProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<DataSetSpecificationProcessingDependencyValidationException>(
                    dataSetSpecificationRetrieveOrAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDataSetSpecificationProcessingDependencyValidationException);

            this.dataSetSpecificationServiceMock.Verify(service =>
                service.RetrieveDataSetSpecificationByIdAsync(inputDataSetSpecification.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedDataSetSpecificationProcessingDependencyValidationException))),
                         Times.Once);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRetrieveOrAddIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            DataSetSpecification someDataSetSpecification = CreateRandomDataSetSpecification();
            DataSetSpecification inputDataSetSpecification = someDataSetSpecification;

            var expectedDataSetSpecificationProcessingDependencyException =
                new DataSetSpecificationProcessingDependencyException(
                    message: "DataSetSpecification processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.dataSetSpecificationServiceMock.Setup(service =>
                service.RetrieveDataSetSpecificationByIdAsync(inputDataSetSpecification.Id))
                    .Throws(dependencyException);

            // when
            ValueTask<DataSetSpecification> dataSetSpecificationRetrieveOrAddTask =
                this.dataSetSpecificationProcessingService.RetrieveOrAddDataSetSpecificationAsync(inputDataSetSpecification);

            DataSetSpecificationProcessingDependencyException actualException =
                await Assert.ThrowsAsync<DataSetSpecificationProcessingDependencyException>(
                    dataSetSpecificationRetrieveOrAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDataSetSpecificationProcessingDependencyException);

            this.dataSetSpecificationServiceMock.Verify(service =>
                service.RetrieveDataSetSpecificationByIdAsync(inputDataSetSpecification.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedDataSetSpecificationProcessingDependencyException))),
                         Times.Once);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveOrAddIfServiceErrorOccursAsync()
        {
            // given
            DataSetSpecification someDataSetSpecification = CreateRandomDataSetSpecification();
            DataSetSpecification inputDataSetSpecification = someDataSetSpecification;

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
                service.RetrieveDataSetSpecificationByIdAsync(inputDataSetSpecification.Id))
                    .Throws(serviceException);

            // when
            ValueTask<DataSetSpecification> addDataSetSpecificationTask =
                this.dataSetSpecificationProcessingService.RetrieveOrAddDataSetSpecificationAsync(inputDataSetSpecification);

            DataSetSpecificationProcessingServiceException actualException =
                await Assert.ThrowsAsync<DataSetSpecificationProcessingServiceException>(addDataSetSpecificationTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDataSetSpecificationProcessingServiveException);

            this.dataSetSpecificationServiceMock.Verify(service =>
                service.RetrieveDataSetSpecificationByIdAsync(inputDataSetSpecification.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedDataSetSpecificationProcessingServiveException))),
                         Times.Once);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
