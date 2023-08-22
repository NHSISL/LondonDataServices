using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
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
            var nullDataTypeException = new NullDataTypeException();

            var expectedDataTypeValidationException =
                new DataTypeValidationException(nullDataTypeException);

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

            var invalidDataTypeException = new InvalidDataTypeException();

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
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(DataType.CreatedDate)}"
                });

            invalidDataTypeException.AddData(
                key: nameof(DataType.UpdatedBy),
                values: "Text is required");

            var expectedDataTypeValidationException =
                new DataTypeValidationException(invalidDataTypeException);

            // when
            ValueTask<DataType> modifyDataTypeTask =
                this.dataTypeService.ModifyDataTypeAsync(invalidDataType);

            DataTypeValidationException actualDataTypeValidationException =
                await Assert.ThrowsAsync<DataTypeValidationException>(
                    modifyDataTypeTask.AsTask);

            //then
            actualDataTypeValidationException.Should()
                .BeEquivalentTo(expectedDataTypeValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

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

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DataType randomDataType = CreateRandomDataType(randomDateTimeOffset);
            DataType invalidDataType = randomDataType;
            var invalidDataTypeException = new InvalidDataTypeException();

            invalidDataTypeException.AddData(
                key: nameof(DataType.UpdatedDate),
                values: $"Date is the same as {nameof(DataType.CreatedDate)}");

            var expectedDataTypeValidationException =
                new DataTypeValidationException(invalidDataTypeException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<DataType> modifyDataTypeTask =
                this.dataTypeService.ModifyDataTypeAsync(invalidDataType);

            DataTypeValidationException actualDataTypeValidationException =
                await Assert.ThrowsAsync<DataTypeValidationException>(
                    modifyDataTypeTask.AsTask);

            // then
            actualDataTypeValidationException.Should()
                .BeEquivalentTo(expectedDataTypeValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataTypeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataTypeByIdAsync(invalidDataType.Id),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(int minutes)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DataType randomDataType = CreateRandomDataType(randomDateTimeOffset);
            randomDataType.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var invalidDataTypeException =
                new InvalidDataTypeException();

            invalidDataTypeException.AddData(
                key: nameof(DataType.UpdatedDate),
                values: "Date is not recent");

            var expectedDataTypeValidatonException =
                new DataTypeValidationException(invalidDataTypeException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<DataType> modifyDataTypeTask =
                this.dataTypeService.ModifyDataTypeAsync(randomDataType);

            DataTypeValidationException actualDataTypeValidationException =
                await Assert.ThrowsAsync<DataTypeValidationException>(
                    modifyDataTypeTask.AsTask);

            // then
            actualDataTypeValidationException.Should()
                .BeEquivalentTo(expectedDataTypeValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataTypeValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataTypeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfDataTypeDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DataType randomDataType = CreateRandomModifyDataType(randomDateTimeOffset);
            DataType nonExistDataType = randomDataType;
            DataType nullDataType = null;

            var notFoundDataTypeException =
                new NotFoundDataTypeException(nonExistDataType.Id);

            var expectedDataTypeValidationException =
                new DataTypeValidationException(notFoundDataTypeException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataTypeByIdAsync(nonExistDataType.Id))
                .ReturnsAsync(nullDataType);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when 
            ValueTask<DataType> modifyDataTypeTask =
                this.dataTypeService.ModifyDataTypeAsync(nonExistDataType);

            DataTypeValidationException actualDataTypeValidationException =
                await Assert.ThrowsAsync<DataTypeValidationException>(
                    modifyDataTypeTask.AsTask);

            // then
            actualDataTypeValidationException.Should()
                .BeEquivalentTo(expectedDataTypeValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataTypeByIdAsync(nonExistDataType.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataTypeValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNegativeNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DataType randomDataType = CreateRandomModifyDataType(randomDateTimeOffset);
            DataType invalidDataType = randomDataType.DeepClone();
            DataType storageDataType = invalidDataType.DeepClone();
            storageDataType.CreatedDate = storageDataType.CreatedDate.AddMinutes(randomMinutes);
            storageDataType.UpdatedDate = storageDataType.UpdatedDate.AddMinutes(randomMinutes);
            var invalidDataTypeException = new InvalidDataTypeException();

            invalidDataTypeException.AddData(
                key: nameof(DataType.CreatedDate),
                values: $"Date is not the same as {nameof(DataType.CreatedDate)}");

            var expectedDataTypeValidationException =
                new DataTypeValidationException(invalidDataTypeException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataTypeByIdAsync(invalidDataType.Id))
                .ReturnsAsync(storageDataType);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<DataType> modifyDataTypeTask =
                this.dataTypeService.ModifyDataTypeAsync(invalidDataType);

            DataTypeValidationException actualDataTypeValidationException =
                await Assert.ThrowsAsync<DataTypeValidationException>(
                    modifyDataTypeTask.AsTask);

            // then
            actualDataTypeValidationException.Should()
                .BeEquivalentTo(expectedDataTypeValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataTypeByIdAsync(invalidDataType.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDataTypeValidationException))),
                       Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCreatedUserDontMacthStorageAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DataType randomDataType = CreateRandomModifyDataType(randomDateTimeOffset);
            DataType invalidDataType = randomDataType.DeepClone();
            DataType storageDataType = invalidDataType.DeepClone();
            invalidDataType.CreatedBy = Guid.NewGuid().ToString();
            storageDataType.UpdatedDate = storageDataType.CreatedDate;

            var invalidDataTypeException = new InvalidDataTypeException();

            invalidDataTypeException.AddData(
                key: nameof(DataType.CreatedBy),
                values: $"Text is not the same as {nameof(DataType.CreatedBy)}");

            var expectedDataTypeValidationException =
                new DataTypeValidationException(invalidDataTypeException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataTypeByIdAsync(invalidDataType.Id))
                .ReturnsAsync(storageDataType);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<DataType> modifyDataTypeTask =
                this.dataTypeService.ModifyDataTypeAsync(invalidDataType);

            DataTypeValidationException actualDataTypeValidationException =
                await Assert.ThrowsAsync<DataTypeValidationException>(
                    modifyDataTypeTask.AsTask);

            // then
            actualDataTypeValidationException.Should().BeEquivalentTo(expectedDataTypeValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataTypeByIdAsync(invalidDataType.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDataTypeValidationException))),
                       Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}