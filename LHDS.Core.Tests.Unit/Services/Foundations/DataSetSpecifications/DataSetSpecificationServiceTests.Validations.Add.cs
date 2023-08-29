using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSetSpecifications
{
    public partial class DataSetSpecificationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfDataSetSpecificationIsNullAndLogItAsync()
        {
            // given
            DataSetSpecification nullDataSetSpecification = null;

            var nullDataSetSpecificationException =
                new NullDataSetSpecificationException(message: "DataSetSpecification is null.");

            var expectedDataSetSpecificationValidationException =
                new DataSetSpecificationValidationException(
                    message: "DataSetSpecification validation errors occurred, please try again.",
                    innerException: nullDataSetSpecificationException);

            // when
            ValueTask<DataSetSpecification> addDataSetSpecificationTask =
                this.dataSetSpecificationService.AddDataSetSpecificationAsync(nullDataSetSpecification);

            DataSetSpecificationValidationException actualDataSetSpecificationValidationException =
                await Assert.ThrowsAsync<DataSetSpecificationValidationException>(
                    addDataSetSpecificationTask.AsTask);

            // then
            actualDataSetSpecificationValidationException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfDataSetSpecificationIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            var invalidDataSetSpecification = new DataSetSpecification
            {
                // TODO:  Add default values for your properties i.e. Name = invalidText
            };

            var invalidDataSetSpecificationException =
                new InvalidDataSetSpecificationException(
                    message: "Invalid dataSetSpecification. Please correct the errors and try again.");

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.Id),
                values: "Id is required");

            //invalidDataSetSpecificationException.AddData(
            //    key: nameof(DataSetSpecification.Name),
            //    values: "Text is required");

            // TODO: Add or remove data here to suit the validation needs for the DataSetSpecification model

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.CreatedDate),
                values: "Date is required");

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.CreatedBy),
                values: "Text is required");

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.UpdatedDate),
                values: "Date is required");

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.UpdatedBy),
                values: "Text is required");

            var expectedDataSetSpecificationValidationException =
                new DataSetSpecificationValidationException(
                    message: "DataSetSpecification validation errors occurred, please try again.",
                    innerException: invalidDataSetSpecificationException);

            // when
            ValueTask<DataSetSpecification> addDataSetSpecificationTask =
                this.dataSetSpecificationService.AddDataSetSpecificationAsync(invalidDataSetSpecification);

            DataSetSpecificationValidationException actualDataSetSpecificationValidationException =
                await Assert.ThrowsAsync<DataSetSpecificationValidationException>(
                    addDataSetSpecificationTask.AsTask);

            // then
            actualDataSetSpecificationValidationException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DataSetSpecification randomDataSetSpecification = CreateRandomDataSetSpecification(randomDateTimeOffset);
            DataSetSpecification invalidDataSetSpecification = randomDataSetSpecification;

            invalidDataSetSpecification.UpdatedDate =
                invalidDataSetSpecification.CreatedDate.AddDays(randomNumber);

            var invalidDataSetSpecificationException = 
                new InvalidDataSetSpecificationException(
                    message: "Invalid dataSetSpecification. Please correct the errors and try again.");

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.UpdatedDate),
                values: $"Date is not the same as {nameof(DataSetSpecification.CreatedDate)}");

            var expectedDataSetSpecificationValidationException =
                new DataSetSpecificationValidationException(
                    message: "DataSetSpecification validation errors occurred, please try again.",
                    innerException: invalidDataSetSpecificationException);

            // when
            ValueTask<DataSetSpecification> addDataSetSpecificationTask =
                this.dataSetSpecificationService.AddDataSetSpecificationAsync(invalidDataSetSpecification);

            DataSetSpecificationValidationException actualDataSetSpecificationValidationException =
                await Assert.ThrowsAsync<DataSetSpecificationValidationException>(
                    addDataSetSpecificationTask.AsTask);

            // then
            actualDataSetSpecificationValidationException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}