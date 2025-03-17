// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSets.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSets
{
    public partial class DataSetServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidDataSetId = Guid.Empty;

            var invalidDataSetException =
                new InvalidDataSetException(
                    message: "Invalid dataSet. Please correct the errors and try again.");

            invalidDataSetException.AddData(
                key: nameof(DataSet.Id),
                values: "Id is required");

            var expectedDataSetValidationException =
                new DataSetValidationException(
                    message: "DataSet validation errors occurred, please try again.",
                    innerException: invalidDataSetException);

            // when
            ValueTask<DataSet> retrieveDataSetByIdTask =
                this.dataSetService.RetrieveDataSetByIdAsync(invalidDataSetId);

            DataSetValidationException actualDataSetValidationException =
                await Assert.ThrowsAsync<DataSetValidationException>(
                    retrieveDataSetByIdTask.AsTask);

            // then
            actualDataSetValidationException.Should()
                .BeEquivalentTo(expectedDataSetValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataSetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfDataSetIsNotFoundAndLogItAsync()
        {
            //given
            Guid someDataSetId = Guid.NewGuid();
            DataSet noDataSet = null;

            var notFoundDataSetException =
                new NotFoundDataSetException(someDataSetId);

            var expectedDataSetValidationException =
                new DataSetValidationException(
                    message: "DataSet validation errors occurred, please try again.",
                    innerException: notFoundDataSetException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noDataSet);

            //when
            ValueTask<DataSet> retrieveDataSetByIdTask =
                this.dataSetService.RetrieveDataSetByIdAsync(someDataSetId);

            DataSetValidationException actualDataSetValidationException =
                await Assert.ThrowsAsync<DataSetValidationException>(
                    retrieveDataSetByIdTask.AsTask);

            //then
            actualDataSetValidationException.Should().BeEquivalentTo(expectedDataSetValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataSetValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }
    }
}