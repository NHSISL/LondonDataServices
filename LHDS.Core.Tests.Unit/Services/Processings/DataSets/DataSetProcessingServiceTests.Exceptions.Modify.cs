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
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfErrorOccursAndLogItAsync(
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
                service.ModifyDataSetAsync(inputDataSet))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<DataSet> dataSetModifyTask =
                this.dataSetProcessingService.ModifyDataSetAsync(inputDataSet);

            DataSetProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<DataSetProcessingDependencyValidationException>(dataSetModifyTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDataSetProcessingDependencyValidationException);

            this.dataSetServiceMock.Verify(service =>
                service.ModifyDataSetAsync(inputDataSet),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedDataSetProcessingDependencyValidationException))),
                         Times.Once);

            this.dataSetServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDependencyErrorOccursAndLogItAsync(
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
                service.ModifyDataSetAsync(inputDataSet))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<DataSet> dataSetModifyTask =
                this.dataSetProcessingService.ModifyDataSetAsync(inputDataSet);

            DataSetProcessingDependencyException actualException =
                await Assert.ThrowsAsync<DataSetProcessingDependencyException>(dataSetModifyTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDataSetProcessingDependencyException);

            this.dataSetServiceMock.Verify(service =>
                service.ModifyDataSetAsync(inputDataSet),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedDataSetProcessingDependencyException))),
                         Times.Once);

            this.dataSetServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAsync()
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
                service.ModifyDataSetAsync(inputDataSet))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<DataSet> dataSetModifyTask =
                this.dataSetProcessingService.ModifyDataSetAsync(inputDataSet);

            DataSetProcessingServiceException actualException =
                await Assert.ThrowsAsync<DataSetProcessingServiceException>(dataSetModifyTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDataSetProcessingServiveException);

            this.dataSetServiceMock.Verify(service =>
                service.ModifyDataSetAsync(inputDataSet),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedDataSetProcessingServiveException))),
                         Times.Once);

            this.dataSetServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}