// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
                    .Throws(dependencyValidationException);

            // when
            ValueTask<DataSet> dataSetAddTask =
                this.dataSetProcessingService.ModifyDataSetAsync(inputDataSet);

            DataSetProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<DataSetProcessingDependencyValidationException>(dataSetAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDataSetProcessingDependencyValidationException);

            this.dataSetServiceMock.Verify(service =>
                service.ModifyDataSetAsync(inputDataSet),
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
        public async Task ShouldThrowDependencyOnModifyIfDependencyErrorOccursAndLogItAsync(
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
                    .Throws(dependencyException);

            // when
            ValueTask<DataSet> dataSetAddTask =
                this.dataSetProcessingService.ModifyDataSetAsync(inputDataSet);

            DataSetProcessingDependencyException actualException =
                await Assert.ThrowsAsync<DataSetProcessingDependencyException>(dataSetAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDataSetProcessingDependencyException);

            this.dataSetServiceMock.Verify(service =>
                service.ModifyDataSetAsync(inputDataSet),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedDataSetProcessingDependencyException))),
                         Times.Once);

            this.dataSetServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}