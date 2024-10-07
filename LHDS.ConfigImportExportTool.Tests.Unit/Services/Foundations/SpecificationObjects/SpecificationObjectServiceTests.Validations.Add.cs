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

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSpecificationObjectValidationException))),
                        Times.Once);

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
            var invalidSpecificationObject = new SpecificationObject
            {
                SupplierObjectName = invalidText,
                OurObjectName = invalidText,
            };

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
                values: "Date is required");

            invalidSpecificationObjectException.AddData(
                key: nameof(SpecificationObject.CreatedBy),
                values: "Text is required");

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
                this.specificationObjectService.AddSpecificationObjectAsync(invalidSpecificationObject);

            SpecificationObjectValidationException actualSpecificationObjectValidationException =
                await Assert.ThrowsAsync<SpecificationObjectValidationException>(addSpecificationObjectTask.AsTask);

            // then
            actualSpecificationObjectValidationException.Should()
                .BeEquivalentTo(expectedSpecificationObjectValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSpecificationObjectValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSpecificationObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfSpecificationObjectIsInvalidLengthAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            SpecificationObject invalidSpecificationObject = CreateRandomSpecificationObject(randomDateTimeOffset);
            invalidSpecificationObject.SupplierObjectName = GetRandomString(256);
            invalidSpecificationObject.OurObjectName = GetRandomString(256);
            invalidSpecificationObject.ObjectDescription = GetRandomString(501);
            invalidSpecificationObject.InterchangeProtocol = GetRandomString(256);
            invalidSpecificationObject.DeletionHandling = GetRandomString(256);
            invalidSpecificationObject.CreatedBy = GetRandomString(256);
            invalidSpecificationObject.UpdatedBy = invalidSpecificationObject.CreatedBy;

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
                values: "Text is exceeding max length");

            invalidSpecificationObjectException.AddData(
                key: nameof(SpecificationObject.UpdatedBy),
                values: "Text is exceeding max length");

            var expectedSpecificationObjectValidationException =
                new SpecificationObjectValidationException(
                    message: "SpecificationObject validation errors occurred, please try again.",
                    innerException: invalidSpecificationObjectException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<SpecificationObject> addSpecificationObjectTask =
                this.specificationObjectService.AddSpecificationObjectAsync(invalidSpecificationObject);

            SpecificationObjectValidationException actualSpecificationObjectValidationException =
                await Assert.ThrowsAsync<SpecificationObjectValidationException>(addSpecificationObjectTask.AsTask);

            // then
            actualSpecificationObjectValidationException.Should()
                .BeEquivalentTo(expectedSpecificationObjectValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSpecificationObjectValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSpecificationObjectAsync(It.IsAny<SpecificationObject>()),
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
            SpecificationObject randomSpecificationObject = CreateRandomSpecificationObject(randomDateTimeOffset);
            SpecificationObject invalidSpecificationObject = randomSpecificationObject;

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

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<SpecificationObject> addSpecificationObjectTask =
                this.specificationObjectService.AddSpecificationObjectAsync(invalidSpecificationObject);

            SpecificationObjectValidationException actualSpecificationObjectValidationException =
                await Assert.ThrowsAsync<SpecificationObjectValidationException>(addSpecificationObjectTask.AsTask);

            // then
            actualSpecificationObjectValidationException.Should()
                .BeEquivalentTo(expectedSpecificationObjectValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSpecificationObjectValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSpecificationObjectAsync(It.IsAny<SpecificationObject>()),
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
            SpecificationObject randomSpecificationObject = CreateRandomSpecificationObject(randomDateTimeOffset);
            SpecificationObject invalidSpecificationObject = randomSpecificationObject;
            invalidSpecificationObject.UpdatedBy = Guid.NewGuid().ToString();

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

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<SpecificationObject> addSpecificationObjectTask =
                this.specificationObjectService.AddSpecificationObjectAsync(invalidSpecificationObject);

            SpecificationObjectValidationException actualSpecificationObjectValidationException =
                await Assert.ThrowsAsync<SpecificationObjectValidationException>(addSpecificationObjectTask.AsTask);

            // then
            actualSpecificationObjectValidationException.Should()
                .BeEquivalentTo(expectedSpecificationObjectValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSpecificationObjectValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSpecificationObjectAsync(It.IsAny<SpecificationObject>()),
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

            SpecificationObject randomSpecificationObject = CreateRandomSpecificationObject(invalidDateTime);
            SpecificationObject invalidSpecificationObject = randomSpecificationObject;

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

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<SpecificationObject> addSpecificationObjectTask =
                this.specificationObjectService.AddSpecificationObjectAsync(invalidSpecificationObject);

            SpecificationObjectValidationException actualSpecificationObjectValidationException =
                await Assert.ThrowsAsync<SpecificationObjectValidationException>(addSpecificationObjectTask.AsTask);

            // then
            actualSpecificationObjectValidationException.Should()
                .BeEquivalentTo(expectedSpecificationObjectValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSpecificationObjectValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSpecificationObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}