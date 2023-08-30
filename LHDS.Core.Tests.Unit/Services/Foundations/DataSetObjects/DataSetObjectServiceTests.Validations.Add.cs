// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataSetObjects;
using LHDS.Core.Models.Foundations.DataSetObjects.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSetObjects
{
    public partial class DataSetObjectServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfDataSetObjectIsNullAndLogItAsync()
        {
            // given
            DataSetObject nullDataSetObject = null;

            var nullDataSetObjectException =
                new NullDataSetObjectException(message: "DataSetObject is null.");

            var expectedDataSetObjectValidationException =
                new DataSetObjectValidationException(
                    message: "DataSetObject validation errors occurred, please try again.",
                    innerException: nullDataSetObjectException);

            // when
            ValueTask<DataSetObject> addDataSetObjectTask =
                this.dataSetObjectService.AddDataSetObjectAsync(nullDataSetObject);

            DataSetObjectValidationException actualDataSetObjectValidationException =
                await Assert.ThrowsAsync<DataSetObjectValidationException>(() =>
                    addDataSetObjectTask.AsTask());

            // then
            actualDataSetObjectValidationException.Should()
                .BeEquivalentTo(expectedDataSetObjectValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetObjectValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfDataSetObjectIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            var invalidDataSetObject = new DataSetObject
            {
                SupplierObjectName = invalidText,
                OurObjectName = invalidText,
                PushOrPull = invalidText,
            };

            var invalidDataSetObjectException =
                new InvalidDataSetObjectException(
                    message: "Invalid dataSetObject. Please correct the errors and try again.");

            invalidDataSetObjectException.AddData(
                key: nameof(DataSetObject.Id),
                values: "Id is required");

            invalidDataSetObjectException.AddData(
               key: nameof(DataSetObject.DataSetSpecificationId),
               values: "Id is required");

            invalidDataSetObjectException.AddData(
               key: nameof(DataSetObject.SupplierObjectName),
               values: "Text is required");

            invalidDataSetObjectException.AddData(
               key: nameof(DataSetObject.OurObjectName),
               values: "Text is required");

            invalidDataSetObjectException.AddData(
               key: nameof(DataSetObject.PushOrPull),
               values: "Text is required");

            invalidDataSetObjectException.AddData(
                key: nameof(DataSetObject.CreatedDate),
                values: "Date is required");

            invalidDataSetObjectException.AddData(
                key: nameof(DataSetObject.CreatedBy),
                values: "Text is required");

            invalidDataSetObjectException.AddData(
                key: nameof(DataSetObject.UpdatedDate),
                values: "Date is required");

            invalidDataSetObjectException.AddData(
                key: nameof(DataSetObject.UpdatedBy),
                values: "Text is required");

            var expectedDataSetObjectValidationException =
                new DataSetObjectValidationException(
                    message: "DataSetObject validation errors occurred, please try again.",
                    innerException: invalidDataSetObjectException);

            // when
            ValueTask<DataSetObject> addDataSetObjectTask =
                this.dataSetObjectService.AddDataSetObjectAsync(invalidDataSetObject);

            DataSetObjectValidationException actualDataSetObjectValidationException =
                await Assert.ThrowsAsync<DataSetObjectValidationException>(() =>
                    addDataSetObjectTask.AsTask());

            // then
            actualDataSetObjectValidationException.Should()
                .BeEquivalentTo(expectedDataSetObjectValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetObjectValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetObjectAsync(It.IsAny<DataSetObject>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfObjectColumnsIsInvalidLenghtAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DataSetObject invalidDataSetObject = CreateRandomDataSetObject(randomDateTimeOffset);
            invalidDataSetObject.SupplierObjectName = GetRandomString(256);
            invalidDataSetObject.OurObjectName = GetRandomString(256);
            invalidDataSetObject.PushOrPull = GetRandomString(11);
            invalidDataSetObject.CreatedBy = GetRandomString(256);
            invalidDataSetObject.UpdatedBy = invalidDataSetObject.CreatedBy;

            var invalidDataSetObjectException =
                new InvalidDataSetObjectException(
                    message: "Invalid dataSetObject. Please correct the errors and try again.");

            invalidDataSetObjectException.AddData(
                key: nameof(DataSetObject.SupplierObjectName),
                values: "Text is exceeding max length");

            invalidDataSetObjectException.AddData(
                 key: nameof(DataSetObject.OurObjectName),
                 values: "Text is exceeding max length");

            invalidDataSetObjectException.AddData(
                 key: nameof(DataSetObject.PushOrPull),
                 values: "Text is exceeding max length");

            invalidDataSetObjectException.AddData(
                key: nameof(DataSetObject.CreatedBy),
                values: "Text is exceeding max length");

            invalidDataSetObjectException.AddData(
                key: nameof(DataSetObject.UpdatedBy),
                values: "Text is exceeding max length");

            var expectedDataSetObjectValidationException =
                new DataSetObjectValidationException(
                    message: "DataSetObject validation errors occurred, please try again.",
                    innerException: invalidDataSetObjectException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<DataSetObject> addDataSetObjectTask =
                this.dataSetObjectService.AddDataSetObjectAsync(invalidDataSetObject);

            DataSetObjectValidationException actualDataSetObjectValidationException =
                await Assert.ThrowsAsync<DataSetObjectValidationException>(() =>
                    addDataSetObjectTask.AsTask());

            // then
            actualDataSetObjectValidationException.Should()
                .BeEquivalentTo(expectedDataSetObjectValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetObjectValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetObjectAsync(It.IsAny<DataSetObject>()),
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
            DataSetObject randomDataSetObject = CreateRandomDataSetObject(randomDateTimeOffset);
            DataSetObject invalidDataSetObject = randomDataSetObject;

            invalidDataSetObject.UpdatedDate =
                invalidDataSetObject.CreatedDate.AddDays(randomNumber);

            var invalidDataSetObjectException =
                new InvalidDataSetObjectException(
                    message: "Invalid dataSetObject. Please correct the errors and try again.");

            invalidDataSetObjectException.AddData(
                key: nameof(DataSetObject.UpdatedDate),
                values: $"Date is not the same as {nameof(DataSetObject.CreatedDate)}");

            var expectedDataSetObjectValidationException =
                new DataSetObjectValidationException(
                    message: "DataSetObject validation errors occurred, please try again.",
                    innerException: invalidDataSetObjectException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<DataSetObject> addDataSetObjectTask =
                this.dataSetObjectService.AddDataSetObjectAsync(invalidDataSetObject);

            DataSetObjectValidationException actualDataSetObjectValidationException =
                await Assert.ThrowsAsync<DataSetObjectValidationException>(() =>
                    addDataSetObjectTask.AsTask());

            // then
            actualDataSetObjectValidationException.Should()
                .BeEquivalentTo(expectedDataSetObjectValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetObjectValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetObjectAsync(It.IsAny<DataSetObject>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateUsersIsNotSameAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DataSetObject randomDataSetObject = CreateRandomDataSetObject(randomDateTimeOffset);
            DataSetObject invalidDataSetObject = randomDataSetObject;
            invalidDataSetObject.UpdatedBy = Guid.NewGuid().ToString();

            var invalidDataSetObjectException =
                new InvalidDataSetObjectException(
                    message: "Invalid dataSetObject. Please correct the errors and try again.");

            invalidDataSetObjectException.AddData(
                key: nameof(DataSetObject.UpdatedBy),
                values: $"Text is not the same as {nameof(DataSetObject.CreatedBy)}");

            var expectedDataSetObjectValidationException =
                new DataSetObjectValidationException(
                    message: "DataSetObject validation errors occurred, please try again.",
                    innerException: invalidDataSetObjectException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<DataSetObject> addDataSetObjectTask =
                this.dataSetObjectService.AddDataSetObjectAsync(invalidDataSetObject);

            DataSetObjectValidationException actualDataSetObjectValidationException =
                await Assert.ThrowsAsync<DataSetObjectValidationException>(() =>
                    addDataSetObjectTask.AsTask());

            // then
            actualDataSetObjectValidationException.Should()
                .BeEquivalentTo(expectedDataSetObjectValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetObjectValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetObjectAsync(It.IsAny<DataSetObject>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync(
            int minutesBeforeOrAfter)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            DateTimeOffset invalidDateTime =
                randomDateTimeOffset.AddMinutes(minutesBeforeOrAfter);

            DataSetObject randomDataSetObject = CreateRandomDataSetObject(invalidDateTime);
            DataSetObject invalidDataSetObject = randomDataSetObject;

            var invalidDataSetObjectException =
                new InvalidDataSetObjectException(
                    message: "Invalid dataSetObject. Please correct the errors and try again.");

            invalidDataSetObjectException.AddData(
                key: nameof(DataSetObject.CreatedDate),
                values: "Date is not recent");

            var expectedDataSetObjectValidationException =
                new DataSetObjectValidationException(
                    message: "DataSetObject validation errors occurred, please try again.",
                    innerException: invalidDataSetObjectException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<DataSetObject> addDataSetObjectTask =
                this.dataSetObjectService.AddDataSetObjectAsync(invalidDataSetObject);

            DataSetObjectValidationException actualDataSetObjectValidationException =
                await Assert.ThrowsAsync<DataSetObjectValidationException>(() =>
                    addDataSetObjectTask.AsTask());

            // then
            actualDataSetObjectValidationException.Should()
                .BeEquivalentTo(expectedDataSetObjectValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetObjectValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetObjectAsync(It.IsAny<DataSetObject>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}