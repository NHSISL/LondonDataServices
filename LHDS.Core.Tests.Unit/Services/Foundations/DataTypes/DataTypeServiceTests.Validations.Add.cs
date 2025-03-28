// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
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
        public async Task ShouldThrowValidationExceptionOnAddIfDataTypeIsNullAndLogItAsync()
        {
            // given
            DataType nullDataType = null;

            var nullDataTypeException =
                new NullDataTypeException(message: "DataType is null.");

            var expectedDataTypeValidationException =
                new DataTypeValidationException(
                    message: "DataType validation errors occurred, please try again.",
                    innerException: nullDataTypeException);

            // when
            ValueTask<DataType> addDataTypeTask =
                this.dataTypeService.AddDataTypeAsync(nullDataType);

            DataTypeValidationException actualDataTypeValidationException =
                await Assert.ThrowsAsync<DataTypeValidationException>(addDataTypeTask.AsTask);

            // then
            actualDataTypeValidationException.Should()
                .BeEquivalentTo(expectedDataTypeValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
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
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            var invalidDataType = new DataType
            {
                Name = invalidText
            };

            var dataTypeServiceMock = new Mock<DataTypeService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            dataTypeServiceMock.Setup(service =>
                service.ApplyAddAuditAsync(invalidDataType))
                    .ReturnsAsync(invalidDataType);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

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
                values:
                [
                    "Date is required",
                    "Date is not recent"
                ]);

            invalidDataTypeException.AddData(
                key: nameof(DataType.CreatedBy),
                values:
                [
                    "Text is required",
                    $"Expected value to be '{randomEntraUser.EntraUserId}' but found '{invalidDataType.CreatedBy}'."
                ]);

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
            ValueTask<DataType> addDataTypeTask =
                dataTypeServiceMock.Object.AddDataTypeAsync(invalidDataType);

            DataTypeValidationException actualDataTypeValidationException =
                await Assert.ThrowsAsync<DataTypeValidationException>(addDataTypeTask.AsTask);

            // then
            actualDataTypeValidationException.Should()
                .BeEquivalentTo(expectedDataTypeValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataTypeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataTypeAsync(It.IsAny<DataType>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfDataTypePropertyLengthsIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();
            DataType randomDataType = CreateRandomDataType(randomDateTimeOffset, randomEntraUser.EntraUserId);
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

            var dataTypeServiceMock = new Mock<DataTypeService>(
                 storageBrokerMock.Object,
                 dateTimeBrokerMock.Object,
                 securityBrokerMock.Object,
                 loggingBrokerMock.Object)
            {
                CallBase = true
            };

            dataTypeServiceMock.Setup(service =>
                service.ApplyAddAuditAsync(invalidDataType))
                    .ReturnsAsync(invalidDataType);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<DataType> addDataTypeTask =
                dataTypeServiceMock.Object.AddDataTypeAsync(invalidDataType);

            DataTypeValidationException actualDataTypeValidationException =
                await Assert.ThrowsAsync<DataTypeValidationException>(addDataTypeTask.AsTask);

            // then
            actualDataTypeValidationException.Should()
                .BeEquivalentTo(expectedDataTypeValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataTypeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataTypeAsync(It.IsAny<DataType>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();
            DataType randomDataType = CreateRandomDataType(randomDateTimeOffset, randomEntraUser.EntraUserId);
            DataType invalidDataType = randomDataType;

            invalidDataType.UpdatedDate =
                invalidDataType.CreatedDate.AddDays(randomNumber);

            var invalidDataTypeException =
                new InvalidDataTypeException(
                    message: "Invalid dataType. Please correct the errors and try again.");

            invalidDataTypeException.AddData(
                key: nameof(DataType.UpdatedDate),
                values: $"Date is not the same as {nameof(DataType.CreatedDate)}");

            var expectedDataTypeValidationException =
                new DataTypeValidationException(
                    message: "DataType validation errors occurred, please try again.",
                    innerException: invalidDataTypeException);

            var dataTypeServiceMock = new Mock<DataTypeService>(
                 storageBrokerMock.Object,
                 dateTimeBrokerMock.Object,
                 securityBrokerMock.Object,
                 loggingBrokerMock.Object)
            {
                CallBase = true
            };

            dataTypeServiceMock.Setup(service =>
                service.ApplyAddAuditAsync(invalidDataType))
                    .ReturnsAsync(invalidDataType);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<DataType> addDataTypeTask =
                dataTypeServiceMock.Object.AddDataTypeAsync(invalidDataType);

            DataTypeValidationException actualDataTypeValidationException =
                await Assert.ThrowsAsync<DataTypeValidationException>(addDataTypeTask.AsTask);

            // then
            actualDataTypeValidationException.Should()
                .BeEquivalentTo(expectedDataTypeValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataTypeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataTypeAsync(It.IsAny<DataType>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateUsersIsNotSameAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();
            DataType randomDataType = CreateRandomDataType(randomDateTimeOffset, randomEntraUser.EntraUserId);
            DataType invalidDataType = randomDataType;
            invalidDataType.UpdatedBy = Guid.NewGuid().ToString();

            var invalidDataTypeException =
                new InvalidDataTypeException(
                    message: "Invalid dataType. Please correct the errors and try again.");

            invalidDataTypeException.AddData(
                key: nameof(DataType.UpdatedBy),
                values: $"Text is not the same as {nameof(DataType.CreatedBy)}");

            var expectedDataTypeValidationException =
                new DataTypeValidationException(
                    message: "DataType validation errors occurred, please try again.",
                    innerException: invalidDataTypeException);

            var dataTypeServiceMock = new Mock<DataTypeService>(
                 storageBrokerMock.Object,
                 dateTimeBrokerMock.Object,
                 securityBrokerMock.Object,
                 loggingBrokerMock.Object)
            {
                CallBase = true
            };

            dataTypeServiceMock.Setup(service =>
                service.ApplyAddAuditAsync(invalidDataType))
                    .ReturnsAsync(invalidDataType);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<DataType> addDataTypeTask =
                dataTypeServiceMock.Object.AddDataTypeAsync(invalidDataType);

            DataTypeValidationException actualDataTypeValidationException =
                await Assert.ThrowsAsync<DataTypeValidationException>(addDataTypeTask.AsTask);

            // then
            actualDataTypeValidationException.Should()
                .BeEquivalentTo(expectedDataTypeValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataTypeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataTypeAsync(It.IsAny<DataType>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync(
            int minutesBeforeOrAfter)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            DateTimeOffset invalidDateTime =
                randomDateTimeOffset.AddMinutes(minutesBeforeOrAfter);

            DataType randomDataType = CreateRandomDataType(invalidDateTime, randomEntraUser.EntraUserId);
            DataType invalidDataType = randomDataType;

            var invalidDataTypeException =
                new InvalidDataTypeException(
                    message: "Invalid dataType. Please correct the errors and try again.");

            invalidDataTypeException.AddData(
                key: nameof(DataType.CreatedDate),
                values: "Date is not recent");

            var expectedDataTypeValidationException =
                new DataTypeValidationException(
                    message: "DataType validation errors occurred, please try again.",
                    innerException: invalidDataTypeException);

            var dataTypeServiceMock = new Mock<DataTypeService>(
                 storageBrokerMock.Object,
                 dateTimeBrokerMock.Object,
                 securityBrokerMock.Object,
                 loggingBrokerMock.Object)
            {
                CallBase = true
            };

            dataTypeServiceMock.Setup(service =>
                service.ApplyAddAuditAsync(invalidDataType))
                    .ReturnsAsync(invalidDataType);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<DataType> addDataTypeTask =
                dataTypeServiceMock.Object.AddDataTypeAsync(invalidDataType);

            DataTypeValidationException actualDataTypeValidationException =
                await Assert.ThrowsAsync<DataTypeValidationException>(addDataTypeTask.AsTask);

            // then
            actualDataTypeValidationException.Should()
                .BeEquivalentTo(expectedDataTypeValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataTypeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataTypeAsync(It.IsAny<DataType>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}