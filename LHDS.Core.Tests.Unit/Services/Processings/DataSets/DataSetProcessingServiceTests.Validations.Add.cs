// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        public async Task ShouldThrowValidationExceptionsOnAddIfDataSetProcessingIsNullAndLogItAsync()
        {
            // given
            DataSet nullDataSet = null;

            var nullDataSetProcessingException =
                new NullDataSetProcessingException(message: "DataSet is null.");

            var expectedDataSetProcessingValidationException =
                new DataSetProcessingValidationException(
                    message: "DataSet processing validation error occurred, please try again.",
                    innerException: nullDataSetProcessingException);

            // when
            ValueTask<DataSet> AddDataSetTask =
                this.dataSetProcessingService.AddDataSetAsync(nullDataSet);

            DataSetProcessingValidationException actualDataSetProcessingValidationException =
                await Assert.ThrowsAsync<DataSetProcessingValidationException>(AddDataSetTask.AsTask);

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