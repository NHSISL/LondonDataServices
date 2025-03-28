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
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidDataSetId = Guid.Empty;

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
            ValueTask<DataSet> removeDataSetByIdTask =
                this.dataSetService.RemoveDataSetByIdAsync(invalidDataSetId);

            DataSetValidationException actualDataSetValidationException =
                await Assert.ThrowsAsync<DataSetValidationException>(
                    removeDataSetByIdTask.AsTask);

            // then
            actualDataSetValidationException.Should()
                .BeEquivalentTo(expectedDataSetValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataSetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteDataSetAsync(It.IsAny<DataSet>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }
    }
}