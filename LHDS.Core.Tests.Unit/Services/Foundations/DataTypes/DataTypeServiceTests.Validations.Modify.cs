using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.DataTypes;
using LHDS.Core.Models.Foundations.DataTypes.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataTypes
{
    public partial class DataTypeServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfDataTypeIsNullAndLogItAsync()
        {
            // given
            DataType nullDataType = null;
            var nullDataTypeException = new NullDataTypeException(message: "DataType is null.");

            var expectedDataTypeValidationException =
                new DataTypeValidationException(
                    message: "DataType validation errors occurred, please try again.",
                    innerException: nullDataTypeException);

            // when
            ValueTask<DataType> modifyDataTypeTask =
                this.dataTypeService.ModifyDataTypeAsync(nullDataType);

            DataTypeValidationException actualDataTypeValidationException =
                await Assert.ThrowsAsync<DataTypeValidationException>(
                    modifyDataTypeTask.AsTask);

            // then
            actualDataTypeValidationException.Should()
                .BeEquivalentTo(expectedDataTypeValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataTypeValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataTypeAsync(It.IsAny<DataType>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfDataTypeIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            var invalidDataType = new DataType
            {
                // TODO:  Add default values for your properties i.e. Name = invalidText
            };

            var invalidDataTypeException = 
                new InvalidDataTypeException(
                        message: "Invalid dataType. Please correct the errors and try again.");

            invalidDataTypeException.AddData(
                key: nameof(DataType.Id),
                values: "Id is required");

            //invalidDataTypeException.AddData(
            //    key: nameof(DataType.Name),
            //    values: "Text is required");

            // TODO: Add or remove data here to suit the validation needs for the DataType model

            invalidDataTypeException.AddData(
                key: nameof(DataType.CreatedDate),
                values: "Date is required");

            invalidDataTypeException.AddData(
                key: nameof(DataType.CreatedBy),
                values: "Text is required");

            invalidDataTypeException.AddData(
                key: nameof(DataType.UpdatedDate),
                values: "Date is required");

            invalidDataTypeException.AddData(
                key: nameof(DataType.UpdatedBy),
                values: "Text is required");

            var expectedDataTypeValidationException =
                new DataTypeValidationException(
                    message: "DataType validation errors occurred, please try again.",
                    innerException: invalidDataTypeException);

            // when
            ValueTask<DataType> modifyDataTypeTask =
                this.dataTypeService.ModifyDataTypeAsync(invalidDataType);

            DataTypeValidationException actualDataTypeValidationException =
                await Assert.ThrowsAsync<DataTypeValidationException>(
                    modifyDataTypeTask.AsTask);

            //then
            actualDataTypeValidationException.Should().BeEquivalentTo(expectedDataTypeValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataTypeValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataTypeAsync(It.IsAny<DataType>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}