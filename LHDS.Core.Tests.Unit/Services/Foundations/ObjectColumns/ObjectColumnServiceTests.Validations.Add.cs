// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ObjectColumns;
using LHDS.Core.Models.Foundations.ObjectColumns.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ObjectColumns
{
    public partial class ObjectColumnServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfObjectColumnIsNullAndLogItAsync()
        {
            // given
            ObjectColumn nullObjectColumn = null;

            var nullObjectColumnException =
                new NullObjectColumnException(message: "ObjectColumn is null.");

            var expectedObjectColumnValidationException =
                new ObjectColumnValidationException(
                    message: "ObjectColumn validation errors occurred, please try again.",
                    innerException: nullObjectColumnException);

            // when
            ValueTask<ObjectColumn> addObjectColumnTask =
                this.objectColumnService.AddObjectColumnAsync(nullObjectColumn);

            ObjectColumnValidationException actualObjectColumnValidationException =
                await Assert.ThrowsAsync<ObjectColumnValidationException>(() =>
                    addObjectColumnTask.AsTask());

            // then
            actualObjectColumnValidationException.Should()
                .BeEquivalentTo(expectedObjectColumnValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedObjectColumnValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfObjectColumnIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            var invalidObjectColumn = new ObjectColumn
            {
                OurColumnName = invalidText,
                SqlDataType = invalidText,
            };

            var invalidObjectColumnException =
                new InvalidObjectColumnException(
                    message: "Invalid objectColumn. Please correct the errors and try again.");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.Id),
                values: "Id is required");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.DataSetObjectId),
                values: "Id is required");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.SupplierColumnName),
                values: "Text is required");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.OurColumnName),
                values: "Text is required");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.SqlDataType),
                values: "Text is required");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.CodeSystem),
                values: "Text is required");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.CreatedDate),
                values: "Date is required");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.CreatedBy),
                values: "Text is required");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.UpdatedDate),
                values: "Date is required");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.UpdatedBy),
                values: "Text is required");

            var expectedObjectColumnValidationException =
                new ObjectColumnValidationException(
                    message: "ObjectColumn validation errors occurred, please try again.",
                    innerException: invalidObjectColumnException);

            // when
            ValueTask<ObjectColumn> addObjectColumnTask =
                this.objectColumnService.AddObjectColumnAsync(invalidObjectColumn);

            ObjectColumnValidationException actualObjectColumnValidationException =
                await Assert.ThrowsAsync<ObjectColumnValidationException>(() =>
                    addObjectColumnTask.AsTask());

            // then
            actualObjectColumnValidationException.Should()
                .BeEquivalentTo(expectedObjectColumnValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedObjectColumnValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertObjectColumnAsync(It.IsAny<ObjectColumn>()),
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
            ObjectColumn randomObjectColumn = CreateRandomObjectColumn(randomDateTimeOffset);
            ObjectColumn invalidObjectColumn = randomObjectColumn;

            invalidObjectColumn.UpdatedDate =
                invalidObjectColumn.CreatedDate.AddDays(randomNumber);

            var invalidObjectColumnException =
                new InvalidObjectColumnException(
                    message: "Invalid objectColumn. Please correct the errors and try again.");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.UpdatedDate),
                values: $"Date is not the same as {nameof(ObjectColumn.CreatedDate)}");

            var expectedObjectColumnValidationException =
                new ObjectColumnValidationException(
                    message: "ObjectColumn validation errors occurred, please try again.",
                    innerException: invalidObjectColumnException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<ObjectColumn> addObjectColumnTask =
                this.objectColumnService.AddObjectColumnAsync(invalidObjectColumn);

            ObjectColumnValidationException actualObjectColumnValidationException =
                await Assert.ThrowsAsync<ObjectColumnValidationException>(() =>
                    addObjectColumnTask.AsTask());

            // then
            actualObjectColumnValidationException.Should()
                .BeEquivalentTo(expectedObjectColumnValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedObjectColumnValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertObjectColumnAsync(It.IsAny<ObjectColumn>()),
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
            ObjectColumn randomObjectColumn = CreateRandomObjectColumn(randomDateTimeOffset);
            ObjectColumn invalidObjectColumn = randomObjectColumn;
            invalidObjectColumn.UpdatedBy = Guid.NewGuid().ToString();

            var invalidObjectColumnException =
                new InvalidObjectColumnException(
                    message: "Invalid objectColumn. Please correct the errors and try again.");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.UpdatedBy),
                values: $"Text is not the same as {nameof(ObjectColumn.CreatedBy)}");

            var expectedObjectColumnValidationException =
                new ObjectColumnValidationException(
                    message: "ObjectColumn validation errors occurred, please try again.",
                    innerException: invalidObjectColumnException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<ObjectColumn> addObjectColumnTask =
                this.objectColumnService.AddObjectColumnAsync(invalidObjectColumn);

            ObjectColumnValidationException actualObjectColumnValidationException =
                await Assert.ThrowsAsync<ObjectColumnValidationException>(() =>
                    addObjectColumnTask.AsTask());

            // then
            actualObjectColumnValidationException.Should()
                .BeEquivalentTo(expectedObjectColumnValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedObjectColumnValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertObjectColumnAsync(It.IsAny<ObjectColumn>()),
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

            ObjectColumn randomObjectColumn = CreateRandomObjectColumn(invalidDateTime);
            ObjectColumn invalidObjectColumn = randomObjectColumn;

            var invalidObjectColumnException =
                new InvalidObjectColumnException(
                    message: "Invalid objectColumn. Please correct the errors and try again.");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.CreatedDate),
                values: "Date is not recent");

            var expectedObjectColumnValidationException =
                new ObjectColumnValidationException(
                    message: "ObjectColumn validation errors occurred, please try again.",
                    innerException: invalidObjectColumnException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<ObjectColumn> addObjectColumnTask =
                this.objectColumnService.AddObjectColumnAsync(invalidObjectColumn);

            ObjectColumnValidationException actualObjectColumnValidationException =
                await Assert.ThrowsAsync<ObjectColumnValidationException>(() =>
                    addObjectColumnTask.AsTask());

            // then
            actualObjectColumnValidationException.Should()
                .BeEquivalentTo(expectedObjectColumnValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedObjectColumnValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertObjectColumnAsync(It.IsAny<ObjectColumn>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}