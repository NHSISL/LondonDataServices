// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveByIdIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            Guid someId = Guid.NewGuid();

            var expectedDataSetProcessingDependencyValidationException =
                new DataSetProcessingDependencyValidationException(
                    message: "DataSet processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.dataSetServiceMock.Setup(service =>
                service.RetrieveDataSetByIdAsync(someId))
                    .Throws(dependencyValidationException);

            // when
            ValueTask<DataSet> dataSetRetrieveByIdTask =
                this.dataSetProcessingService.RetrieveDataSetByIdAsync(someId);

            DataSetProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<DataSetProcessingDependencyValidationException>(
                    dataSetRetrieveByIdTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDataSetProcessingDependencyValidationException);

            this.dataSetServiceMock.Verify(service =>
                service.RetrieveDataSetByIdAsync(someId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedDataSetProcessingDependencyValidationException))),
                         Times.Once);

            this.dataSetServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
