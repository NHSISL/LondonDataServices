using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.ObjectColumns;
using LHDS.Core.Models.Foundations.ObjectColumns.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ObjectColumns
{
    public partial class ObjectColumnServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfObjectColumnIsNullAndLogItAsync()
        {
            // given
            ObjectColumn nullObjectColumn = null;
            var nullObjectColumnException = new NullObjectColumnException(message: "ObjectColumn is null.");

            var expectedObjectColumnValidationException =
                new ObjectColumnValidationException(
                    message: "ObjectColumn validation errors occurred, please try again.",
                    innerException: nullObjectColumnException);

            // when
            ValueTask<ObjectColumn> modifyObjectColumnTask =
                this.objectColumnService.ModifyObjectColumnAsync(nullObjectColumn);

            ObjectColumnValidationException actualObjectColumnValidationException =
                await Assert.ThrowsAsync<ObjectColumnValidationException>(
                    modifyObjectColumnTask.AsTask);

            // then
            actualObjectColumnValidationException.Should()
                .BeEquivalentTo(expectedObjectColumnValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedObjectColumnValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateObjectColumnAsync(It.IsAny<ObjectColumn>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfObjectColumnIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            var invalidObjectColumn = new ObjectColumn
            {
                // TODO:  Add default values for your properties i.e. Name = invalidText
            };

            var invalidObjectColumnException = 
                new InvalidObjectColumnException(
                    message: "Invalid objectColumn. Please correct the errors and try again.");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.Id),
                values: "Id is required");

            //invalidObjectColumnException.AddData(
            //    key: nameof(ObjectColumn.Name),
            //    values: "Text is required");

            // TODO: Add or remove data here to suit the validation needs for the ObjectColumn model

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.CreatedDate),
                values: "Date is required");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.CreatedBy),
                values: "Text is required");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.UpdatedDate),
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(ObjectColumn.CreatedDate)}"
                });

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.UpdatedBy),
                values: "Text is required");

            var expectedObjectColumnValidationException =
                new ObjectColumnValidationException(
                    message: "ObjectColumn validation errors occurred, please try again.",
                    innerException: invalidObjectColumnException);

            // when
            ValueTask<ObjectColumn> modifyObjectColumnTask =
                this.objectColumnService.ModifyObjectColumnAsync(invalidObjectColumn);

            ObjectColumnValidationException actualObjectColumnValidationException =
                await Assert.ThrowsAsync<ObjectColumnValidationException>(
                    modifyObjectColumnTask.AsTask);

            //then
            actualObjectColumnValidationException.Should()
                .BeEquivalentTo(expectedObjectColumnValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedObjectColumnValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateObjectColumnAsync(It.IsAny<ObjectColumn>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            ObjectColumn randomObjectColumn = CreateRandomObjectColumn(randomDateTimeOffset);
            ObjectColumn invalidObjectColumn = randomObjectColumn;

            var invalidObjectColumnException = 
                new InvalidObjectColumnException(
                    message: "Invalid objectColumn. Please correct the errors and try again.");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.UpdatedDate),
                values: $"Date is the same as {nameof(ObjectColumn.CreatedDate)}");

            var expectedObjectColumnValidationException =
                new ObjectColumnValidationException(
                    message: "ObjectColumn validation errors occurred, please try again.",
                    innerException: invalidObjectColumnException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<ObjectColumn> modifyObjectColumnTask =
                this.objectColumnService.ModifyObjectColumnAsync(invalidObjectColumn);

            ObjectColumnValidationException actualObjectColumnValidationException =
                await Assert.ThrowsAsync<ObjectColumnValidationException>(
                    modifyObjectColumnTask.AsTask);

            // then
            actualObjectColumnValidationException.Should()
                .BeEquivalentTo(expectedObjectColumnValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedObjectColumnValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectObjectColumnByIdAsync(invalidObjectColumn.Id),
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
            ObjectColumn randomObjectColumn = CreateRandomObjectColumn(randomDateTimeOffset);
            randomObjectColumn.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var invalidObjectColumnException = 
                new InvalidObjectColumnException(
                    message: "Invalid objectColumn. Please correct the errors and try again.");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.UpdatedDate),
                values: "Date is not recent");

            var expectedObjectColumnValidatonException =
                new ObjectColumnValidationException(
                    message: "ObjectColumn validation errors occurred, please try again.",
                    innerException: invalidObjectColumnException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<ObjectColumn> modifyObjectColumnTask =
                this.objectColumnService.ModifyObjectColumnAsync(randomObjectColumn);

            ObjectColumnValidationException actualObjectColumnValidationException =
                await Assert.ThrowsAsync<ObjectColumnValidationException>(
                    modifyObjectColumnTask.AsTask);

            // then
            actualObjectColumnValidationException.Should().BeEquivalentTo(expectedObjectColumnValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedObjectColumnValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectObjectColumnByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}