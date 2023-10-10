// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveAllIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var expectedDataSetSpecificationProcessingDependencyValidationException =
                new DataSetSpecificationProcessingDependencyValidationException(
                    message: "DataSetSpecification processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.dataSetSpecificationServiceMock.Setup(service =>
                service.RetrieveAllDataSetSpecifications())
                    .Throws(dependencyValidationException);

            // when
            Action dataSetSpecificationRetrieveAction = () =>
                this.dataSetSpecificationProcessingService.RetrieveAllDataSetSpecifications();

            DataSetSpecificationProcessingDependencyValidationException actualException =
                Assert.Throws<DataSetSpecificationProcessingDependencyValidationException>(
                    dataSetSpecificationRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedDataSetSpecificationProcessingDependencyValidationException);

            this.dataSetSpecificationServiceMock.Verify(service =>
                service.RetrieveAllDataSetSpecifications(),
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
        public async Task ShouldThrowDependencyExceptionOnRetrieveAllIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var expectedDataSetSpecificationProcessingDependencyException =
                new DataSetSpecificationProcessingDependencyException(
                    message: "DataSetSpecification processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.dataSetSpecificationServiceMock.Setup(service =>
                service.RetrieveAllDataSetSpecifications())
                    .Throws(dependencyException);

            // when
            Action dataSetSpecificationRetrieveAction = () =>
                this.dataSetSpecificationProcessingService.RetrieveAllDataSetSpecifications();

            DataSetSpecificationProcessingDependencyException actualException =
                Assert.Throws<DataSetSpecificationProcessingDependencyException>(dataSetSpecificationRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedDataSetSpecificationProcessingDependencyException);

            this.dataSetSpecificationServiceMock.Verify(service =>
                service.RetrieveAllDataSetSpecifications(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedDataSetSpecificationProcessingDependencyException))),
                         Times.Once);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAsync()
        {
            // given
            var serviceException = new Exception();

            var failedDataSetSpecificationProcessingServiceException =
                new FailedDataSetSpecificationProcessingServiceException(
                    message: "Failed DataSetSpecification processing service error occurred, contact support.",
                    innerException: serviceException);

            var expectedDataSetSpecificationProcessingServiveException =
                new DataSetSpecificationProcessingServiceException(
                    message: "DataSetSpecification processing service error occurred, contact support.",
                    innerException: failedDataSetSpecificationProcessingServiceException);

            this.dataSetSpecificationServiceMock.Setup(service =>
                service.RetrieveAllDataSetSpecifications())
                    .Throws(serviceException);

            // when
            Action dataSetSpecificationRetrieveAction = () =>
                this.dataSetSpecificationProcessingService.RetrieveAllDataSetSpecifications();

            DataSetSpecificationProcessingServiceException actualException =
                Assert.Throws<DataSetSpecificationProcessingServiceException>(dataSetSpecificationRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedDataSetSpecificationProcessingServiveException);

            this.dataSetSpecificationServiceMock.Verify(service =>
                service.RetrieveAllDataSetSpecifications(),
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
