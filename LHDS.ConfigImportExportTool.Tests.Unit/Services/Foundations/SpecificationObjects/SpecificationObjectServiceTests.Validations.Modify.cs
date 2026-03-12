// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects.Exceptions;
using Moq;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Foundations.SpecificationObjects
{
    public partial class SpecificationObjectServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfSpecificationObjectIsNullAndLogItAsync()
        {
            // given
            SpecificationObject nullSpecificationObject = null;
            var nullSpecificationObjectException = new NullSpecificationObjectException(message: "SpecificationObject is null.");

            var expectedSpecificationObjectValidationException =
                new SpecificationObjectValidationException(
                    message: "SpecificationObject validation errors occurred, please try again.",
                    innerException: nullSpecificationObjectException);

            // when
            ValueTask<SpecificationObject> modifySpecificationObjectTask =
                this.specificationObjectService.ModifySpecificationObjectAsync(nullSpecificationObject);

            SpecificationObjectValidationException actualSpecificationObjectValidationException =
                await Assert.ThrowsAsync<SpecificationObjectValidationException>(
                    modifySpecificationObjectTask.AsTask);

            // then
            actualSpecificationObjectValidationException.Should()
                .BeEquivalentTo(expectedSpecificationObjectValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSpecificationObjectValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSpecificationObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfSpecificationObjectIsInvalidAndLogItAsync(string invalidText)
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
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(SpecificationObject.CreatedDate)}"
                });

            invalidSpecificationObjectException.AddData(
                key: nameof(SpecificationObject.UpdatedBy),
                values: "Text is required");

            var expectedSpecificationObjectValidationException =
                new SpecificationObjectValidationException(
                    message: "SpecificationObject validation errors occurred, please try again.",
                    innerException: invalidSpecificationObjectException);

            // when
            ValueTask<SpecificationObject> modifySpecificationObjectTask =
                this.specificationObjectService.ModifySpecificationObjectAsync(invalidSpecificationObject);

            SpecificationObjectValidationException actualSpecificationObjectValidationException =
                await Assert.ThrowsAsync<SpecificationObjectValidationException>(
                    modifySpecificationObjectTask.AsTask);

            //then
            actualSpecificationObjectValidationException.Should()
                .BeEquivalentTo(expectedSpecificationObjectValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSpecificationObjectValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSpecificationObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfSpecificationObjectIsInvalidLengthAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            SpecificationObject invalidSpecificationObject = CreateRandomModifySpecificationObject(randomDateTimeOffset);
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
            ValueTask<SpecificationObject> modifySpecificationObjectTask =
                this.specificationObjectService.ModifySpecificationObjectAsync(invalidSpecificationObject);

            SpecificationObjectValidationException actualSpecificationObjectValidationException =
                await Assert.ThrowsAsync<SpecificationObjectValidationException>(modifySpecificationObjectTask.AsTask);

            // then
            actualSpecificationObjectValidationException.Should()
                .BeEquivalentTo(expectedSpecificationObjectValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSpecificationObjectValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSpecificationObjectAsync(It.IsAny<SpecificationObject>()),
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
            SpecificationObject randomSpecificationObject = CreateRandomSpecificationObject(randomDateTimeOffset);
            SpecificationObject invalidSpecificationObject = randomSpecificationObject;

            var invalidSpecificationObjectException =
                new InvalidSpecificationObjectException(
                    message: "Invalid specificationObject. Please correct the errors and try again.");

            invalidSpecificationObjectException.AddData(
                key: nameof(SpecificationObject.UpdatedDate),
                values: $"Date is the same as {nameof(SpecificationObject.CreatedDate)}");

            var expectedSpecificationObjectValidationException =
                new SpecificationObjectValidationException(
                    message: "SpecificationObject validation errors occurred, please try again.",
                    innerException: invalidSpecificationObjectException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<SpecificationObject> modifySpecificationObjectTask =
                this.specificationObjectService.ModifySpecificationObjectAsync(invalidSpecificationObject);

            SpecificationObjectValidationException actualSpecificationObjectValidationException =
                await Assert.ThrowsAsync<SpecificationObjectValidationException>(
                    modifySpecificationObjectTask.AsTask);

            // then
            actualSpecificationObjectValidationException.Should()
                .BeEquivalentTo(expectedSpecificationObjectValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSpecificationObjectValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSpecificationObjectByIdAsync(invalidSpecificationObject.Id),
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
            SpecificationObject randomSpecificationObject = CreateRandomSpecificationObject(randomDateTimeOffset);
            randomSpecificationObject.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var invalidSpecificationObjectException =
                new InvalidSpecificationObjectException(
                    message: "Invalid specificationObject. Please correct the errors and try again.");

            invalidSpecificationObjectException.AddData(
                key: nameof(SpecificationObject.UpdatedDate),
                values: "Date is not recent");

            var expectedSpecificationObjectValidatonException =
                new SpecificationObjectValidationException(
                    message: "SpecificationObject validation errors occurred, please try again.",
                    innerException: invalidSpecificationObjectException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<SpecificationObject> modifySpecificationObjectTask =
                this.specificationObjectService.ModifySpecificationObjectAsync(randomSpecificationObject);

            SpecificationObjectValidationException actualSpecificationObjectValidationException =
                await Assert.ThrowsAsync<SpecificationObjectValidationException>(
                    modifySpecificationObjectTask.AsTask);

            // then
            actualSpecificationObjectValidationException.Should()
                .BeEquivalentTo(expectedSpecificationObjectValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSpecificationObjectValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSpecificationObjectByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfSpecificationObjectDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            SpecificationObject randomSpecificationObject = CreateRandomModifySpecificationObject(randomDateTimeOffset);
            SpecificationObject nonExistSpecificationObject = randomSpecificationObject;
            SpecificationObject nullSpecificationObject = null;

            var notFoundSpecificationObjectException =
                new NotFoundSpecificationObjectException(
                    message: $"Couldn't find specificationObject with specificationObjectId: " +
                        $"{nonExistSpecificationObject.Id}.");

            var expectedSpecificationObjectValidationException =
                new SpecificationObjectValidationException(
                    message: "SpecificationObject validation errors occurred, please try again.",
                    innerException: notFoundSpecificationObjectException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSpecificationObjectByIdAsync(nonExistSpecificationObject.Id))
                .ReturnsAsync(nullSpecificationObject);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            // when 
            ValueTask<SpecificationObject> modifySpecificationObjectTask =
                this.specificationObjectService.ModifySpecificationObjectAsync(nonExistSpecificationObject);

            SpecificationObjectValidationException actualSpecificationObjectValidationException =
                await Assert.ThrowsAsync<SpecificationObjectValidationException>(
                    modifySpecificationObjectTask.AsTask);

            // then
            actualSpecificationObjectValidationException.Should()
                .BeEquivalentTo(expectedSpecificationObjectValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSpecificationObjectByIdAsync(nonExistSpecificationObject.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSpecificationObjectValidationException))),
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
            SpecificationObject randomSpecificationObject = CreateRandomModifySpecificationObject(randomDateTimeOffset);
            SpecificationObject invalidSpecificationObject = randomSpecificationObject.DeepClone();
            SpecificationObject storageSpecificationObject = invalidSpecificationObject.DeepClone();
            storageSpecificationObject.CreatedDate = storageSpecificationObject.CreatedDate.AddMinutes(randomMinutes);
            storageSpecificationObject.UpdatedDate = storageSpecificationObject.UpdatedDate.AddMinutes(randomMinutes);

            var invalidSpecificationObjectException =
                new InvalidSpecificationObjectException(
                    message: "Invalid specificationObject. Please correct the errors and try again.");

            invalidSpecificationObjectException.AddData(
                key: nameof(SpecificationObject.CreatedDate),
                values: $"Date is not the same as {nameof(SpecificationObject.CreatedDate)}");

            var expectedSpecificationObjectValidationException =
                new SpecificationObjectValidationException(
                    message: "SpecificationObject validation errors occurred, please try again.",
                    innerException: invalidSpecificationObjectException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSpecificationObjectByIdAsync(invalidSpecificationObject.Id))
                .ReturnsAsync(storageSpecificationObject);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<SpecificationObject> modifySpecificationObjectTask =
                this.specificationObjectService.ModifySpecificationObjectAsync(invalidSpecificationObject);

            SpecificationObjectValidationException actualSpecificationObjectValidationException =
                await Assert.ThrowsAsync<SpecificationObjectValidationException>(
                    modifySpecificationObjectTask.AsTask);

            // then
            actualSpecificationObjectValidationException.Should()
                .BeEquivalentTo(expectedSpecificationObjectValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSpecificationObjectByIdAsync(invalidSpecificationObject.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedSpecificationObjectValidationException))),
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
            SpecificationObject randomSpecificationObject = CreateRandomModifySpecificationObject(randomDateTimeOffset);
            SpecificationObject invalidSpecificationObject = randomSpecificationObject.DeepClone();
            SpecificationObject storageSpecificationObject = invalidSpecificationObject.DeepClone();
            invalidSpecificationObject.CreatedBy = Guid.NewGuid().ToString();
            storageSpecificationObject.UpdatedDate = storageSpecificationObject.CreatedDate;

            var invalidSpecificationObjectException =
                new InvalidSpecificationObjectException(
                    message: "Invalid specificationObject. Please correct the errors and try again.");

            invalidSpecificationObjectException.AddData(
                key: nameof(SpecificationObject.CreatedBy),
                values: $"Text is not the same as {nameof(SpecificationObject.CreatedBy)}");

            var expectedSpecificationObjectValidationException =
                new SpecificationObjectValidationException(
                    message: "SpecificationObject validation errors occurred, please try again.",
                    innerException: invalidSpecificationObjectException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSpecificationObjectByIdAsync(invalidSpecificationObject.Id))
                .ReturnsAsync(storageSpecificationObject);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<SpecificationObject> modifySpecificationObjectTask =
                this.specificationObjectService.ModifySpecificationObjectAsync(invalidSpecificationObject);

            SpecificationObjectValidationException actualSpecificationObjectValidationException =
                await Assert.ThrowsAsync<SpecificationObjectValidationException>(
                    modifySpecificationObjectTask.AsTask);

            // then
            actualSpecificationObjectValidationException.Should().BeEquivalentTo(expectedSpecificationObjectValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSpecificationObjectByIdAsync(invalidSpecificationObject.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedSpecificationObjectValidationException))),
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
            SpecificationObject randomSpecificationObject = CreateRandomModifySpecificationObject(randomDateTimeOffset);
            SpecificationObject invalidSpecificationObject = randomSpecificationObject;
            SpecificationObject storageSpecificationObject = randomSpecificationObject.DeepClone();

            var invalidSpecificationObjectException =
                new InvalidSpecificationObjectException(
                    message: "Invalid specificationObject. Please correct the errors and try again.");

            invalidSpecificationObjectException.AddData(
                key: nameof(SpecificationObject.UpdatedDate),
                values: $"Date is the same as {nameof(SpecificationObject.UpdatedDate)}");

            var expectedSpecificationObjectValidationException =
                new SpecificationObjectValidationException(
                    message: "SpecificationObject validation errors occurred, please try again.",
                    innerException: invalidSpecificationObjectException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSpecificationObjectByIdAsync(invalidSpecificationObject.Id))
                .ReturnsAsync(storageSpecificationObject);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<SpecificationObject> modifySpecificationObjectTask =
                this.specificationObjectService.ModifySpecificationObjectAsync(invalidSpecificationObject);

            // then
            await Assert.ThrowsAsync<SpecificationObjectValidationException>(
                modifySpecificationObjectTask.AsTask);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSpecificationObjectValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSpecificationObjectByIdAsync(invalidSpecificationObject.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}