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
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfErrorOccursAndLogItAsync(
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
                service.ModifyDataSetSpecificationAsync(inputDataSetSpecification))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<DataSetSpecification> dataSetSpecificationAddTask =
                this.dataSetSpecificationProcessingService.ModifyDataSetSpecificationAsync(inputDataSetSpecification);

            DataSetSpecificationProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<DataSetSpecificationProcessingDependencyValidationException>(
                    dataSetSpecificationAddTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationProcessingDependencyValidationException);

            this.dataSetSpecificationServiceMock.Verify(service =>
                service.ModifyDataSetSpecificationAsync(inputDataSetSpecification),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedDataSetSpecificationProcessingDependencyValidationException))),
                         Times.Once);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDependencyErrorOccursAndLogItAsync(
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
                service.ModifyDataSetSpecificationAsync(inputDataSetSpecification))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<DataSetSpecification> dataSetSpecificationAddTask =
                this.dataSetSpecificationProcessingService.ModifyDataSetSpecificationAsync(inputDataSetSpecification);

            DataSetSpecificationProcessingDependencyException actualException =
                await Assert.ThrowsAsync<DataSetSpecificationProcessingDependencyException>(
                    dataSetSpecificationAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDataSetSpecificationProcessingDependencyException);

            this.dataSetSpecificationServiceMock.Verify(service =>
                service.ModifyDataSetSpecificationAsync(inputDataSetSpecification),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedDataSetSpecificationProcessingDependencyException))),
                         Times.Once);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAsync()
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
                service.ModifyDataSetSpecificationAsync(inputDataSetSpecification))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<DataSetSpecification> addDataSetSpecificationTask =
                this.dataSetSpecificationProcessingService.ModifyDataSetSpecificationAsync(inputDataSetSpecification);

            DataSetSpecificationProcessingServiceException actualException =
                await Assert.ThrowsAsync<DataSetSpecificationProcessingServiceException>(
                    addDataSetSpecificationTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDataSetSpecificationProcessingServiveException);

            this.dataSetSpecificationServiceMock.Verify(service =>
                service.ModifyDataSetSpecificationAsync(inputDataSetSpecification),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedDataSetSpecificationProcessingServiveException))),
                         Times.Once);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
