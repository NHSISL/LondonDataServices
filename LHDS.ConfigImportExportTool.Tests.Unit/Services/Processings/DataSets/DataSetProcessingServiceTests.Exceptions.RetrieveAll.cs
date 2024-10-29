// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Foundations.Datasets;
using LHDS.ConfigImportExportTool.Models.Processings.DataSets.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Processings.DataSets
{
    public partial class DataSetProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveAllIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var expectedDataSetProcessingDependencyValidationException =
                new DataSetProcessingDependencyValidationException(
                    message: "DataSet processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            dataSetServiceMock.Setup(service =>
                service.RetrieveAllDataSetsAsync())
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<IQueryable<DataSet>> retrieveAllDataSetsTask =
                this.dataSetProcessingService.RetrieveAllDataSetsAsync();

            DataSetProcessingDependencyValidationException actualDataSetDependencyException =
                await Assert.ThrowsAsync<DataSetProcessingDependencyValidationException>(retrieveAllDataSetsTask.AsTask);

            // then
            actualDataSetDependencyException.Should().BeEquivalentTo(
                expectedDataSetProcessingDependencyValidationException);

            dataSetServiceMock.Verify(service =>
                service.RetrieveAllDataSetsAsync(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedDataSetProcessingDependencyValidationException))),
                         Times.Once);

            dataSetServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRetrieveAllIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var expectedDataSetProcessingDependencyException =
                new DataSetProcessingDependencyException(
                    message: "DataSet processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            dataSetServiceMock.Setup(service =>
                service.RetrieveAllDataSetsAsync())
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<IQueryable<DataSet>> retrieveAllDataSetsTask =
                this.dataSetProcessingService.RetrieveAllDataSetsAsync();

            DataSetProcessingDependencyException actualDataSetDependencyException =
                await Assert.ThrowsAsync<DataSetProcessingDependencyException>(retrieveAllDataSetsTask.AsTask);

            // then
            actualDataSetDependencyException.Should().BeEquivalentTo(expectedDataSetProcessingDependencyException);

            dataSetServiceMock.Verify(service =>
                service.RetrieveAllDataSetsAsync(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedDataSetProcessingDependencyException))),
                         Times.Once);

            dataSetServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAsync()
        {
            // given
            var serviceException = new Exception();

            var failedDataSetProcessingServiceException =
                new FailedDataSetProcessingServiceException(
                    message: "Failed DataSet processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedDataSetProcessingServiveException =
                new DataSetProcessingServiceException(
                    message: "DataSet processing service error occurred, please contact support.",
                    innerException: failedDataSetProcessingServiceException);

            dataSetServiceMock.Setup(service =>
                service.RetrieveAllDataSetsAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<IQueryable<DataSet>> retrieveAllDataSetsTask =
                this.dataSetProcessingService.RetrieveAllDataSetsAsync();

            DataSetProcessingServiceException actualDataSetServiceException =
                await Assert.ThrowsAsync<DataSetProcessingServiceException>(retrieveAllDataSetsTask.AsTask);

            // then
            actualDataSetServiceException.Should().BeEquivalentTo(expectedDataSetProcessingServiveException);

            dataSetServiceMock.Verify(service =>
                service.RetrieveAllDataSetsAsync(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedDataSetProcessingServiveException))),
                         Times.Once);

            dataSetServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}