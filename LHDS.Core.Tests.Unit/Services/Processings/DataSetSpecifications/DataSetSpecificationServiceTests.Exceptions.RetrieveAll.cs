// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using FluentAssertions;
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
        public void ShouldThrowDependencyValidationExceptionOnRetrieveAllIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var expectedDataSetSpecificationProcessingDependencyValidationException =
                new DataSetSpecificationProcessingDependencyValidationException(
                    message: "DataSetSpecification processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            dataSetSpecificationServiceMock.Setup(service =>
                service.RetrieveAllDataSetSpecifications())
                    .Throws(dependencyValidationException);

            // when
            Action dataSetSpecificationRetrieveAction = () =>
                dataSetSpecificationProcessingService.RetrieveAllDataSetSpecifications();

            DataSetSpecificationProcessingDependencyValidationException actualException =
                Assert.Throws<DataSetSpecificationProcessingDependencyValidationException>(
                    dataSetSpecificationRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedDataSetSpecificationProcessingDependencyValidationException);

            dataSetSpecificationServiceMock.Verify(service =>
                service.RetrieveAllDataSetSpecifications(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedDataSetSpecificationProcessingDependencyValidationException))),
                         Times.Once);

            dataSetSpecificationServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public void ShouldThrowDependencyExceptionOnRetrieveAllIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var expectedDataSetSpecificationProcessingDependencyException =
                new DataSetSpecificationProcessingDependencyException(
                    message: "DataSetSpecification processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            dataSetSpecificationServiceMock.Setup(service =>
                service.RetrieveAllDataSetSpecifications())
                    .Throws(dependencyException);

            // when
            Action dataSetSpecificationRetrieveAction = () =>
                dataSetSpecificationProcessingService.RetrieveAllDataSetSpecifications();

            DataSetSpecificationProcessingDependencyException actualException =
                Assert.Throws<DataSetSpecificationProcessingDependencyException>(dataSetSpecificationRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedDataSetSpecificationProcessingDependencyException);

            dataSetSpecificationServiceMock.Verify(service =>
                service.RetrieveAllDataSetSpecifications(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedDataSetSpecificationProcessingDependencyException))),
                         Times.Once);

            dataSetSpecificationServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAsync()
        {
            // given
            var serviceException = new Exception();

            var failedDataSetSpecificationProcessingServiceException =
                new FailedDataSetSpecificationProcessingServiceException(
                    message: "Failed DataSetSpecification processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedDataSetSpecificationProcessingServiveException =
                new DataSetSpecificationProcessingServiceException(
                    message: "DataSetSpecification processing service error occurred, please contact support.",
                    innerException: failedDataSetSpecificationProcessingServiceException);

            dataSetSpecificationServiceMock.Setup(service =>
                service.RetrieveAllDataSetSpecifications())
                    .Throws(serviceException);

            // when
            Action dataSetSpecificationRetrieveAction = () =>
                dataSetSpecificationProcessingService.RetrieveAllDataSetSpecifications();

            DataSetSpecificationProcessingServiceException actualException =
                Assert.Throws<DataSetSpecificationProcessingServiceException>(dataSetSpecificationRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedDataSetSpecificationProcessingServiveException);

            dataSetSpecificationServiceMock.Verify(service =>
                service.RetrieveAllDataSetSpecifications(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedDataSetSpecificationProcessingServiveException))),
                         Times.Once);

            dataSetSpecificationServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
