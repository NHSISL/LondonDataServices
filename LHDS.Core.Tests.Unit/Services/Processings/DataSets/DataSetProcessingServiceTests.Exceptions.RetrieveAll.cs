// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using FluentAssertions;
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
        public void ShouldThrowDependencyValidationExceptionOnRetrieveAllIfErrorOccursAndLogItAsync(
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
            Action dataSetRetrieveAction = () =>
                dataSetProcessingService.RetrieveAllDataSets();

            DataSetProcessingDependencyValidationException actualException =
                Assert.Throws<DataSetProcessingDependencyValidationException>(dataSetRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedDataSetProcessingDependencyValidationException);

            dataSetServiceMock.Verify(service =>
                service.RetrieveAllDataSetsAsync(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedDataSetProcessingDependencyValidationException))),
                         Times.Once);

            dataSetServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public void ShouldThrowDependencyExceptionOnRetrieveAllIfDependencyErrorOccursAndLogItAsync(
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
            Action dataSetRetrieveAction = () =>
                dataSetProcessingService.RetrieveAllDataSets();

            DataSetProcessingDependencyException actualException =
                Assert.Throws<DataSetProcessingDependencyException>(dataSetRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedDataSetProcessingDependencyException);

            dataSetServiceMock.Verify(service =>
                service.RetrieveAllDataSetsAsync(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedDataSetProcessingDependencyException))),
                         Times.Once);

            dataSetServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAsync()
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
            Action dataSetRetrieveAction = () =>
                dataSetProcessingService.RetrieveAllDataSets();

            DataSetProcessingServiceException actualException =
                Assert.Throws<DataSetProcessingServiceException>(dataSetRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedDataSetProcessingServiveException);

            dataSetServiceMock.Verify(service =>
                service.RetrieveAllDataSetsAsync(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedDataSetProcessingServiveException))),
                         Times.Once);

            dataSetServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}