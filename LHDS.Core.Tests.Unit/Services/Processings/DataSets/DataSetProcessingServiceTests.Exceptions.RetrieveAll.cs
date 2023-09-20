// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveAllIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var expectedDataSetProcessingDependencyValidationException =
                new DataSetProcessingDependencyValidationException(
                    message: "DataSet processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.dataSetServiceMock.Setup(service =>
                service.RetrieveAllDataSets())
                    .Throws(dependencyValidationException);

            // when
            Action dataSetRetrieveAction = () =>
                this.dataSetProcessingService.RetrieveAllDataSets();

            DataSetProcessingDependencyValidationException actualException =
                Assert.Throws<DataSetProcessingDependencyValidationException>(dataSetRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedDataSetProcessingDependencyValidationException);

            this.dataSetServiceMock.Verify(service =>
                service.RetrieveAllDataSets(),
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
        public async Task ShouldThrowDependencyOnRetrieveAllIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var expectedDataSetProcessingDependencyException =
                new DataSetProcessingDependencyException(
                    message: "DataSet processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.dataSetServiceMock.Setup(service =>
                service.RetrieveAllDataSets())
                    .Throws(dependencyException);

            // when
            Action dataSetRetrieveAction = () =>
                this.dataSetProcessingService.RetrieveAllDataSets();

            DataSetProcessingDependencyException actualException =
                Assert.Throws<DataSetProcessingDependencyException>(dataSetRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedDataSetProcessingDependencyException);

            this.dataSetServiceMock.Verify(service =>
                service.RetrieveAllDataSets(),
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