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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
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
            EntraUser randomEntraUser = CreateRandomEntraUser();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            var invalidObjectColumn = new ObjectColumn
            {
                SupplierColumnName = invalidText,
                OurColumnName = invalidText,
                SqlDataType = invalidText,
                CodeSystem = invalidText,
            };

            var objectColumnServiceMock = new Mock<ObjectColumnService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            objectColumnServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidObjectColumn))
                    .ReturnsAsync(invalidObjectColumn);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

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
                        $"Expected value to be '{randomEntraUser.EntraUserId}' but found " +
                        $"'{invalidObjectColumn.UpdatedBy}'."
                    ]);

            var expectedObjectColumnValidationException =
                new ObjectColumnValidationException(
                    message: "ObjectColumn validation errors occurred, please try again.",
                    innerException: invalidObjectColumnException);

            // when
            ValueTask<ObjectColumn> modifyObjectColumnTask =
                objectColumnServiceMock.Object.ModifyObjectColumnAsync(invalidObjectColumn);

            ObjectColumnValidationException actualObjectColumnValidationException =
                await Assert.ThrowsAsync<ObjectColumnValidationException>(
                    modifyObjectColumnTask.AsTask);

            //then
            actualObjectColumnValidationException.Should()
                .BeEquivalentTo(expectedObjectColumnValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedObjectColumnValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateObjectColumnAsync(It.IsAny<ObjectColumn>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfObjectColumnIsInvalidLengthAndLogItAsync()
        {
            // given 
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser(entraUserId: GetRandomStringWithLengthOf(256));

            ObjectColumn invalidObjectColumn = 
                CreateRandomModifyObjectColumn(randomDateTimeOffset, randomEntraUser.EntraUserId);

            var inputCreatedByUpdatedByString = randomEntraUser.EntraUserId;
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

            var objectColumnServiceMock = new Mock<ObjectColumnService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            objectColumnServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidObjectColumn))
                    .ReturnsAsync(invalidObjectColumn);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

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

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<ObjectColumn> modifyObjectColumnTask =
                objectColumnServiceMock.Object.ModifyObjectColumnAsync(invalidObjectColumn);

            ObjectColumnValidationException actualObjectColumnValidationException =
                await Assert.ThrowsAsync<ObjectColumnValidationException>(
                    modifyObjectColumnTask.AsTask);

            //then
            actualObjectColumnValidationException.Should()
                .BeEquivalentTo(expectedObjectColumnValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedObjectColumnValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateObjectColumnAsync(It.IsAny<ObjectColumn>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            ObjectColumn randomObjectColumn =
                CreateRandomObjectColumn(randomDateTimeOffset, randomEntraUser.EntraUserId);

            ObjectColumn invalidObjectColumn = randomObjectColumn;

            var objectColumnServiceMock = new Mock<ObjectColumnService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            objectColumnServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidObjectColumn))
                    .ReturnsAsync(invalidObjectColumn);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

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
                objectColumnServiceMock.Object.ModifyObjectColumnAsync(invalidObjectColumn);

            ObjectColumnValidationException actualObjectColumnValidationException =
                await Assert.ThrowsAsync<ObjectColumnValidationException>(
                    modifyObjectColumnTask.AsTask);

            // then
            actualObjectColumnValidationException.Should()
                .BeEquivalentTo(expectedObjectColumnValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedObjectColumnValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectObjectColumnByIdAsync(invalidObjectColumn.Id),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(int minutes)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            ObjectColumn invalidObjectColumn =
                CreateRandomModifyObjectColumn(randomDateTimeOffset, randomEntraUser.EntraUserId);

            invalidObjectColumn.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var objectColumnServiceMock = new Mock<ObjectColumnService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            objectColumnServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidObjectColumn))
                    .ReturnsAsync(invalidObjectColumn);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

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
                objectColumnServiceMock.Object.ModifyObjectColumnAsync(invalidObjectColumn);

            ObjectColumnValidationException actualObjectColumnValidationException =
                await Assert.ThrowsAsync<ObjectColumnValidationException>(
                    modifyObjectColumnTask.AsTask);

            // then
            actualObjectColumnValidationException.Should()
                .BeEquivalentTo(expectedObjectColumnValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedObjectColumnValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectObjectColumnByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfObjectColumnDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            ObjectColumn invalidObjectColumn =
                CreateRandomModifyObjectColumn(randomDateTimeOffset, randomEntraUser.EntraUserId); 
            
            ObjectColumn nonExistObjectColumn = invalidObjectColumn;
            ObjectColumn nullObjectColumn = null;

            var notFoundObjectColumnException =
                new NotFoundObjectColumnException(nonExistObjectColumn.Id);

            var expectedObjectColumnValidationException =
                new ObjectColumnValidationException(
                    message: "ObjectColumn validation errors occurred, please try again.",
                    innerException: notFoundObjectColumnException);

            var objectColumnServiceMock = new Mock<ObjectColumnService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            objectColumnServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidObjectColumn))
                    .ReturnsAsync(invalidObjectColumn);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when 
            ValueTask<ObjectColumn> modifyObjectColumnTask =
                objectColumnServiceMock.Object.ModifyObjectColumnAsync(nonExistObjectColumn);

            ObjectColumnValidationException actualObjectColumnValidationException =
                await Assert.ThrowsAsync<ObjectColumnValidationException>(
                    modifyObjectColumnTask.AsTask);

            // then
            actualObjectColumnValidationException.Should()
                .BeEquivalentTo(expectedObjectColumnValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectObjectColumnByIdAsync(nonExistObjectColumn.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedObjectColumnValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
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
            EntraUser randomEntraUser = CreateRandomEntraUser();

            ObjectColumn randomObjectColumn =
                CreateRandomModifyObjectColumn(randomDateTimeOffset, randomEntraUser.EntraUserId);

            ObjectColumn invalidObjectColumn = randomObjectColumn.DeepClone();
            ObjectColumn storageObjectColumn = invalidObjectColumn.DeepClone();
            storageObjectColumn.CreatedDate = storageObjectColumn.CreatedDate.AddMinutes(randomMinutes);
            storageObjectColumn.UpdatedDate = storageObjectColumn.UpdatedDate.AddMinutes(randomMinutes);

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

            var objectColumnServiceMock = new Mock<ObjectColumnService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            objectColumnServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidObjectColumn))
                    .ReturnsAsync(invalidObjectColumn);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<ObjectColumn> modifyObjectColumnTask =
                objectColumnServiceMock.Object.ModifyObjectColumnAsync(invalidObjectColumn);

            ObjectColumnValidationException actualObjectColumnValidationException =
                await Assert.ThrowsAsync<ObjectColumnValidationException>(
                    modifyObjectColumnTask.AsTask);

            // then
            actualObjectColumnValidationException.Should()
                .BeEquivalentTo(expectedObjectColumnValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectObjectColumnByIdAsync(invalidObjectColumn.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedObjectColumnValidationException))),
                       Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCreatedUserDontMatchStorageAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            ObjectColumn randomObjectColumn =
                CreateRandomModifyObjectColumn(randomDateTimeOffset, randomEntraUser.EntraUserId); 
            
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

            var objectColumnServiceMock = new Mock<ObjectColumnService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            objectColumnServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidObjectColumn))
                    .ReturnsAsync(invalidObjectColumn);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<ObjectColumn> modifyObjectColumnTask =
                objectColumnServiceMock.Object.ModifyObjectColumnAsync(invalidObjectColumn);

            ObjectColumnValidationException actualObjectColumnValidationException =
                await Assert.ThrowsAsync<ObjectColumnValidationException>(
                    modifyObjectColumnTask.AsTask);

            // then
            actualObjectColumnValidationException.Should().BeEquivalentTo(expectedObjectColumnValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectObjectColumnByIdAsync(invalidObjectColumn.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);        

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedObjectColumnValidationException))),
                       Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            ObjectColumn randomObjectColumn =
                CreateRandomModifyObjectColumn(randomDateTimeOffset, randomEntraUser.EntraUserId);

            ObjectColumn invalidObjectColumn = randomObjectColumn;
            ObjectColumn storageObjectColumn = randomObjectColumn.DeepClone();

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

            var objectColumnServiceMock = new Mock<ObjectColumnService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            objectColumnServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidObjectColumn))
                    .ReturnsAsync(invalidObjectColumn);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<ObjectColumn> modifyObjectColumnTask =
                objectColumnServiceMock.Object.ModifyObjectColumnAsync(invalidObjectColumn);

            // then
            await Assert.ThrowsAsync<ObjectColumnValidationException>(
                modifyObjectColumnTask.AsTask);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedObjectColumnValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectObjectColumnByIdAsync(invalidObjectColumn.Id),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}