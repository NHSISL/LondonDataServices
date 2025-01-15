// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSets.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSets
{
    public partial class DataSetServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfDataSetIsNullAndLogItAsync()
        {
            // given
            DataSet nullDataSet = null;
            var nullDataSetException = new NullDataSetException(message: "DataSet is null.");

            var expectedDataSetValidationException =
                new DataSetValidationException(
                    message: "DataSet validation errors occurred, please try again.",
                    innerException: nullDataSetException);

            // when
            ValueTask<DataSet> modifyDataSetTask =
                this.dataSetService.ModifyDataSetAsync(nullDataSet);

            DataSetValidationException actualDataSetValidationException =
                await Assert.ThrowsAsync<DataSetValidationException>(
                    modifyDataSetTask.AsTask);

            // then
            actualDataSetValidationException.Should()
                .BeEquivalentTo(expectedDataSetValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataSetAsync(It.IsAny<DataSet>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfDataSetIsInvalidAndLogItAsync(string invalidText)
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
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(DataSet.CreatedDate)}"
                });

            invalidDataSetException.AddData(
                key: nameof(DataSet.UpdatedBy),
                values: "Text is required");

            var expectedDataSetValidationException =
                new DataSetValidationException(
                    message: "DataSet validation errors occurred, please try again.",
                    innerException: invalidDataSetException);

            // when
            ValueTask<DataSet> modifyDataSetTask =
                this.dataSetService.ModifyDataSetAsync(invalidDataSet);

            DataSetValidationException actualDataSetValidationException =
                await Assert.ThrowsAsync<DataSetValidationException>(
                    modifyDataSetTask.AsTask);

            //then
            actualDataSetValidationException.Should()
                .BeEquivalentTo(expectedDataSetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
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
        public async Task ShouldThrowValidationExceptionOnModifyIfDataSetIsInvalidLengthAndLogItAsync()
        {
            // given 
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DataSet invalidDataSet = CreateRandomModifyDataSet(randomDateTimeOffset);
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
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<DataSet> modifyDataSetTask =
                this.dataSetService.ModifyDataSetAsync(invalidDataSet);

            DataSetValidationException actualDataSetValidationException =
                await Assert.ThrowsAsync<DataSetValidationException>(
                    modifyDataSetTask.AsTask);

            //then
            actualDataSetValidationException.Should()
                .BeEquivalentTo(expectedDataSetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
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
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DataSet randomDataSet = CreateRandomDataSet(randomDateTimeOffset);
            DataSet invalidDataSet = randomDataSet;

            var invalidDataSetException =
                new InvalidDataSetException(
                    message: "Invalid dataSet. Please correct the errors and try again.");

            invalidDataSetException.AddData(
                key: nameof(DataSet.UpdatedDate),
                values: $"Date is the same as {nameof(DataSet.CreatedDate)}");

            var expectedDataSetValidationException =
                new DataSetValidationException(
                    message: "DataSet validation errors occurred, please try again.",
                    innerException: invalidDataSetException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<DataSet> modifyDataSetTask =
                this.dataSetService.ModifyDataSetAsync(invalidDataSet);

            DataSetValidationException actualDataSetValidationException =
                await Assert.ThrowsAsync<DataSetValidationException>(
                    modifyDataSetTask.AsTask);

            // then
            actualDataSetValidationException.Should()
                .BeEquivalentTo(expectedDataSetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataSetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetByIdAsync(invalidDataSet.Id),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(int minutes)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DataSet randomDataSet = CreateRandomDataSet(randomDateTimeOffset);
            randomDataSet.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var invalidDataSetException =
                new InvalidDataSetException(
                    message: "Invalid dataSet. Please correct the errors and try again.");

            invalidDataSetException.AddData(
                key: nameof(DataSet.UpdatedDate),
                values: "Date is not recent");

            var expectedDataSetValidationException =
                new DataSetValidationException(
                    message: "DataSet validation errors occurred, please try again.",
                    innerException: invalidDataSetException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<DataSet> modifyDataSetTask =
                this.dataSetService.ModifyDataSetAsync(randomDataSet);

            DataSetValidationException actualDataSetValidationException =
                await Assert.ThrowsAsync<DataSetValidationException>(
                    modifyDataSetTask.AsTask);

            // then
            actualDataSetValidationException.Should()
                .BeEquivalentTo(expectedDataSetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataSetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfDataSetDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DataSet randomDataSet = CreateRandomModifyDataSet(randomDateTimeOffset);
            DataSet nonExistDataSet = randomDataSet;
            DataSet nullDataSet = null;

            var notFoundDataSetException =
                new NotFoundDataSetException(nonExistDataSet.Id);

            var expectedDataSetValidationException =
                new DataSetValidationException(
                    message: "DataSet validation errors occurred, please try again.",
                    innerException: notFoundDataSetException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetByIdAsync(nonExistDataSet.Id))
                .ReturnsAsync(nullDataSet);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            // when 
            ValueTask<DataSet> modifyDataSetTask =
                this.dataSetService.ModifyDataSetAsync(nonExistDataSet);

            DataSetValidationException actualDataSetValidationException =
                await Assert.ThrowsAsync<DataSetValidationException>(
                    modifyDataSetTask.AsTask);

            // then
            actualDataSetValidationException.Should()
                .BeEquivalentTo(expectedDataSetValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetByIdAsync(nonExistDataSet.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataSetValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNegativeNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DataSet randomDataSet = CreateRandomModifyDataSet(randomDateTimeOffset);
            DataSet invalidDataSet = randomDataSet.DeepClone();
            DataSet storageDataSet = invalidDataSet.DeepClone();
            storageDataSet.CreatedDate = storageDataSet.CreatedDate.AddMinutes(randomMinutes);
            storageDataSet.UpdatedDate = storageDataSet.UpdatedDate.AddMinutes(randomMinutes);

            var invalidDataSetException =
                new InvalidDataSetException(
                    message: "Invalid dataSet. Please correct the errors and try again.");

            invalidDataSetException.AddData(
                key: nameof(DataSet.CreatedDate),
                values: $"Date is not the same as {nameof(DataSet.CreatedDate)}");

            var expectedDataSetValidationException =
                new DataSetValidationException(
                    message: "DataSet validation errors occurred, please try again.",
                    innerException: invalidDataSetException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetByIdAsync(invalidDataSet.Id))
                .ReturnsAsync(storageDataSet);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<DataSet> modifyDataSetTask =
                this.dataSetService.ModifyDataSetAsync(invalidDataSet);

            DataSetValidationException actualDataSetValidationException =
                await Assert.ThrowsAsync<DataSetValidationException>(
                    modifyDataSetTask.AsTask);

            // then
            actualDataSetValidationException.Should()
                .BeEquivalentTo(expectedDataSetValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetByIdAsync(invalidDataSet.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDataSetValidationException))),
                       Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCreatedUserDontMatchStorageAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DataSet randomDataSet = CreateRandomModifyDataSet(randomDateTimeOffset);
            DataSet invalidDataSet = randomDataSet.DeepClone();
            DataSet storageDataSet = invalidDataSet.DeepClone();
            invalidDataSet.CreatedBy = Guid.NewGuid().ToString();
            storageDataSet.UpdatedDate = storageDataSet.CreatedDate;

            var invalidDataSetException =
                new InvalidDataSetException(
                    message: "Invalid dataSet. Please correct the errors and try again.");

            invalidDataSetException.AddData(
                key: nameof(DataSet.CreatedBy),
                values: $"Text is not the same as {nameof(DataSet.CreatedBy)}");

            var expectedDataSetValidationException =
                new DataSetValidationException(
                    message: "DataSet validation errors occurred, please try again.",
                    innerException: invalidDataSetException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetByIdAsync(invalidDataSet.Id))
                .ReturnsAsync(storageDataSet);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<DataSet> modifyDataSetTask =
                this.dataSetService.ModifyDataSetAsync(invalidDataSet);

            DataSetValidationException actualDataSetValidationException =
                await Assert.ThrowsAsync<DataSetValidationException>(
                    modifyDataSetTask.AsTask);

            // then
            actualDataSetValidationException.Should().BeEquivalentTo(expectedDataSetValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetByIdAsync(invalidDataSet.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDataSetValidationException))),
                       Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DataSet randomDataSet = CreateRandomModifyDataSet(randomDateTimeOffset);
            DataSet invalidDataSet = randomDataSet;
            DataSet storageDataSet = randomDataSet.DeepClone();

            var invalidDataSetException =
                new InvalidDataSetException(
                    message: "Invalid dataSet. Please correct the errors and try again.");

            invalidDataSetException.AddData(
                key: nameof(DataSet.UpdatedDate),
                values: $"Date is the same as {nameof(DataSet.UpdatedDate)}");

            var expectedDataSetValidationException =
                new DataSetValidationException(
                    message: "DataSet validation errors occurred, please try again.",
                    innerException: invalidDataSetException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetByIdAsync(invalidDataSet.Id))
                .ReturnsAsync(storageDataSet);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<DataSet> modifyDataSetTask =
                this.dataSetService.ModifyDataSetAsync(invalidDataSet);

            // then
            await Assert.ThrowsAsync<DataSetValidationException>(
                modifyDataSetTask.AsTask);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataSetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetByIdAsync(invalidDataSet.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}