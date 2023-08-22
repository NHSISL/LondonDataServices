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
        public async Task ShouldThrowValidationExceptionOnAddIfDataTypeIsNullAndLogItAsync()
        {
            // given
            DataType nullDataType = null;

            var nullDataTypeException =
                new NullDataTypeException();

            var expectedDataTypeValidationException =
                new DataTypeValidationException(nullDataTypeException);

            // when
            ValueTask<DataType> addDataTypeTask =
                this.dataTypeService.AddDataTypeAsync(nullDataType);

            DataTypeValidationException actualDataTypeValidationException =
                await Assert.ThrowsAsync<DataTypeValidationException>(
                    addDataTypeTask.AsTask);

            // then
            actualDataTypeValidationException.Should()
                .BeEquivalentTo(expectedDataTypeValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataTypeValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfDataTypeIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            var invalidDataType = new DataType
            {
                // TODO:  Add default values for your properties i.e. Name = invalidText
            };

            var invalidDataTypeException =
                new InvalidDataTypeException();

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
                new DataTypeValidationException(invalidDataTypeException);

            // when
            ValueTask<DataType> addDataTypeTask =
                this.dataTypeService.AddDataTypeAsync(invalidDataType);

            DataTypeValidationException actualDataTypeValidationException =
                await Assert.ThrowsAsync<DataTypeValidationException>(
                    addDataTypeTask.AsTask);

            // then
            actualDataTypeValidationException.Should()
                .BeEquivalentTo(expectedDataTypeValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataTypeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataTypeAsync(It.IsAny<DataType>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}