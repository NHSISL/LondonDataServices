// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.DataTypes;
using LHDS.Core.Models.Foundations.DataTypes.Exceptions;
using LHDS.Core.Services.Foundations.DataTypes;
using Moq;
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

            this.securityAuditBrokerMock.Setup(broker =>
				broker.ApplyModifyAuditValuesAsync(nullDataType))
					.ThrowsAsync(nullDataTypeException);

			// when
			ValueTask<DataType> modifyDataTypeTask =
                this.dataTypeService.ModifyDataTypeAsync(nullDataType);

            DataTypeValidationException actualDataTypeValidationException =
                await Assert.ThrowsAsync<DataTypeValidationException>(
                    modifyDataTypeTask.AsTask);

            // then
            actualDataTypeValidationException.Should()
                .BeEquivalentTo(expectedDataTypeValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(nullDataType),
				    Times.Once);

			this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataTypeValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataTypeAsync(It.IsAny<DataType>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
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
            DateTimeOffset randomDateOffset = GetRandomDateTimeOffset();
            string randomEntraUserId = GetRandomStringWithLengthOf(50);

            var invalidDataType = new DataType
            {
                Name = invalidText,
                CreatedBy = invalidText,
                UpdatedBy = invalidText
            };

            var invalidDataTypeException =
                new InvalidDataTypeException(
                    message: "Invalid dataType. Please correct the errors and try again.");

            invalidDataTypeException.AddData(
                key: nameof(DataType.Id),
                values: "Id is required");

            invalidDataTypeException.AddData(
                key: nameof(DataType.Name),
                values: "Text is required");

            invalidDataTypeException.AddData(
                key: nameof(DataType.CreatedDate),
                values: "Date is required");

            invalidDataTypeException.AddData(
                key: nameof(DataType.CreatedBy),
                values: "Text is required");

            invalidDataTypeException.AddData(
                key: nameof(DataType.UpdatedDate),
                values:
                    [
                        "Date is required",
                        "Date is the same as CreatedDate",
                        "Date is not recent"
                    ]);

            invalidDataTypeException.AddData(
                key: nameof(DataType.UpdatedBy),
                values:
                    [
                        "Text is required",
                        $"Expected value to be '{randomEntraUserId}' but found " +
                        $"'{invalidDataType.UpdatedBy}'."
                    ]);

            var expectedDataTypeValidationException =
                new DataTypeValidationException(
                    message: "DataType validation errors occurred, please try again.",
                    innerException: invalidDataTypeException);

            securityAuditBrokerMock.Setup(service =>
                service.ApplyModifyAuditValuesAsync(invalidDataType))
                    .ReturnsAsync(invalidDataType);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            // when
            ValueTask<DataType> modifyDataTypeTask =
                dataTypeService.ModifyDataTypeAsync(invalidDataType);

            DataTypeValidationException actualDataTypeValidationException =
                await Assert.ThrowsAsync<DataTypeValidationException>(
                    modifyDataTypeTask.AsTask);

            //then
            actualDataTypeValidationException.Should()
                .BeEquivalentTo(expectedDataTypeValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidDataType),
				    Times.Once);

			this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataTypeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataTypeAsync(It.IsAny<DataType>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfDataTypePropertyLengthsIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomEntraUserId = GetRandomStringWithLengthOf(50);
            DataType randomDataType = CreateRandomModifyDataType(randomDateTimeOffset, randomEntraUserId);
            randomDataType.Name = GetRandomString(51);
            var invalidDataType = randomDataType;

            var invalidDataTypeException =
                new InvalidDataTypeException(
                    message: "Invalid dataType. Please correct the errors and try again.");

            invalidDataTypeException.AddData(
                key: nameof(DataType.Name),
                values: "Text is exceeding max length");

            var expectedDataTypeValidationException =
                new DataTypeValidationException(
                    message: "DataType validation errors occurred, please try again.",
                    innerException: invalidDataTypeException);

            securityAuditBrokerMock.Setup(service =>
                service.ApplyModifyAuditValuesAsync(invalidDataType))
                    .ReturnsAsync(invalidDataType);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            // when
            ValueTask<DataType> modifyDataTypeTask =
                dataTypeService.ModifyDataTypeAsync(invalidDataType);

            DataTypeValidationException actualDataTypeValidationException =
                await Assert.ThrowsAsync<DataTypeValidationException>(
                    modifyDataTypeTask.AsTask);

            //then
            actualDataTypeValidationException.Should()
                .BeEquivalentTo(expectedDataTypeValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
				broker.ApplyModifyAuditValuesAsync(invalidDataType),
				    Times.Once);

			this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataTypeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataTypeAsync(It.IsAny<DataType>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomEntraUserId = GetRandomStringWithLengthOf(50);
            DataType randomDataType = CreateRandomDataType(randomDateTimeOffset, randomEntraUserId);
            DataType invalidDataType = randomDataType;

            var invalidDataTypeException =
                new InvalidDataTypeException(
                    message: "Invalid dataType. Please correct the errors and try again.");

            invalidDataTypeException.AddData(
                key: nameof(DataType.UpdatedDate),
                values: $"Date is the same as {nameof(DataType.CreatedDate)}");

            var expectedDataTypeValidationException =
                new DataTypeValidationException(
                    message: "DataType validation errors occurred, please try again.",
                    innerException: invalidDataTypeException);

            securityAuditBrokerMock.Setup(service =>
                service.ApplyModifyAuditValuesAsync(invalidDataType))
                    .ReturnsAsync(invalidDataType);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            // when
            ValueTask<DataType> modifyDataTypeTask =
                dataTypeService.ModifyDataTypeAsync(invalidDataType);

            DataTypeValidationException actualDataTypeValidationException =
                await Assert.ThrowsAsync<DataTypeValidationException>(
                    modifyDataTypeTask.AsTask);

            // then
            actualDataTypeValidationException.Should()
                .BeEquivalentTo(expectedDataTypeValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidDataType),
					Times.Once);

			this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataTypeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataTypeByIdAsync(invalidDataType.Id),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
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
            string randomEntraUserId = GetRandomStringWithLengthOf(50);
            DataType randomDataType = CreateRandomDataType(randomDateTimeOffset, randomEntraUserId);
            randomDataType.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var invalidDataTypeException =
                new InvalidDataTypeException(
                    message: "Invalid dataType. Please correct the errors and try again.");

            invalidDataTypeException.AddData(
                key: nameof(DataType.UpdatedDate),
                values: "Date is not recent");

            var expectedDataTypeValidatonException =
                new DataTypeValidationException(
                    message: "DataType validation errors occurred, please try again.",
                    innerException: invalidDataTypeException);

            securityAuditBrokerMock.Setup(service =>
                service.ApplyModifyAuditValuesAsync(randomDataType))
                    .ReturnsAsync(randomDataType);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            // when
            ValueTask<DataType> modifyDataTypeTask =
                dataTypeService.ModifyDataTypeAsync(randomDataType);

            DataTypeValidationException actualDataTypeValidationException =
                await Assert.ThrowsAsync<DataTypeValidationException>(
                    modifyDataTypeTask.AsTask);

            // then
            actualDataTypeValidationException.Should()
                .BeEquivalentTo(expectedDataTypeValidatonException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(randomDataType),
					Times.Once);

			this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataTypeValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataTypeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfDataTypeDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomEntraUserId = GetRandomStringWithLengthOf(50);
            DataType randomDataType = CreateRandomModifyDataType(randomDateTimeOffset, randomEntraUserId);
            DataType nonExistDataType = randomDataType;
            DataType nullDataType = null;

            var notFoundDataTypeException =
                new NotFoundDataTypeException(nonExistDataType.Id);

            var expectedDataTypeValidationException =
                new DataTypeValidationException(
                    message: "DataType validation errors occurred, please try again.",
                    innerException: notFoundDataTypeException);

            securityAuditBrokerMock.Setup(service =>
                service.ApplyModifyAuditValuesAsync(nonExistDataType))
                    .ReturnsAsync(nonExistDataType);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataTypeByIdAsync(nonExistDataType.Id))
                .ReturnsAsync(nullDataType);

            // when
            ValueTask<DataType> modifyDataTypeTask =
                dataTypeService.ModifyDataTypeAsync(nonExistDataType);

            DataTypeValidationException actualDataTypeValidationException =
                await Assert.ThrowsAsync<DataTypeValidationException>(
                    modifyDataTypeTask.AsTask);

            // then
            actualDataTypeValidationException.Should()
                .BeEquivalentTo(expectedDataTypeValidationException);

            this.securityAuditBrokerMock.Verify(Brokers =>
				Brokers.ApplyModifyAuditValuesAsync(nonExistDataType),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
                broker.SelectDataTypeByIdAsync(nonExistDataType.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataTypeValidationException))),
                        Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
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
            string randomEntraUserId = GetRandomStringWithLengthOf(50);
            DataType randomDataType = CreateRandomModifyDataType(randomDateTimeOffset, randomEntraUserId);
            DataType invalidDataType = randomDataType.DeepClone();
            DataType storageDataType = invalidDataType.DeepClone();
            storageDataType.CreatedDate = storageDataType.CreatedDate.AddMinutes(randomMinutes);
            storageDataType.UpdatedDate = storageDataType.UpdatedDate.AddMinutes(randomMinutes);

            var invalidDataTypeException =
                new InvalidDataTypeException(
                    message: "Invalid dataType. Please correct the errors and try again.");

            invalidDataTypeException.AddData(
                key: nameof(DataType.CreatedDate),
                values: $"Date is not the same as {nameof(DataType.CreatedDate)}");

            var expectedDataTypeValidationException =
                new DataTypeValidationException(
                    message: "DataType validation errors occurred, please try again.",
                    innerException: invalidDataTypeException);

            securityAuditBrokerMock.Setup(service =>
                service.ApplyModifyAuditValuesAsync(invalidDataType))
                    .ReturnsAsync(invalidDataType);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataTypeByIdAsync(invalidDataType.Id))
                .ReturnsAsync(storageDataType);

            // when
            ValueTask<DataType> modifyDataTypeTask =
                dataTypeService.ModifyDataTypeAsync(invalidDataType);

            DataTypeValidationException actualDataTypeValidationException =
                await Assert.ThrowsAsync<DataTypeValidationException>(
                    modifyDataTypeTask.AsTask);

            // then
            actualDataTypeValidationException.Should()
                .BeEquivalentTo(expectedDataTypeValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidDataType),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
                broker.SelectDataTypeByIdAsync(invalidDataType.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDataTypeValidationException))),
                       Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCreatedUserDontMatchStorageAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomEntraUserId = GetRandomStringWithLengthOf(50);
            DataType randomDataType = CreateRandomModifyDataType(randomDateTimeOffset, randomEntraUserId);
            DataType invalidDataType = randomDataType.DeepClone();
            DataType storageDataType = invalidDataType.DeepClone();
            invalidDataType.CreatedBy = Guid.NewGuid().ToString();
            storageDataType.UpdatedDate = storageDataType.CreatedDate;

            var invalidDataTypeException =
                new InvalidDataTypeException(
                    message: "Invalid dataType. Please correct the errors and try again.");

            invalidDataTypeException.AddData(
                key: nameof(DataType.CreatedBy),
                values: $"Text is not the same as {nameof(DataType.CreatedBy)}");

            var expectedDataTypeValidationException =
                new DataTypeValidationException(
                    message: "DataType validation errors occurred, please try again.",
                    innerException: invalidDataTypeException);

            securityAuditBrokerMock.Setup(service =>
                service.ApplyModifyAuditValuesAsync(invalidDataType))
                    .ReturnsAsync(invalidDataType);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataTypeByIdAsync(invalidDataType.Id))
                .ReturnsAsync(storageDataType);

            // when
            ValueTask<DataType> modifyDataTypeTask =
                dataTypeService.ModifyDataTypeAsync(invalidDataType);

            DataTypeValidationException actualDataTypeValidationException =
                await Assert.ThrowsAsync<DataTypeValidationException>(
                    modifyDataTypeTask.AsTask);

            // then
            actualDataTypeValidationException.Should().BeEquivalentTo(expectedDataTypeValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
				broker.ApplyModifyAuditValuesAsync(invalidDataType),
				    Times.Once);

			this.storageBrokerMock.Verify(broker =>
                broker.SelectDataTypeByIdAsync(invalidDataType.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDataTypeValidationException))),
                       Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomEntraUserId = GetRandomStringWithLengthOf(50);
            DataType randomDataType = CreateRandomModifyDataType(randomDateTimeOffset, randomEntraUserId);
            DataType invalidDataType = randomDataType;
            DataType storageDataType = randomDataType.DeepClone();

            var invalidDataTypeException =
                new InvalidDataTypeException(
                    message: "Invalid dataType. Please correct the errors and try again.");

            invalidDataTypeException.AddData(
                key: nameof(DataType.UpdatedDate),
                values: $"Date is the same as {nameof(DataType.UpdatedDate)}");

            var expectedDataTypeValidationException =
                new DataTypeValidationException(
                    message: "DataType validation errors occurred, please try again.",
                    innerException: invalidDataTypeException);

            securityAuditBrokerMock.Setup(service =>
                service.ApplyModifyAuditValuesAsync(invalidDataType))
                    .ReturnsAsync(invalidDataType);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataTypeByIdAsync(invalidDataType.Id))
                .ReturnsAsync(storageDataType);

            // when
            ValueTask<DataType> modifyDataTypeTask =
                dataTypeService.ModifyDataTypeAsync(invalidDataType);

            // then
            await Assert.ThrowsAsync<DataTypeValidationException>(
                modifyDataTypeTask.AsTask);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidDataType),
				    Times.Once);

			this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataTypeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataTypeByIdAsync(invalidDataType.Id),
                    Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}