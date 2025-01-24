// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Processings.DataSetSpecifications.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.DataSetSpecifications
{
    public partial class DataSetSpecificationProcessingServiceTests
    {
        [Fact]
        public async Task
            ShouldThrowValidationExceptionsOnModifyOrAddIfDataSetSpecificationProcessingIsNullAndLogItAsync()
        {
            // given
            DataSetSpecification nullDataSetSpecification = null;

            var nullDataSetSpecificationProcessingException =
                new NullDataSetSpecificationProcessingException(message: "DataSetSpecification is null.");

            var expectedDataSetSpecificationProcessingValidationException =
                new DataSetSpecificationProcessingValidationException(
                    message: "DataSetSpecification processing validation error occurred, please try again.",
                    innerException: nullDataSetSpecificationProcessingException);

            // when
            ValueTask<DataSetSpecification> AddDataSetSpecificationTask =
                this.dataSetSpecificationProcessingService
                    .ModifyOrAddDataSetSpecificationAsync(nullDataSetSpecification);

            DataSetSpecificationProcessingValidationException actualDataSetSpecificationProcessingValidationException =
                await Assert.ThrowsAsync<DataSetSpecificationProcessingValidationException>(
                    AddDataSetSpecificationTask.AsTask);

            //then
            actualDataSetSpecificationProcessingValidationException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationProcessingValidationException))),
                        Times.Once);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionsOnModifyOrAddIfIdIsInvalidAndLogItAsync()
        {
            // given
            DataSetSpecification invalidDataSetSpecification = new DataSetSpecification();

            var invalidArgumentDataSetSpecificationProcessingException =
                new InvalidArgumentDataSetSpecificationProcessingException(
                    message: "Invalid argument(s). Please correct the errors and try again.");

            invalidArgumentDataSetSpecificationProcessingException.AddData(
                key: "Id",
                values: "Id is required");

            var expectedDataSetSpecificationProcessingValidationException =
                new DataSetSpecificationProcessingValidationException(
                    message: "DataSetSpecification processing validation error occurred, please try again.",
                    innerException: invalidArgumentDataSetSpecificationProcessingException);

            // when
            ValueTask<DataSetSpecification> RetrieveDataSetSpecificationTask =
                this.dataSetSpecificationProcessingService
                    .ModifyOrAddDataSetSpecificationAsync(invalidDataSetSpecification);

            DataSetSpecificationProcessingValidationException actualDataSetSpecificationProcessingValidationException =
                await Assert.ThrowsAsync<DataSetSpecificationProcessingValidationException>(
                    RetrieveDataSetSpecificationTask.AsTask);

            //then
            actualDataSetSpecificationProcessingValidationException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationProcessingValidationException))),
                        Times.Once);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
