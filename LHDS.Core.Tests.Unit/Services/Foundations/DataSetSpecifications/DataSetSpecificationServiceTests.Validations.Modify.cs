using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSetSpecifications
{
    public partial class DataSetSpecificationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfDataSetSpecificationIsNullAndLogItAsync()
        {
            // given
            DataSetSpecification nullDataSetSpecification = null;
            var nullDataSetSpecificationException = new NullDataSetSpecificationException(message: "DataSetSpecification is null.");

            var expectedDataSetSpecificationValidationException =
                new DataSetSpecificationValidationException(
                    message: "DataSetSpecification validation errors occurred, please try again.",
                    innerException: nullDataSetSpecificationException);

            // when
            ValueTask<DataSetSpecification> modifyDataSetSpecificationTask =
                this.dataSetSpecificationService.ModifyDataSetSpecificationAsync(nullDataSetSpecification);

            DataSetSpecificationValidationException actualDataSetSpecificationValidationException =
                await Assert.ThrowsAsync<DataSetSpecificationValidationException>(
                    modifyDataSetSpecificationTask.AsTask);

            // then
            actualDataSetSpecificationValidationException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfDataSetSpecificationIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            var invalidDataSetSpecification = new DataSetSpecification
            {
                // TODO:  Add default values for your properties i.e. Name = invalidText
            };

            var invalidDataSetSpecificationException = 
                new InvalidDataSetSpecificationException(
                    message: "Invalid dataSetSpecification. Please correct the errors and try again.");

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.Id),
                values: "Id is required");

            //invalidDataSetSpecificationException.AddData(
            //    key: nameof(DataSetSpecification.Name),
            //    values: "Text is required");

            // TODO: Add or remove data here to suit the validation needs for the DataSetSpecification model

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.CreatedDate),
                values: "Date is required");

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.CreatedBy),
                values: "Text is required");

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.UpdatedDate),
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(DataSetSpecification.CreatedDate)}"
                });

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.UpdatedBy),
                values: "Text is required");

            var expectedDataSetSpecificationValidationException =
                new DataSetSpecificationValidationException(
                    message: "DataSetSpecification validation errors occurred, please try again.",
                    innerException: invalidDataSetSpecificationException);

            // when
            ValueTask<DataSetSpecification> modifyDataSetSpecificationTask =
                this.dataSetSpecificationService.ModifyDataSetSpecificationAsync(invalidDataSetSpecification);

            DataSetSpecificationValidationException actualDataSetSpecificationValidationException =
                await Assert.ThrowsAsync<DataSetSpecificationValidationException>(
                    modifyDataSetSpecificationTask.AsTask);

            //then
            actualDataSetSpecificationValidationException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()),
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
            DataSetSpecification randomDataSetSpecification = CreateRandomDataSetSpecification(randomDateTimeOffset);
            DataSetSpecification invalidDataSetSpecification = randomDataSetSpecification;

            var invalidDataSetSpecificationException = 
                new InvalidDataSetSpecificationException(
                    message: "Invalid dataSetSpecification. Please correct the errors and try again.");

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.UpdatedDate),
                values: $"Date is the same as {nameof(DataSetSpecification.CreatedDate)}");

            var expectedDataSetSpecificationValidationException =
                new DataSetSpecificationValidationException(
                    message: "DataSetSpecification validation errors occurred, please try again.",
                    innerException: invalidDataSetSpecificationException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<DataSetSpecification> modifyDataSetSpecificationTask =
                this.dataSetSpecificationService.ModifyDataSetSpecificationAsync(invalidDataSetSpecification);

            DataSetSpecificationValidationException actualDataSetSpecificationValidationException =
                await Assert.ThrowsAsync<DataSetSpecificationValidationException>(
                    modifyDataSetSpecificationTask.AsTask);

            // then
            actualDataSetSpecificationValidationException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetSpecificationByIdAsync(invalidDataSetSpecification.Id),
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
            DataSetSpecification randomDataSetSpecification = CreateRandomDataSetSpecification(randomDateTimeOffset);
            randomDataSetSpecification.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var invalidDataSetSpecificationException = 
                new InvalidDataSetSpecificationException(
                    message: "Invalid dataSetSpecification. Please correct the errors and try again.");

            invalidDataSetSpecificationException.AddData(
                key: nameof(DataSetSpecification.UpdatedDate),
                values: "Date is not recent");

            var expectedDataSetSpecificationValidatonException =
                new DataSetSpecificationValidationException(
                    message: "DataSetSpecification validation errors occurred, please try again.",
                    innerException: invalidDataSetSpecificationException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<DataSetSpecification> modifyDataSetSpecificationTask =
                this.dataSetSpecificationService.ModifyDataSetSpecificationAsync(randomDataSetSpecification);

            DataSetSpecificationValidationException actualDataSetSpecificationValidationException =
                await Assert.ThrowsAsync<DataSetSpecificationValidationException>(
                    modifyDataSetSpecificationTask.AsTask);

            // then
            actualDataSetSpecificationValidationException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetSpecificationByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfDataSetSpecificationDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DataSetSpecification randomDataSetSpecification = CreateRandomModifyDataSetSpecification(randomDateTimeOffset);
            DataSetSpecification nonExistDataSetSpecification = randomDataSetSpecification;
            DataSetSpecification nullDataSetSpecification = null;

            var notFoundDataSetSpecificationException =
                new NotFoundDataSetSpecificationException(nonExistDataSetSpecification.Id);

            var expectedDataSetSpecificationValidationException =
                new DataSetSpecificationValidationException(
                    message: "DataSetSpecification validation errors occurred, please try again.",
                    innerException: notFoundDataSetSpecificationException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetSpecificationByIdAsync(nonExistDataSetSpecification.Id))
                .ReturnsAsync(nullDataSetSpecification);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when 
            ValueTask<DataSetSpecification> modifyDataSetSpecificationTask =
                this.dataSetSpecificationService.ModifyDataSetSpecificationAsync(nonExistDataSetSpecification);

            DataSetSpecificationValidationException actualDataSetSpecificationValidationException =
                await Assert.ThrowsAsync<DataSetSpecificationValidationException>(
                    modifyDataSetSpecificationTask.AsTask);

            // then
            actualDataSetSpecificationValidationException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetSpecificationByIdAsync(nonExistDataSetSpecification.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}