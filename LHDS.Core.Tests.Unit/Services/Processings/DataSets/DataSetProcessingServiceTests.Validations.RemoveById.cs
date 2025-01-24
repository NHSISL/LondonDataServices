// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Processings.DataSets.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.DataSets
{
    public partial class DataSetProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionsOnRemoveIfDataSetProcessingIsNullAndLogItAsync()
        {
            // given
            Guid invalidId = Guid.Empty;

            var invalidArgumentDataSetProcessingException =
                new InvalidArgumentDataSetProcessingException(
                    message: "Invalid argument(s). Please correct the errors and try again.");

            invalidArgumentDataSetProcessingException.AddData(
                key: "Id",
                values: "Id is required");

            var expectedDataSetProcessingValidationException =
                new DataSetProcessingValidationException(
                    message: "DataSet processing validation error occurred, please try again.",
                    innerException: invalidArgumentDataSetProcessingException);

            // when
            ValueTask<DataSet> dataSetRemoveByIdTask =
                this.dataSetProcessingService.RemoveDataSetByIdAsync(invalidId);

            DataSetProcessingValidationException actualDataSetProcessingValidationException =
                await Assert.ThrowsAsync<DataSetProcessingValidationException>(dataSetRemoveByIdTask.AsTask);

            //then
            actualDataSetProcessingValidationException.Should()
                .BeEquivalentTo(expectedDataSetProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataSetProcessingValidationException))),
                        Times.Once);

            this.dataSetServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
