// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
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
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveAllIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var expectedDataSetSpecificationProcessingDependencyValidationException =
                new DataSetSpecificationProcessingDependencyValidationException(
                    message: "DataSetSpecification processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            dataSetSpecificationServiceMock.Setup(service =>
                service.RetrieveAllDataSetSpecificationsAsync())
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<IQueryable<DataSetSpecification>> dataSetSpecificationRetrieveTask =
                dataSetSpecificationProcessingService.RetrieveAllDataSetSpecificationsAsync();

            DataSetSpecificationProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<DataSetSpecificationProcessingDependencyValidationException>(
                    dataSetSpecificationRetrieveTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDataSetSpecificationProcessingDependencyValidationException);

            dataSetSpecificationServiceMock.Verify(service =>
                service.RetrieveAllDataSetSpecificationsAsync(),
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
        public async Task ShouldThrowDependencyExceptionOnRetrieveAllIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var expectedDataSetSpecificationProcessingDependencyException =
                new DataSetSpecificationProcessingDependencyException(
                    message: "DataSetSpecification processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            dataSetSpecificationServiceMock.Setup(service =>
                service.RetrieveAllDataSetSpecificationsAsync())
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<IQueryable<DataSetSpecification>> dataSetSpecificationRetrieveTask =
                 dataSetSpecificationProcessingService.RetrieveAllDataSetSpecificationsAsync();

            DataSetSpecificationProcessingDependencyException actualException =
                await Assert.ThrowsAsync<DataSetSpecificationProcessingDependencyException>(
                    dataSetSpecificationRetrieveTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDataSetSpecificationProcessingDependencyException);

            dataSetSpecificationServiceMock.Verify(service =>
                service.RetrieveAllDataSetSpecificationsAsync(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedDataSetSpecificationProcessingDependencyException))),
                         Times.Once);

            dataSetSpecificationServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAsync()
        {
            // given
            var serviceException = new Exception();

            var failedDataSetSpecificationProcessingServiceException =
                new FailedDataSetSpecificationProcessingServiceException(
                    message: "Failed DataSetSpecification processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedDataSetSpecificationProcessingServiceException =
                new DataSetSpecificationProcessingServiceException(
                    message: "DataSetSpecification processing service error occurred, please contact support.",
                    innerException: failedDataSetSpecificationProcessingServiceException);

            dataSetSpecificationServiceMock.Setup(service =>
                service.RetrieveAllDataSetSpecificationsAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<IQueryable<DataSetSpecification>> dataSetSpecificationRetrieveTask =
                dataSetSpecificationProcessingService.RetrieveAllDataSetSpecificationsAsync();

            DataSetSpecificationProcessingServiceException actualException =
                await Assert.ThrowsAsync<DataSetSpecificationProcessingServiceException>(
                    dataSetSpecificationRetrieveTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDataSetSpecificationProcessingServiceException);

            dataSetSpecificationServiceMock.Verify(service =>
                service.RetrieveAllDataSetSpecificationsAsync(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedDataSetSpecificationProcessingServiceException))),
                         Times.Once);

            dataSetSpecificationServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
