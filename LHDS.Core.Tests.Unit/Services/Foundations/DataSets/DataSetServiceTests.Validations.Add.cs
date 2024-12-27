// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSets.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSets
{
    public partial class DataSetServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfDataSetIsNullAndLogItAsync()
        {
            // given
            DataSet nullDataSet = null;

            var nullDataSetException =
                new NullDataSetException(message: "DataSet is null.");

            var expectedDataSetValidationException =
                new DataSetValidationException(
                    message: "DataSet validation errors occurred, please try again.",
                    innerException: nullDataSetException);

            // when
            ValueTask<DataSet> addDataSetTask =
                this.dataSetService.AddDataSetAsync(nullDataSet);

            DataSetValidationException actualDataSetValidationException =
                await Assert.ThrowsAsync<DataSetValidationException>(addDataSetTask.AsTask);

            // then
            actualDataSetValidationException.Should()
                .BeEquivalentTo(expectedDataSetValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfDataSetIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            var invalidDataSet = new DataSet
            {
                DataSetName = invalidText,
                DataSetAliases = invalidText,
                DataSetAuthor = invalidText,
                SpecifiedBy = invalidText,
                CollectedBy = invalidText,
                DataSourceType = invalidText,
            };

            var invalidDataSetException =
                new InvalidDataSetException(
                    message: "Invalid dataSet. Please correct the errors and try again.");

            invalidDataSetException.AddData(
                key: nameof(DataSet.Id),
                values: "Id is required");

            invalidDataSetException.AddData(
                key: nameof(DataSet.SupplierId),
                values: "Id is required");

            invalidDataSetException.AddData(
               key: nameof(DataSet.DataSetName),
               values: "Text is required");

            invalidDataSetException.AddData(
               key: nameof(DataSet.DataSetAliases),
               values: "Text is required");

            invalidDataSetException.AddData(
               key: nameof(DataSet.DataSetAuthor),
               values: "Text is required");

            invalidDataSetException.AddData(
               key: nameof(DataSet.SpecifiedBy),
               values: "Text is required");

            invalidDataSetException.AddData(
               key: nameof(DataSet.CollectedBy),
               values: "Text is required");

            invalidDataSetException.AddData(
               key: nameof(DataSet.DataSourceType),
               values: "Text is required");

            invalidDataSetException.AddData(
                key: nameof(DataSet.CreatedDate),
                values: "Date is required");

            invalidDataSetException.AddData(
                key: nameof(DataSet.CreatedBy),
                values: "Text is required");

            invalidDataSetException.AddData(
                key: nameof(DataSet.UpdatedDate),
                values: "Date is required");

            invalidDataSetException.AddData(
                key: nameof(DataSet.UpdatedBy),
                values: "Text is required");

            var expectedDataSetValidationException =
                new DataSetValidationException(
                    message: "DataSet validation errors occurred, please try again.",
                    innerException: invalidDataSetException);

            // when
            ValueTask<DataSet> addDataSetTask =
                this.dataSetService.AddDataSetAsync(invalidDataSet);

            DataSetValidationException actualDataSetValidationException =
                await Assert.ThrowsAsync<DataSetValidationException>(addDataSetTask.AsTask);

            // then
            actualDataSetValidationException.Should()
                .BeEquivalentTo(expectedDataSetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetAsync(It.IsAny<DataSet>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfDataSetIsInvalidLengthAndLogItAsync()
        {
            // given 
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DataSet invalidDataSet = CreateRandomDataSet(randomDateTimeOffset);
            invalidDataSet.DataSetName = GetRandomString(151);
            invalidDataSet.DataSetAliases = GetRandomString(251);
            invalidDataSet.DataSetAuthor = GetRandomString(151);
            invalidDataSet.DataSourceType = GetRandomString(151);
            invalidDataSet.CreatedBy = GetRandomString(256);
            invalidDataSet.UpdatedBy = invalidDataSet.CreatedBy;

            var invalidDataSetException =
                new InvalidDataSetException(
                    message: "Invalid dataSet. Please correct the errors and try again.");

            invalidDataSetException.AddData(
                key: nameof(DataSet.DataSetName),
                values: "Text is exceeding max length");

            invalidDataSetException.AddData(
                key: nameof(DataSet.DataSetAliases),
                values: "Text is exceeding max length");

            invalidDataSetException.AddData(
                key: nameof(DataSet.DataSetAuthor),
                values: "Text is exceeding max length");

            invalidDataSetException.AddData(
                key: nameof(DataSet.DataSourceType),
                values: "Text is exceeding max length");

            invalidDataSetException.AddData(
                key: nameof(DataSet.CreatedBy),
                values: "Text is exceeding max length");

            invalidDataSetException.AddData(
                key: nameof(DataSet.UpdatedBy),
                values: "Text is exceeding max length");

            var expectedDataSetValidationException =
                new DataSetValidationException(
                    message: "DataSet validation errors occurred, please try again.",
                    innerException: invalidDataSetException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<DataSet> addDataSetTask =
                this.dataSetService.AddDataSetAsync(invalidDataSet);

            DataSetValidationException actualDataSetValidationException =
                await Assert.ThrowsAsync<DataSetValidationException>(
                    addDataSetTask.AsTask);

            //then
            actualDataSetValidationException.Should()
                .BeEquivalentTo(expectedDataSetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataSetAsync(It.IsAny<DataSet>()),
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
            DataSet randomDataSet = CreateRandomDataSet(randomDateTimeOffset);
            DataSet invalidDataSet = randomDataSet;

            invalidDataSet.UpdatedDate =
                invalidDataSet.CreatedDate.AddDays(randomNumber);

            var invalidDataSetException =
                new InvalidDataSetException(
                    message: "Invalid dataSet. Please correct the errors and try again.");

            invalidDataSetException.AddData(
                key: nameof(DataSet.UpdatedDate),
                values: $"Date is not the same as {nameof(DataSet.CreatedDate)}");

            var expectedDataSetValidationException =
                new DataSetValidationException(
                    message: "DataSet validation errors occurred, please try again.",
                    innerException: invalidDataSetException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<DataSet> addDataSetTask =
                this.dataSetService.AddDataSetAsync(invalidDataSet);

            DataSetValidationException actualDataSetValidationException =
                await Assert.ThrowsAsync<DataSetValidationException>(addDataSetTask.AsTask);

            // then
            actualDataSetValidationException.Should()
                .BeEquivalentTo(expectedDataSetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetAsync(It.IsAny<DataSet>()),
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
            DataSet randomDataSet = CreateRandomDataSet(randomDateTimeOffset);
            DataSet invalidDataSet = randomDataSet;
            invalidDataSet.UpdatedBy = Guid.NewGuid().ToString();

            var invalidDataSetException =
                new InvalidDataSetException(
                    message: "Invalid dataSet. Please correct the errors and try again.");

            invalidDataSetException.AddData(
                key: nameof(DataSet.UpdatedBy),
                values: $"Text is not the same as {nameof(DataSet.CreatedBy)}");

            var expectedDataSetValidationException =
                new DataSetValidationException(
                    message: "DataSet validation errors occurred, please try again.",
                    innerException: invalidDataSetException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<DataSet> addDataSetTask =
                this.dataSetService.AddDataSetAsync(invalidDataSet);

            DataSetValidationException actualDataSetValidationException =
                await Assert.ThrowsAsync<DataSetValidationException>(addDataSetTask.AsTask);

            // then
            actualDataSetValidationException.Should()
                .BeEquivalentTo(expectedDataSetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetAsync(It.IsAny<DataSet>()),
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

            DataSet randomDataSet = CreateRandomDataSet(invalidDateTime);
            DataSet invalidDataSet = randomDataSet;

            var invalidDataSetException =
                new InvalidDataSetException(
                    message: "Invalid dataSet. Please correct the errors and try again.");

            invalidDataSetException.AddData(
                key: nameof(DataSet.CreatedDate),
                values: "Date is not recent");

            var expectedDataSetValidationException =
                new DataSetValidationException(
                    message: "DataSet validation errors occurred, please try again.",
                    innerException: invalidDataSetException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<DataSet> addDataSetTask =
                this.dataSetService.AddDataSetAsync(invalidDataSet);

            DataSetValidationException actualDataSetValidationException =
                await Assert.ThrowsAsync<DataSetValidationException>(addDataSetTask.AsTask);

            // then
            actualDataSetValidationException.Should()
                .BeEquivalentTo(expectedDataSetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetAsync(It.IsAny<DataSet>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}