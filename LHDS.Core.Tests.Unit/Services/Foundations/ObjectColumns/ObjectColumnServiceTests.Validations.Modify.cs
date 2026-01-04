// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.ObjectColumns;
using LHDS.Core.Models.Foundations.ObjectColumns.Exceptions;
using LHDS.Core.Services.Foundations.ObjectColumns;
using Moq;
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

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(nullObjectColumn),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedObjectColumnValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateObjectColumnAsync(It.IsAny<ObjectColumn>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfObjectColumnIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            string randomUserId = GetRandomStringWithLengthOf(50);
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            var invalidObjectColumn = new ObjectColumn
            {
                SupplierColumnName = invalidText,
                OurColumnName = invalidText,
                SqlDataType = invalidText,
                CodeSystem = invalidText,
            };

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidObjectColumn))
                    .ReturnsAsync(invalidObjectColumn);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            var invalidObjectColumnException =
                new InvalidObjectColumnException(
                    message: "Invalid objectColumn. Please correct the errors and try again.");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.Id),
                values: "Id is required");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.SpecificationObjectId),
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
                values:
                    [
                        "Date is required",
                        "Date is the same as CreatedDate",
                        $"Date is not recent"
                    ]);

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.UpdatedBy),
                values:
                    [
                        "Text is required",
                        $"Expected value to be '{randomUserId}' but found " +
                        $"'{invalidObjectColumn.UpdatedBy}'."
                    ]);

            var expectedObjectColumnValidationException =
                new ObjectColumnValidationException(
                    message: "ObjectColumn validation errors occurred, please try again.",
                    innerException: invalidObjectColumnException);

            // when
            ValueTask<ObjectColumn> modifyObjectColumnTask =
                objectColumnService.ModifyObjectColumnAsync(invalidObjectColumn);

            ObjectColumnValidationException actualObjectColumnValidationException =
                await Assert.ThrowsAsync<ObjectColumnValidationException>(
                    modifyObjectColumnTask.AsTask);

            //then
            actualObjectColumnValidationException.Should()
                .BeEquivalentTo(expectedObjectColumnValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidObjectColumn),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedObjectColumnValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateObjectColumnAsync(It.IsAny<ObjectColumn>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfObjectColumnIsInvalidLengthAndLogItAsync()
        {
            // given 
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId =  GetRandomStringWithLengthOf(256);

            ObjectColumn invalidObjectColumn = 
                CreateRandomModifyObjectColumn(randomDateTimeOffset, randomUserId);

            var inputCreatedByUpdatedByString = randomUserId;
            invalidObjectColumn.SupplierColumnName = GetRandomString(256);
            invalidObjectColumn.OurColumnName = GetRandomString(256);
            invalidObjectColumn.ColumnDescription = GetRandomString(501);
            invalidObjectColumn.PopulatedBy = GetRandomString(256);
            invalidObjectColumn.SqlDataType = GetRandomString(51);
            invalidObjectColumn.FhirDataType = GetRandomString(256);
            invalidObjectColumn.SupplierDateFormat = GetRandomString(256);
            invalidObjectColumn.PersonConfidentialDataType = GetRandomString(256);
            invalidObjectColumn.MaskingMethod = GetRandomString(256);
            invalidObjectColumn.CodeSystem = GetRandomString(256);
            invalidObjectColumn.PartitionColumnLevel = GetRandomString(256);
            invalidObjectColumn.CreatedBy = inputCreatedByUpdatedByString;
            invalidObjectColumn.UpdatedBy = inputCreatedByUpdatedByString;

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidObjectColumn))
                    .ReturnsAsync(invalidObjectColumn);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            var invalidObjectColumnException =
                new InvalidObjectColumnException(
                    message: "Invalid objectColumn. Please correct the errors and try again.");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.SupplierColumnName),
                values: "Text is exceeding max length");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.OurColumnName),
                values: "Text is exceeding max length");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.ColumnDescription),
                values: "Text is exceeding max length");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.PopulatedBy),
                values: "Text is exceeding max length");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.SqlDataType),
                values: "Text is exceeding max length");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.FhirDataType),
                values: "Text is exceeding max length");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.SupplierDateFormat),
                values: "Text is exceeding max length");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.PersonConfidentialDataType),
                values: "Text is exceeding max length");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.MaskingMethod),
                values: "Text is exceeding max length");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.CodeSystem),
                values: "Text is exceeding max length");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.PartitionColumnLevel),
                values: "Text is exceeding max length");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.CreatedBy),
                values: "Text is exceeding max length");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.UpdatedBy),
                values: "Text is exceeding max length");

            var expectedObjectColumnValidationException =
                new ObjectColumnValidationException(
                    message: "ObjectColumn validation errors occurred, please try again.",
                    innerException: invalidObjectColumnException);

            // when
            ValueTask<ObjectColumn> modifyObjectColumnTask =
                objectColumnService.ModifyObjectColumnAsync(invalidObjectColumn);

            ObjectColumnValidationException actualObjectColumnValidationException =
                await Assert.ThrowsAsync<ObjectColumnValidationException>(
                    modifyObjectColumnTask.AsTask);

            //then
            actualObjectColumnValidationException.Should()
                .BeEquivalentTo(expectedObjectColumnValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidObjectColumn),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedObjectColumnValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateObjectColumnAsync(It.IsAny<ObjectColumn>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            ObjectColumn randomObjectColumn =
                CreateRandomObjectColumn(randomDateTimeOffset, randomUserId);

            ObjectColumn invalidObjectColumn = randomObjectColumn;

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidObjectColumn))
                    .ReturnsAsync(invalidObjectColumn);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

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

            // when
            ValueTask<ObjectColumn> modifyObjectColumnTask =
                objectColumnService.ModifyObjectColumnAsync(invalidObjectColumn);

            ObjectColumnValidationException actualObjectColumnValidationException =
                await Assert.ThrowsAsync<ObjectColumnValidationException>(
                    modifyObjectColumnTask.AsTask);

            // then
            actualObjectColumnValidationException.Should()
                .BeEquivalentTo(expectedObjectColumnValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidObjectColumn),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedObjectColumnValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectObjectColumnByIdAsync(invalidObjectColumn.Id),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
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
            string randomUserId = GetRandomStringWithLengthOf(50);

            ObjectColumn invalidObjectColumn =
                CreateRandomObjectColumn(randomDateTimeOffset, randomUserId);

            invalidObjectColumn.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidObjectColumn))
                    .ReturnsAsync(invalidObjectColumn);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

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
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<ObjectColumn> modifyObjectColumnTask =
                objectColumnService.ModifyObjectColumnAsync(invalidObjectColumn);

            ObjectColumnValidationException actualObjectColumnValidationException =
                await Assert.ThrowsAsync<ObjectColumnValidationException>(
                    modifyObjectColumnTask.AsTask);

            // then
            actualObjectColumnValidationException.Should()
                .BeEquivalentTo(expectedObjectColumnValidatonException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidObjectColumn),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedObjectColumnValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectObjectColumnByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfObjectColumnDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            ObjectColumn invalidObjectColumn =
                CreateRandomModifyObjectColumn(randomDateTimeOffset, randomUserId); 
            
            ObjectColumn nonExistObjectColumn = invalidObjectColumn;
            var notFoundObjectColumnException = new NotFoundObjectColumnException(nonExistObjectColumn.Id);

            var expectedObjectColumnValidationException =
                new ObjectColumnValidationException(
                    message: "ObjectColumn validation errors occurred, please try again.",
                    innerException: notFoundObjectColumnException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidObjectColumn))
                    .ReturnsAsync(invalidObjectColumn);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            // when 
            ValueTask<ObjectColumn> modifyObjectColumnTask =
                objectColumnService.ModifyObjectColumnAsync(nonExistObjectColumn);

            ObjectColumnValidationException actualObjectColumnValidationException =
                await Assert.ThrowsAsync<ObjectColumnValidationException>(
                    modifyObjectColumnTask.AsTask);

            // then
            actualObjectColumnValidationException.Should()
                .BeEquivalentTo(expectedObjectColumnValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(nonExistObjectColumn),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectObjectColumnByIdAsync(nonExistObjectColumn.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedObjectColumnValidationException))),
                        Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNegativeNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            ObjectColumn randomObjectColumn =
                CreateRandomModifyObjectColumn(randomDateTimeOffset, randomUserId);

            ObjectColumn invalidObjectColumn = randomObjectColumn.DeepClone();
            ObjectColumn storageObjectColumn = invalidObjectColumn.DeepClone();
            storageObjectColumn.CreatedDate = storageObjectColumn.CreatedDate.AddMinutes(randomMinutes);
            storageObjectColumn.UpdatedDate = storageObjectColumn.UpdatedDate.AddMinutes(randomMinutes);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidObjectColumn))
                    .ReturnsAsync(invalidObjectColumn);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            var invalidObjectColumnException =
                new InvalidObjectColumnException(
                    message: "Invalid objectColumn. Please correct the errors and try again.");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.CreatedDate),
                values: $"Date is not the same as {nameof(ObjectColumn.CreatedDate)}");

            var expectedObjectColumnValidationException =
                new ObjectColumnValidationException(
                    message: "ObjectColumn validation errors occurred, please try again.",
                    innerException: invalidObjectColumnException);
            
            this.storageBrokerMock.Setup(broker =>
                broker.SelectObjectColumnByIdAsync(invalidObjectColumn.Id))
                    .ReturnsAsync(storageObjectColumn);

            // when
            ValueTask<ObjectColumn> modifyObjectColumnTask =
                objectColumnService.ModifyObjectColumnAsync(invalidObjectColumn);

            ObjectColumnValidationException actualObjectColumnValidationException =
                await Assert.ThrowsAsync<ObjectColumnValidationException>(
                    modifyObjectColumnTask.AsTask);

            // then
            actualObjectColumnValidationException.Should()
                .BeEquivalentTo(expectedObjectColumnValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidObjectColumn),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectObjectColumnByIdAsync(invalidObjectColumn.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedObjectColumnValidationException))),
                       Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCreatedUserDontMatchStorageAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            ObjectColumn randomObjectColumn =
                CreateRandomModifyObjectColumn(randomDateTimeOffset, randomUserId); 
            
            ObjectColumn invalidObjectColumn = randomObjectColumn.DeepClone();
            ObjectColumn storageObjectColumn = invalidObjectColumn.DeepClone();
            invalidObjectColumn.CreatedBy = Guid.NewGuid().ToString();
            storageObjectColumn.UpdatedDate = storageObjectColumn.CreatedDate;

            var invalidObjectColumnException =
                new InvalidObjectColumnException(
                    message: "Invalid objectColumn. Please correct the errors and try again.");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.CreatedBy),
                values: $"Text is not the same as {nameof(ObjectColumn.CreatedBy)}");

            var expectedObjectColumnValidationException =
                new ObjectColumnValidationException(
                    message: "ObjectColumn validation errors occurred, please try again.",
                    innerException: invalidObjectColumnException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidObjectColumn))
                    .ReturnsAsync(invalidObjectColumn);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectObjectColumnByIdAsync(invalidObjectColumn.Id))
                    .ReturnsAsync(storageObjectColumn);

            // when
            ValueTask<ObjectColumn> modifyObjectColumnTask =
                objectColumnService.ModifyObjectColumnAsync(invalidObjectColumn);

            ObjectColumnValidationException actualObjectColumnValidationException =
                await Assert.ThrowsAsync<ObjectColumnValidationException>(
                    modifyObjectColumnTask.AsTask);

            // then
            actualObjectColumnValidationException.Should().BeEquivalentTo(expectedObjectColumnValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidObjectColumn),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once); 
            
            this.storageBrokerMock.Verify(broker =>
                broker.SelectObjectColumnByIdAsync(invalidObjectColumn.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedObjectColumnValidationException))),
                       Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            ObjectColumn randomObjectColumn =
                CreateRandomModifyObjectColumn(randomDateTimeOffset, randomUserId);

            ObjectColumn invalidObjectColumn = randomObjectColumn;
            ObjectColumn storageObjectColumn = randomObjectColumn.DeepClone();
            invalidObjectColumn.UpdatedDate = storageObjectColumn.UpdatedDate;

            var invalidObjectColumnException =
                new InvalidObjectColumnException(
                    message: "Invalid objectColumn. Please correct the errors and try again.");

            invalidObjectColumnException.AddData(
                key: nameof(ObjectColumn.UpdatedDate),
                values: $"Date is the same as {nameof(ObjectColumn.UpdatedDate)}");

            var expectedObjectColumnValidationException =
                new ObjectColumnValidationException(
                    message: "ObjectColumn validation errors occurred, please try again.",
                    innerException: invalidObjectColumnException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidObjectColumn))
                    .ReturnsAsync(invalidObjectColumn);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            this.storageBrokerMock.Setup(broker => 
                broker.SelectObjectColumnByIdAsync(invalidObjectColumn.Id))
                    .ReturnsAsync(storageObjectColumn);

            // when
            ValueTask <ObjectColumn> modifyObjectColumnTask =
                objectColumnService.ModifyObjectColumnAsync(invalidObjectColumn);

            // then
            await Assert.ThrowsAsync<ObjectColumnValidationException>(
                modifyObjectColumnTask.AsTask);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidObjectColumn),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedObjectColumnValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectObjectColumnByIdAsync(invalidObjectColumn.Id),
                    Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}