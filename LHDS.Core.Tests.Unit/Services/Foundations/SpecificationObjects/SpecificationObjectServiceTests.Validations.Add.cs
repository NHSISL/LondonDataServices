// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using LHDS.Core.Models.Foundations.SpecificationObjects.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SpecificationObjects
{
    public partial class SpecificationObjectServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfSpecificationObjectIsNullAndLogItAsync()
        {
            // given
            SpecificationObject nullSpecificationObject = null;

            var nullSpecificationObjectException =
                new NullSpecificationObjectException(message: "SpecificationObject is null.");

            var expectedSpecificationObjectValidationException =
                new SpecificationObjectValidationException(
                    message: "SpecificationObject validation errors occurred, please try again.",
                    innerException: nullSpecificationObjectException);

            // when
            ValueTask<SpecificationObject> addSpecificationObjectTask =
                this.specificationObjectService.AddSpecificationObjectAsync(nullSpecificationObject);

            SpecificationObjectValidationException actualSpecificationObjectValidationException =
                await Assert.ThrowsAsync<SpecificationObjectValidationException>(addSpecificationObjectTask.AsTask);

            // then
            actualSpecificationObjectValidationException.Should()
                .BeEquivalentTo(expectedSpecificationObjectValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(It.IsAny<SpecificationObject>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSpecificationObjectValidationException))),
                        Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfSpecificationObjectIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            string randomUserId = GetRandomStringWithLengthOf(50);
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            var invalidSpecificationObject = new SpecificationObject
            {
                SupplierObjectName = invalidText,
                OurObjectName = invalidText,
            };

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(invalidSpecificationObject))
                    .ReturnsAsync(invalidSpecificationObject);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            var invalidSpecificationObjectException =
                new InvalidSpecificationObjectException(
                    message: "Invalid specificationObject. Please correct the errors and try again.");

            invalidSpecificationObjectException.AddData(
                key: nameof(SpecificationObject.Id),
                values: "Id is required");

            invalidSpecificationObjectException.AddData(
               key: nameof(SpecificationObject.DataSetSpecificationId),
               values: "Id is required");

            invalidSpecificationObjectException.AddData(
               key: nameof(SpecificationObject.SupplierObjectName),
               values: "Text is required");

            invalidSpecificationObjectException.AddData(
               key: nameof(SpecificationObject.OurObjectName),
               values: "Text is required");

            invalidSpecificationObjectException.AddData(
                key: nameof(SpecificationObject.CreatedDate),
                values:
                [
                    "Date is required",
                    $"Date is not recent"
                ]);

            invalidSpecificationObjectException.AddData(
                key: nameof(SpecificationObject.CreatedBy),
                values:
                [
                    "Text is required",
                    $"Expected value to be '{randomUserId}'" +
                    $" but found '{invalidSpecificationObject.CreatedBy}'."
                ]);

            invalidSpecificationObjectException.AddData(
                key: nameof(SpecificationObject.UpdatedDate),
                values: "Date is required");

            invalidSpecificationObjectException.AddData(
                key: nameof(SpecificationObject.UpdatedBy),
                values: "Text is required");

            var expectedSpecificationObjectValidationException =
                new SpecificationObjectValidationException(
                    message: "SpecificationObject validation errors occurred, please try again.",
                    innerException: invalidSpecificationObjectException);

            // when
            ValueTask<SpecificationObject> addSpecificationObjectTask =
                specificationObjectService.AddSpecificationObjectAsync(invalidSpecificationObject);

            SpecificationObjectValidationException actualSpecificationObjectValidationException =
                await Assert.ThrowsAsync<SpecificationObjectValidationException>(addSpecificationObjectTask.AsTask);

            // then
            actualSpecificationObjectValidationException.Should()
                .BeEquivalentTo(expectedSpecificationObjectValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(invalidSpecificationObject),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSpecificationObjectValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSpecificationObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfSpecificationObjectIsInvalidLengthAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            SpecificationObject invalidSpecificationObject =
                CreateRandomSpecificationObject(randomDateTimeOffset, randomUserId);

            invalidSpecificationObject.SupplierObjectName = GetRandomString(256);
            invalidSpecificationObject.OurObjectName = GetRandomString(256);
            invalidSpecificationObject.ObjectDescription = GetRandomString(501);
            invalidSpecificationObject.InterchangeProtocol = GetRandomString(256);
            invalidSpecificationObject.DeletionHandling = GetRandomString(256);
            invalidSpecificationObject.CreatedBy = GetRandomString(256);
            invalidSpecificationObject.UpdatedBy = invalidSpecificationObject.CreatedBy;

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(invalidSpecificationObject))
                    .ReturnsAsync(invalidSpecificationObject);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            var invalidSpecificationObjectException =
                new InvalidSpecificationObjectException(
                    message: "Invalid specificationObject. Please correct the errors and try again.");

            invalidSpecificationObjectException.AddData(
                key: nameof(SpecificationObject.SupplierObjectName),
                values: "Text is exceeding max length");

            invalidSpecificationObjectException.AddData(
                key: nameof(SpecificationObject.OurObjectName),
                values: "Text is exceeding max length");

            invalidSpecificationObjectException.AddData(
                key: nameof(SpecificationObject.ObjectDescription),
                values: "Text is exceeding max length");

            invalidSpecificationObjectException.AddData(
                key: nameof(SpecificationObject.InterchangeProtocol),
                values: "Text is exceeding max length");

            invalidSpecificationObjectException.AddData(
                key: nameof(SpecificationObject.DeletionHandling),
                values: "Text is exceeding max length");

            invalidSpecificationObjectException.AddData(
                key: nameof(SpecificationObject.CreatedBy),
                values:
                [
                    "Text is exceeding max length",
                    $"Expected value to be '{randomUserId}'" +
                    $" but found '{invalidSpecificationObject.CreatedBy}'."
                ]);

            invalidSpecificationObjectException.AddData(
                key: nameof(SpecificationObject.UpdatedBy),
                values: "Text is exceeding max length");

            var expectedSpecificationObjectValidationException =
                new SpecificationObjectValidationException(
                    message: "SpecificationObject validation errors occurred, please try again.",
                    innerException: invalidSpecificationObjectException);

            // when
            ValueTask<SpecificationObject> addSpecificationObjectTask =
                specificationObjectService.AddSpecificationObjectAsync(invalidSpecificationObject);

            SpecificationObjectValidationException actualSpecificationObjectValidationException =
                await Assert.ThrowsAsync<SpecificationObjectValidationException>(addSpecificationObjectTask.AsTask);

            // then
            actualSpecificationObjectValidationException.Should()
                .BeEquivalentTo(expectedSpecificationObjectValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(invalidSpecificationObject),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSpecificationObjectValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSpecificationObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            SpecificationObject randomSpecificationObject =
                CreateRandomSpecificationObject(randomDateTimeOffset, randomUserId);

            SpecificationObject invalidSpecificationObject = randomSpecificationObject;

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(invalidSpecificationObject))
                    .ReturnsAsync(invalidSpecificationObject);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            invalidSpecificationObject.UpdatedDate =
                invalidSpecificationObject.CreatedDate.AddDays(randomNumber);

            var invalidSpecificationObjectException =
                new InvalidSpecificationObjectException(
                    message: "Invalid specificationObject. Please correct the errors and try again.");

            invalidSpecificationObjectException.AddData(
                key: nameof(SpecificationObject.UpdatedDate),
                values: $"Date is not the same as {nameof(SpecificationObject.CreatedDate)}");

            var expectedSpecificationObjectValidationException =
                new SpecificationObjectValidationException(
                    message: "SpecificationObject validation errors occurred, please try again.",
                    innerException: invalidSpecificationObjectException);

            // when
            ValueTask<SpecificationObject> addSpecificationObjectTask =
                specificationObjectService.AddSpecificationObjectAsync(invalidSpecificationObject);

            SpecificationObjectValidationException actualSpecificationObjectValidationException =
                await Assert.ThrowsAsync<SpecificationObjectValidationException>(addSpecificationObjectTask.AsTask);

            // then
            actualSpecificationObjectValidationException.Should()
                .BeEquivalentTo(expectedSpecificationObjectValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(invalidSpecificationObject),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSpecificationObjectValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSpecificationObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateUsersIsNotSameAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            SpecificationObject randomSpecificationObject =
                CreateRandomSpecificationObject(randomDateTimeOffset, randomUserId);

            SpecificationObject invalidSpecificationObject = randomSpecificationObject;
            invalidSpecificationObject.UpdatedBy = Guid.NewGuid().ToString();

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(invalidSpecificationObject))
                    .ReturnsAsync(invalidSpecificationObject);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            var invalidSpecificationObjectException =
                new InvalidSpecificationObjectException(
                    message: "Invalid specificationObject. Please correct the errors and try again.");

            invalidSpecificationObjectException.AddData(
                key: nameof(SpecificationObject.UpdatedBy),
                values: $"Text is not the same as {nameof(SpecificationObject.CreatedBy)}");

            var expectedSpecificationObjectValidationException =
                new SpecificationObjectValidationException(
                    message: "SpecificationObject validation errors occurred, please try again.",
                    innerException: invalidSpecificationObjectException);

            // when
            ValueTask<SpecificationObject> addSpecificationObjectTask =
                specificationObjectService.AddSpecificationObjectAsync(invalidSpecificationObject);

            SpecificationObjectValidationException actualSpecificationObjectValidationException =
                await Assert.ThrowsAsync<SpecificationObjectValidationException>(addSpecificationObjectTask.AsTask);

            // then
            actualSpecificationObjectValidationException.Should()
                .BeEquivalentTo(expectedSpecificationObjectValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(invalidSpecificationObject),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSpecificationObjectValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSpecificationObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
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

            DateTimeOffset invalidDateTime =
                randomDateTimeOffset.AddMinutes(minutesBeforeOrAfter);

            string randomUserId = GetRandomStringWithLengthOf(50);

            SpecificationObject randomSpecificationObject =
                CreateRandomSpecificationObject(randomDateTimeOffset, randomUserId);

            SpecificationObject invalidSpecificationObject = randomSpecificationObject;

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(invalidSpecificationObject))
                    .ReturnsAsync(invalidSpecificationObject);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(invalidDateTime);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            var invalidSpecificationObjectException =
                new InvalidSpecificationObjectException(
                    message: "Invalid specificationObject. Please correct the errors and try again.");

            invalidSpecificationObjectException.AddData(
                key: nameof(SpecificationObject.CreatedDate),
                values: "Date is not recent");

            var expectedSpecificationObjectValidationException =
                new SpecificationObjectValidationException(
                    message: "SpecificationObject validation errors occurred, please try again.",
                    innerException: invalidSpecificationObjectException);

            // when
            ValueTask<SpecificationObject> addSpecificationObjectTask =
                specificationObjectService.AddSpecificationObjectAsync(invalidSpecificationObject);

            SpecificationObjectValidationException actualSpecificationObjectValidationException =
                await Assert.ThrowsAsync<SpecificationObjectValidationException>(addSpecificationObjectTask.AsTask);

            // then
            actualSpecificationObjectValidationException.Should()
                .BeEquivalentTo(expectedSpecificationObjectValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(invalidSpecificationObject),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSpecificationObjectValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSpecificationObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}