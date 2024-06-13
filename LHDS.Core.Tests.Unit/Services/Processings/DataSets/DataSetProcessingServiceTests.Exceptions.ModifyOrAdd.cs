// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Processings.DataSets.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.DataSets
{
    public partial class DataSetProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveOrModifyOrAddIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            DataSet someDataSet = CreateRandomDataSet();
            DataSet inputDataSet = someDataSet;

            var expectedDataSetProcessingDependencyValidationException =
                new DataSetProcessingDependencyValidationException(
                    message: "DataSet processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.dataSetServiceMock.Setup(service =>
                service.RetrieveDataSetByIdAsync(inputDataSet.Id))
                    .Throws(dependencyValidationException);

            // when
            ValueTask<DataSet> dataSetModifyOrAddTask =
                this.dataSetProcessingService.ModifyOrAddDataSetAsync(inputDataSet);

            DataSetProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<DataSetProcessingDependencyValidationException>(
                    dataSetModifyOrAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDataSetProcessingDependencyValidationException);

            this.dataSetServiceMock.Verify(service =>
                service.RetrieveDataSetByIdAsync(inputDataSet.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedDataSetProcessingDependencyValidationException))),
                         Times.Once);

            this.dataSetServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRetrieveOrModifyOrAddIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            DataSet someDataSet = CreateRandomDataSet();
            DataSet inputDataSet = someDataSet;

            var expectedDataSetProcessingDependencyException =
                new DataSetProcessingDependencyException(
                    message: "DataSet processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.dataSetServiceMock.Setup(service =>
                service.RetrieveDataSetByIdAsync(inputDataSet.Id))
                    .Throws(dependencyException);

            // when
            ValueTask<DataSet> dataSetModifyOrAddTask =
                this.dataSetProcessingService.ModifyOrAddDataSetAsync(inputDataSet);

            DataSetProcessingDependencyException actualException =
                await Assert.ThrowsAsync<DataSetProcessingDependencyException>(dataSetModifyOrAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDataSetProcessingDependencyException);

            this.dataSetServiceMock.Verify(service =>
                service.RetrieveDataSetByIdAsync(inputDataSet.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedDataSetProcessingDependencyException))),
                         Times.Once);

            this.dataSetServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveOrModifyOrAddIfServiceErrorOccursAsync()
        {
            // given
            DataSet someDataSet = CreateRandomDataSet();
            DataSet inputDataSet = someDataSet;

            var serviceException = new Exception();

            var failedDataSetProcessingServiceException =
                new FailedDataSetProcessingServiceException(
                    message: "Failed DataSet processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedDataSetProcessingServiveException =
                new DataSetProcessingServiceException(
                    message: "DataSet processing service error occurred, please contact support.",
                    innerException: failedDataSetProcessingServiceException);

            this.dataSetServiceMock.Setup(service =>
                service.RetrieveDataSetByIdAsync(inputDataSet.Id))
                    .Throws(serviceException);

            // when
            ValueTask<DataSet> dataSetModifyOrAddTask =
                this.dataSetProcessingService.ModifyOrAddDataSetAsync(inputDataSet);

            DataSetProcessingServiceException actualException =
                await Assert.ThrowsAsync<DataSetProcessingServiceException>(dataSetModifyOrAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDataSetProcessingServiveException);

            this.dataSetServiceMock.Verify(service =>
                service.RetrieveDataSetByIdAsync(inputDataSet.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedDataSetProcessingServiveException))),
                         Times.Once);

            this.dataSetServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}