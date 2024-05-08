// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSetSpecifications
{
    public partial class DataSetSpecificationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfDataSetSpecificationIsNullAndLogItAsync()
        {
            // given
            DataSetSpecification nullDataSetSpecification = null;

            var nullDataSetSpecificationException =
                new NullDataSetSpecificationException(message: "DataSetSpecification is null.");

            var expectedDataSetSpecificationValidationException =
                new DataSetSpecificationValidationException(
                    message: "DataSetSpecification validation errors occurred, please try again.",
                    innerException: nullDataSetSpecificationException);

            // when
            ValueTask<DataSetSpecification> addDataSetSpecificationTask =
                this.dataSetSpecificationService.AddDataSetSpecificationAsync(nullDataSetSpecification);

            DataSetSpecificationValidationException actualDataSetSpecificationValidationException =
                await Assert.ThrowsAsync<DataSetSpecificationValidationException>(addDataSetSpecificationTask.AsTask);

            // then
            actualDataSetSpecificationValidationException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfDataSetSpecificationIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidDataSetSpecification = new DataSetSpecification
            {
                SupplierSpecificationVersion = invalidText,
                OurSpecificationVersion = invalidText,
                CreatedBy = invalidText,
                UpdatedBy = invalidText
            };

            var invalidDataSetSpecificationException =
                new InvalidDataSetSpecificationException(
                    message: "Invalid dataSetSpecification. Please correct the errors and try again.");

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.Id),
                values: "Id is required");

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.DataSetId),
                values: "Id is required");

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.SupplierSpecificationVersion),
                values: "Text is required");

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.OurSpecificationVersion),
                values: "Text is required");

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.CreatedDate),
                values: "Date is required");

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.CreatedBy),
                values: "Text is required");

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.UpdatedDate),
                values: "Date is required");

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.UpdatedBy),
                values: "Text is required");

            var expectedDataSetSpecificationValidationException =
                new DataSetSpecificationValidationException(
                    message: "DataSetSpecification validation errors occurred, please try again.",
                    innerException: invalidDataSetSpecificationException);

            // when
            ValueTask<DataSetSpecification> addDataSetSpecificationTask =
                this.dataSetSpecificationService.AddDataSetSpecificationAsync(invalidDataSetSpecification);

            DataSetSpecificationValidationException actualDataSetSpecificationValidationException =
                await Assert.ThrowsAsync<DataSetSpecificationValidationException>(addDataSetSpecificationTask.AsTask);

            // then
            actualDataSetSpecificationValidationException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfDataSetSpecificationIsInvalidLengthAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DataSetSpecification invalidDataSetSpecification = CreateRandomDataSetSpecification(randomDateTimeOffset);
            invalidDataSetSpecification.SupplierSpecificationVersion = GetRandomString(11);
            invalidDataSetSpecification.OurSpecificationVersion = GetRandomString(11);
            invalidDataSetSpecification.CreatedBy = GetRandomString(256);
            invalidDataSetSpecification.UpdatedBy = invalidDataSetSpecification.CreatedBy;

            var invalidDataSetSpecificationException =
                new InvalidDataSetSpecificationException(
                    message: "Invalid dataSetSpecification. Please correct the errors and try again.");

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.SupplierSpecificationVersion),
                values: "Text is exceeding max length");

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.OurSpecificationVersion),
                values: "Text is exceeding max length");

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.CreatedBy),
                values: "Text is exceeding max length");

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.UpdatedBy),
                values: "Text is exceeding max length");

            var expectedDataSetSpecificationValidationException =
                new DataSetSpecificationValidationException(
                    message: "DataSetSpecification validation errors occurred, please try again.",
                    innerException: invalidDataSetSpecificationException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<DataSetSpecification> addDataSetSpecificationTask =
                this.dataSetSpecificationService.AddDataSetSpecificationAsync(invalidDataSetSpecification);

            DataSetSpecificationValidationException actualDataSetSpecificationValidationException =
                await Assert.ThrowsAsync<DataSetSpecificationValidationException>(addDataSetSpecificationTask.AsTask);

            // then
            actualDataSetSpecificationValidationException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()),
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
            DataSetSpecification randomDataSetSpecification = CreateRandomDataSetSpecification(randomDateTimeOffset);
            DataSetSpecification invalidDataSetSpecification = randomDataSetSpecification;

            invalidDataSetSpecification.UpdatedDate =
                invalidDataSetSpecification.CreatedDate.AddDays(randomNumber);

            var invalidDataSetSpecificationException =
                new InvalidDataSetSpecificationException(
                    message: "Invalid dataSetSpecification. Please correct the errors and try again.");

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.UpdatedDate),
                values: $"Date is not the same as {nameof(DataSetSpecification.CreatedDate)}");

            var expectedDataSetSpecificationValidationException =
                new DataSetSpecificationValidationException(
                    message: "DataSetSpecification validation errors occurred, please try again.",
                    innerException: invalidDataSetSpecificationException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<DataSetSpecification> addDataSetSpecificationTask =
                this.dataSetSpecificationService.AddDataSetSpecificationAsync(invalidDataSetSpecification);

            DataSetSpecificationValidationException actualDataSetSpecificationValidationException =
                await Assert.ThrowsAsync<DataSetSpecificationValidationException>(addDataSetSpecificationTask.AsTask);

            // then
            actualDataSetSpecificationValidationException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()),
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
            DataSetSpecification randomDataSetSpecification = CreateRandomDataSetSpecification(randomDateTimeOffset);
            DataSetSpecification invalidDataSetSpecification = randomDataSetSpecification;
            invalidDataSetSpecification.UpdatedBy = Guid.NewGuid().ToString();

            var invalidDataSetSpecificationException =
                new InvalidDataSetSpecificationException(
                    message: "Invalid dataSetSpecification. Please correct the errors and try again.");

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.UpdatedBy),
                values: $"Text is not the same as {nameof(DataSetSpecification.CreatedBy)}");

            var expectedDataSetSpecificationValidationException =
                new DataSetSpecificationValidationException(
                    message: "DataSetSpecification validation errors occurred, please try again.",
                    innerException: invalidDataSetSpecificationException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<DataSetSpecification> addDataSetSpecificationTask =
                this.dataSetSpecificationService.AddDataSetSpecificationAsync(invalidDataSetSpecification);

            DataSetSpecificationValidationException actualDataSetSpecificationValidationException =
                await Assert.ThrowsAsync<DataSetSpecificationValidationException>(addDataSetSpecificationTask.AsTask);

            // then
            actualDataSetSpecificationValidationException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()),
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

            DataSetSpecification randomDataSetSpecification = CreateRandomDataSetSpecification(invalidDateTime);
            DataSetSpecification invalidDataSetSpecification = randomDataSetSpecification;

            var invalidDataSetSpecificationException =
                new InvalidDataSetSpecificationException(
                    message: "Invalid dataSetSpecification. Please correct the errors and try again.");

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.CreatedDate),
                values: "Date is not recent");

            var expectedDataSetSpecificationValidationException =
                new DataSetSpecificationValidationException(
                    message: "DataSetSpecification validation errors occurred, please try again.",
                    innerException: invalidDataSetSpecificationException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<DataSetSpecification> addDataSetSpecificationTask =
                this.dataSetSpecificationService.AddDataSetSpecificationAsync(invalidDataSetSpecification);

            DataSetSpecificationValidationException actualDataSetSpecificationValidationException =
                await Assert.ThrowsAsync<DataSetSpecificationValidationException>(addDataSetSpecificationTask.AsTask);

            // then
            actualDataSetSpecificationValidationException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}