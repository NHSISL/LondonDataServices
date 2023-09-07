// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using LHDS.Core.Models.Foundations.SpecificationObjects.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSetObjects
{
    public partial class DataSetObjectServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfDataSetObjectIsNullAndLogItAsync()
        {
            // given
            SpecificationObject nullDataSetObject = null;
            var nullDataSetObjectException = new NullDataSetObjectException(message: "DataSetObject is null.");

            var expectedDataSetObjectValidationException =
                new DataSetObjectValidationException(
                    message: "DataSetObject validation errors occurred, please try again.",
                    innerException: nullDataSetObjectException);

            // when
            ValueTask<SpecificationObject> modifyDataSetObjectTask =
                this.dataSetObjectService.ModifyDataSetObjectAsync(nullDataSetObject);

            DataSetObjectValidationException actualDataSetObjectValidationException =
                await Assert.ThrowsAsync<DataSetObjectValidationException>(
                    modifyDataSetObjectTask.AsTask);

            // then
            actualDataSetObjectValidationException.Should()
                .BeEquivalentTo(expectedDataSetObjectValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetObjectValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataSetObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfDataSetObjectIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            var invalidDataSetObject = new SpecificationObject
            {
                SupplierObjectName = invalidText,
                OurObjectName = invalidText,
            };

            var invalidDataSetObjectException =
                new InvalidDataSetObjectException(
                    message: "Invalid dataSetObject. Please correct the errors and try again.");

            invalidDataSetObjectException.AddData(
                key: nameof(SpecificationObject.Id),
                values: "Id is required");

            invalidDataSetObjectException.AddData(
                key: nameof(SpecificationObject.DataSetSpecificationId),
                values: "Id is required");

            invalidDataSetObjectException.AddData(
                key: nameof(SpecificationObject.SupplierObjectName),
                values: "Text is required");

            invalidDataSetObjectException.AddData(
                key: nameof(SpecificationObject.OurObjectName),
                values: "Text is required");

            invalidDataSetObjectException.AddData(
                key: nameof(SpecificationObject.CreatedDate),
                values: "Date is required");

            invalidDataSetObjectException.AddData(
                key: nameof(SpecificationObject.CreatedBy),
                values: "Text is required");

            invalidDataSetObjectException.AddData(
                key: nameof(SpecificationObject.UpdatedDate),
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(SpecificationObject.CreatedDate)}"
                });

            invalidDataSetObjectException.AddData(
                key: nameof(SpecificationObject.UpdatedBy),
                values: "Text is required");

            var expectedDataSetObjectValidationException =
                new DataSetObjectValidationException(
                    message: "DataSetObject validation errors occurred, please try again.",
                    innerException: invalidDataSetObjectException);

            // when
            ValueTask<SpecificationObject> modifyDataSetObjectTask =
                this.dataSetObjectService.ModifyDataSetObjectAsync(invalidDataSetObject);

            DataSetObjectValidationException actualDataSetObjectValidationException =
                await Assert.ThrowsAsync<DataSetObjectValidationException>(
                    modifyDataSetObjectTask.AsTask);

            //then
            actualDataSetObjectValidationException.Should()
                .BeEquivalentTo(expectedDataSetObjectValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetObjectValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataSetObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfDataSetObjectIsInvalidLengthAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            SpecificationObject invalidDataSetObject = CreateRandomModifyDataSetObject(randomDateTimeOffset);
            invalidDataSetObject.SupplierObjectName = GetRandomString(256);
            invalidDataSetObject.OurObjectName = GetRandomString(256);
            invalidDataSetObject.ObjectDescription = GetRandomString(501);
            invalidDataSetObject.InterchangeProtocol = GetRandomString(256);
            invalidDataSetObject.DeletionHandling = GetRandomString(256);
            invalidDataSetObject.CreatedBy = GetRandomString(256);
            invalidDataSetObject.UpdatedBy = invalidDataSetObject.CreatedBy;

            var invalidDataSetObjectException =
                new InvalidDataSetObjectException(
                    message: "Invalid dataSetObject. Please correct the errors and try again.");

            invalidDataSetObjectException.AddData(
                key: nameof(SpecificationObject.SupplierObjectName),
                values: "Text is exceeding max length");

            invalidDataSetObjectException.AddData(
                key: nameof(SpecificationObject.OurObjectName),
                values: "Text is exceeding max length");

            invalidDataSetObjectException.AddData(
                key: nameof(SpecificationObject.ObjectDescription),
                values: "Text is exceeding max length");

            invalidDataSetObjectException.AddData(
                key: nameof(SpecificationObject.InterchangeProtocol),
                values: "Text is exceeding max length");

            invalidDataSetObjectException.AddData(
                key: nameof(SpecificationObject.DeletionHandling),
                values: "Text is exceeding max length");

            invalidDataSetObjectException.AddData(
                key: nameof(SpecificationObject.CreatedBy),
                values: "Text is exceeding max length");

            invalidDataSetObjectException.AddData(
                key: nameof(SpecificationObject.UpdatedBy),
                values: "Text is exceeding max length");

            var expectedDataSetObjectValidationException =
                new DataSetObjectValidationException(
                    message: "DataSetObject validation errors occurred, please try again.",
                    innerException: invalidDataSetObjectException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<SpecificationObject> modifyDataSetObjectTask =
                this.dataSetObjectService.ModifyDataSetObjectAsync(invalidDataSetObject);

            DataSetObjectValidationException actualDataSetObjectValidationException =
                await Assert.ThrowsAsync<DataSetObjectValidationException>(() =>
                    modifyDataSetObjectTask.AsTask());

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
                broker.UpdateDataSetObjectAsync(It.IsAny<SpecificationObject>()),
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
            SpecificationObject randomDataSetObject = CreateRandomDataSetObject(randomDateTimeOffset);
            SpecificationObject invalidDataSetObject = randomDataSetObject;

            var invalidDataSetObjectException =
                new InvalidDataSetObjectException(
                    message: "Invalid dataSetObject. Please correct the errors and try again.");

            invalidDataSetObjectException.AddData(
                key: nameof(SpecificationObject.UpdatedDate),
                values: $"Date is the same as {nameof(SpecificationObject.CreatedDate)}");

            var expectedDataSetObjectValidationException =
                new DataSetObjectValidationException(
                    message: "DataSetObject validation errors occurred, please try again.",
                    innerException: invalidDataSetObjectException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<SpecificationObject> modifyDataSetObjectTask =
                this.dataSetObjectService.ModifyDataSetObjectAsync(invalidDataSetObject);

            DataSetObjectValidationException actualDataSetObjectValidationException =
                await Assert.ThrowsAsync<DataSetObjectValidationException>(
                    modifyDataSetObjectTask.AsTask);

            // then
            actualDataSetObjectValidationException.Should()
                .BeEquivalentTo(expectedDataSetObjectValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetObjectValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetObjectByIdAsync(invalidDataSetObject.Id),
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
            SpecificationObject randomDataSetObject = CreateRandomDataSetObject(randomDateTimeOffset);
            randomDataSetObject.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var invalidDataSetObjectException =
                new InvalidDataSetObjectException(
                    message: "Invalid dataSetObject. Please correct the errors and try again.");

            invalidDataSetObjectException.AddData(
                key: nameof(SpecificationObject.UpdatedDate),
                values: "Date is not recent");

            var expectedDataSetObjectValidatonException =
                new DataSetObjectValidationException(
                    message: "DataSetObject validation errors occurred, please try again.",
                    innerException: invalidDataSetObjectException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<SpecificationObject> modifyDataSetObjectTask =
                this.dataSetObjectService.ModifyDataSetObjectAsync(randomDataSetObject);

            DataSetObjectValidationException actualDataSetObjectValidationException =
                await Assert.ThrowsAsync<DataSetObjectValidationException>(
                    modifyDataSetObjectTask.AsTask);

            // then
            actualDataSetObjectValidationException.Should()
                .BeEquivalentTo(expectedDataSetObjectValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetObjectValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetObjectByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfDataSetObjectDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            SpecificationObject randomDataSetObject = CreateRandomModifyDataSetObject(randomDateTimeOffset);
            SpecificationObject nonExistDataSetObject = randomDataSetObject;
            SpecificationObject nullDataSetObject = null;

            var notFoundDataSetObjectException =
                new NotFoundDataSetObjectException(nonExistDataSetObject.Id);

            var expectedDataSetObjectValidationException =
                new DataSetObjectValidationException(
                    message: "DataSetObject validation errors occurred, please try again.",
                    innerException: notFoundDataSetObjectException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetObjectByIdAsync(nonExistDataSetObject.Id))
                .ReturnsAsync(nullDataSetObject);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when 
            ValueTask<SpecificationObject> modifyDataSetObjectTask =
                this.dataSetObjectService.ModifyDataSetObjectAsync(nonExistDataSetObject);

            DataSetObjectValidationException actualDataSetObjectValidationException =
                await Assert.ThrowsAsync<DataSetObjectValidationException>(
                    modifyDataSetObjectTask.AsTask);

            // then
            actualDataSetObjectValidationException.Should()
                .BeEquivalentTo(expectedDataSetObjectValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetObjectByIdAsync(nonExistDataSetObject.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetObjectValidationException))),
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
            SpecificationObject randomDataSetObject = CreateRandomModifyDataSetObject(randomDateTimeOffset);
            SpecificationObject invalidDataSetObject = randomDataSetObject.DeepClone();
            SpecificationObject storageDataSetObject = invalidDataSetObject.DeepClone();
            storageDataSetObject.CreatedDate = storageDataSetObject.CreatedDate.AddMinutes(randomMinutes);
            storageDataSetObject.UpdatedDate = storageDataSetObject.UpdatedDate.AddMinutes(randomMinutes);

            var invalidDataSetObjectException =
                new InvalidDataSetObjectException(
                    message: "Invalid dataSetObject. Please correct the errors and try again.");

            invalidDataSetObjectException.AddData(
                key: nameof(SpecificationObject.CreatedDate),
                values: $"Date is not the same as {nameof(SpecificationObject.CreatedDate)}");

            var expectedDataSetObjectValidationException =
                new DataSetObjectValidationException(
                    message: "DataSetObject validation errors occurred, please try again.",
                    innerException: invalidDataSetObjectException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetObjectByIdAsync(invalidDataSetObject.Id))
                .ReturnsAsync(storageDataSetObject);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<SpecificationObject> modifyDataSetObjectTask =
                this.dataSetObjectService.ModifyDataSetObjectAsync(invalidDataSetObject);

            DataSetObjectValidationException actualDataSetObjectValidationException =
                await Assert.ThrowsAsync<DataSetObjectValidationException>(
                    modifyDataSetObjectTask.AsTask);

            // then
            actualDataSetObjectValidationException.Should()
                .BeEquivalentTo(expectedDataSetObjectValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetObjectByIdAsync(invalidDataSetObject.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDataSetObjectValidationException))),
                       Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCreatedUserDontMacthStorageAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            SpecificationObject randomDataSetObject = CreateRandomModifyDataSetObject(randomDateTimeOffset);
            SpecificationObject invalidDataSetObject = randomDataSetObject.DeepClone();
            SpecificationObject storageDataSetObject = invalidDataSetObject.DeepClone();
            invalidDataSetObject.CreatedBy = Guid.NewGuid().ToString();
            storageDataSetObject.UpdatedDate = storageDataSetObject.CreatedDate;

            var invalidDataSetObjectException =
                new InvalidDataSetObjectException(
                    message: "Invalid dataSetObject. Please correct the errors and try again.");

            invalidDataSetObjectException.AddData(
                key: nameof(SpecificationObject.CreatedBy),
                values: $"Text is not the same as {nameof(SpecificationObject.CreatedBy)}");

            var expectedDataSetObjectValidationException =
                new DataSetObjectValidationException(
                    message: "DataSetObject validation errors occurred, please try again.",
                    innerException: invalidDataSetObjectException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetObjectByIdAsync(invalidDataSetObject.Id))
                .ReturnsAsync(storageDataSetObject);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<SpecificationObject> modifyDataSetObjectTask =
                this.dataSetObjectService.ModifyDataSetObjectAsync(invalidDataSetObject);

            DataSetObjectValidationException actualDataSetObjectValidationException =
                await Assert.ThrowsAsync<DataSetObjectValidationException>(
                    modifyDataSetObjectTask.AsTask);

            // then
            actualDataSetObjectValidationException.Should().BeEquivalentTo(expectedDataSetObjectValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetObjectByIdAsync(invalidDataSetObject.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDataSetObjectValidationException))),
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
            SpecificationObject randomDataSetObject = CreateRandomModifyDataSetObject(randomDateTimeOffset);
            SpecificationObject invalidDataSetObject = randomDataSetObject;
            SpecificationObject storageDataSetObject = randomDataSetObject.DeepClone();

            var invalidDataSetObjectException =
                new InvalidDataSetObjectException(
                    message: "Invalid dataSetObject. Please correct the errors and try again.");

            invalidDataSetObjectException.AddData(
                key: nameof(SpecificationObject.UpdatedDate),
                values: $"Date is the same as {nameof(SpecificationObject.UpdatedDate)}");

            var expectedDataSetObjectValidationException =
                new DataSetObjectValidationException(
                    message: "DataSetObject validation errors occurred, please try again.",
                    innerException: invalidDataSetObjectException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetObjectByIdAsync(invalidDataSetObject.Id))
                .ReturnsAsync(storageDataSetObject);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<SpecificationObject> modifyDataSetObjectTask =
                this.dataSetObjectService.ModifyDataSetObjectAsync(invalidDataSetObject);

            // then
            await Assert.ThrowsAsync<DataSetObjectValidationException>(
                modifyDataSetObjectTask.AsTask);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetObjectValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetObjectByIdAsync(invalidDataSetObject.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}