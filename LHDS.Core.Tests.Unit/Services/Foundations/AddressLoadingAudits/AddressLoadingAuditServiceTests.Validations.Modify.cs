// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using LHDS.Core.Models.Foundations.AddressLoadingAudits.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressLoadingAudits
{
    public partial class AddressLoadingAuditServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfAddressLoadingAuditIsNullAndLogItAsync()
        {
            // given
            AddressLoadingAudit nullAddressLoadingAudit = null;
            var nullAddressLoadingAuditException = new NullAddressLoadingAuditException(message: "AddressLoadingAudit is null.");

            var expectedAddressLoadingAuditValidationException =
                new AddressLoadingAuditValidationException(
                    message: "AddressLoadingAudit validation errors occurred, please try again.",
                    innerException: nullAddressLoadingAuditException);

            // when
            ValueTask<AddressLoadingAudit> modifyAddressLoadingAuditTask =
                this.addressLoadingAuditService.ModifyAddressLoadingAuditAsync(nullAddressLoadingAudit);

            AddressLoadingAuditValidationException actualAddressLoadingAuditValidationException =
                await Assert.ThrowsAsync<AddressLoadingAuditValidationException>(
                    modifyAddressLoadingAuditTask.AsTask);

            // then
            actualAddressLoadingAuditValidationException.Should()
                .BeEquivalentTo(expectedAddressLoadingAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressLoadingAuditValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressLoadingAuditAsync(It.IsAny<AddressLoadingAudit>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfAddressLoadingAuditIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            var invalidAddressLoadingAudit = new AddressLoadingAudit
            {
                // TODO:  Add default values for your properties i.e. Name = invalidText
            };

            var invalidAddressLoadingAuditException =
                new InvalidAddressLoadingAuditException(
                    message: "Invalid addressLoadingAudit. Please correct the errors and try again.");

            invalidAddressLoadingAuditException.AddData(
                key: nameof(AddressLoadingAudit.Id),
                values: "Id is required");

            //invalidAddressLoadingAuditException.AddData(
            //    key: nameof(AddressLoadingAudit.Name),
            //    values: "Text is required");

            // TODO: Add or remove data here to suit the validation needs for the AddressLoadingAudit model

            invalidAddressLoadingAuditException.AddData(
                key: nameof(AddressLoadingAudit.CreatedDate),
                values: "Date is required");

            invalidAddressLoadingAuditException.AddData(
                key: nameof(AddressLoadingAudit.CreatedBy),
                values: "Text is required");

            invalidAddressLoadingAuditException.AddData(
                key: nameof(AddressLoadingAudit.UpdatedDate),
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(AddressLoadingAudit.CreatedDate)}"
                });

            invalidAddressLoadingAuditException.AddData(
                key: nameof(AddressLoadingAudit.UpdatedBy),
                values: "Text is required");

            var expectedAddressLoadingAuditValidationException =
                new AddressLoadingAuditValidationException(
                    message: "AddressLoadingAudit validation errors occurred, please try again.",
                    innerException: invalidAddressLoadingAuditException);

            // when
            ValueTask<AddressLoadingAudit> modifyAddressLoadingAuditTask =
                this.addressLoadingAuditService.ModifyAddressLoadingAuditAsync(invalidAddressLoadingAudit);

            AddressLoadingAuditValidationException actualAddressLoadingAuditValidationException =
                await Assert.ThrowsAsync<AddressLoadingAuditValidationException>(
                    modifyAddressLoadingAuditTask.AsTask);

            //then
            actualAddressLoadingAuditValidationException.Should()
                .BeEquivalentTo(expectedAddressLoadingAuditValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressLoadingAuditValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressLoadingAuditAsync(It.IsAny<AddressLoadingAudit>()),
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
            AddressLoadingAudit randomAddressLoadingAudit = CreateRandomAddressLoadingAudit(randomDateTimeOffset);
            AddressLoadingAudit invalidAddressLoadingAudit = randomAddressLoadingAudit;

            var invalidAddressLoadingAuditException =
                new InvalidAddressLoadingAuditException(
                    message: "Invalid addressLoadingAudit. Please correct the errors and try again.");

            invalidAddressLoadingAuditException.AddData(
                key: nameof(AddressLoadingAudit.UpdatedDate),
                values: $"Date is the same as {nameof(AddressLoadingAudit.CreatedDate)}");

            var expectedAddressLoadingAuditValidationException =
                new AddressLoadingAuditValidationException(
                    message: "AddressLoadingAudit validation errors occurred, please try again.",
                    innerException: invalidAddressLoadingAuditException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<AddressLoadingAudit> modifyAddressLoadingAuditTask =
                this.addressLoadingAuditService.ModifyAddressLoadingAuditAsync(invalidAddressLoadingAudit);

            AddressLoadingAuditValidationException actualAddressLoadingAuditValidationException =
                await Assert.ThrowsAsync<AddressLoadingAuditValidationException>(
                    modifyAddressLoadingAuditTask.AsTask);

            // then
            actualAddressLoadingAuditValidationException.Should()
                .BeEquivalentTo(expectedAddressLoadingAuditValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressLoadingAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(invalidAddressLoadingAudit.Id),
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
            AddressLoadingAudit randomAddressLoadingAudit = CreateRandomAddressLoadingAudit(randomDateTimeOffset);
            randomAddressLoadingAudit.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var invalidAddressLoadingAuditException =
                new InvalidAddressLoadingAuditException(
                    message: "Invalid addressLoadingAudit. Please correct the errors and try again.");

            invalidAddressLoadingAuditException.AddData(
                key: nameof(AddressLoadingAudit.UpdatedDate),
                values: "Date is not recent");

            var expectedAddressLoadingAuditValidatonException =
                new AddressLoadingAuditValidationException(
                    message: "AddressLoadingAudit validation errors occurred, please try again.",
                    innerException: invalidAddressLoadingAuditException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<AddressLoadingAudit> modifyAddressLoadingAuditTask =
                this.addressLoadingAuditService.ModifyAddressLoadingAuditAsync(randomAddressLoadingAudit);

            AddressLoadingAuditValidationException actualAddressLoadingAuditValidationException =
                await Assert.ThrowsAsync<AddressLoadingAuditValidationException>(
                    modifyAddressLoadingAuditTask.AsTask);

            // then
            actualAddressLoadingAuditValidationException.Should()
                .BeEquivalentTo(expectedAddressLoadingAuditValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressLoadingAuditValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfAddressLoadingAuditDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            AddressLoadingAudit randomAddressLoadingAudit = CreateRandomModifyAddressLoadingAudit(randomDateTimeOffset);
            AddressLoadingAudit nonExistAddressLoadingAudit = randomAddressLoadingAudit;
            AddressLoadingAudit nullAddressLoadingAudit = null;

            var notFoundAddressLoadingAuditException =
                new NotFoundAddressLoadingAuditException(nonExistAddressLoadingAudit.Id);

            var expectedAddressLoadingAuditValidationException =
                new AddressLoadingAuditValidationException(
                    message: "AddressLoadingAudit validation errors occurred, please try again.",
                    innerException: notFoundAddressLoadingAuditException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(nonExistAddressLoadingAudit.Id))
                .ReturnsAsync(nullAddressLoadingAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when 
            ValueTask<AddressLoadingAudit> modifyAddressLoadingAuditTask =
                this.addressLoadingAuditService.ModifyAddressLoadingAuditAsync(nonExistAddressLoadingAudit);

            AddressLoadingAuditValidationException actualAddressLoadingAuditValidationException =
                await Assert.ThrowsAsync<AddressLoadingAuditValidationException>(
                    modifyAddressLoadingAuditTask.AsTask);

            // then
            actualAddressLoadingAuditValidationException.Should()
                .BeEquivalentTo(expectedAddressLoadingAuditValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(nonExistAddressLoadingAudit.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressLoadingAuditValidationException))),
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
            AddressLoadingAudit randomAddressLoadingAudit = CreateRandomModifyAddressLoadingAudit(randomDateTimeOffset);
            AddressLoadingAudit invalidAddressLoadingAudit = randomAddressLoadingAudit.DeepClone();
            AddressLoadingAudit storageAddressLoadingAudit = invalidAddressLoadingAudit.DeepClone();
            storageAddressLoadingAudit.CreatedDate = storageAddressLoadingAudit.CreatedDate.AddMinutes(randomMinutes);
            storageAddressLoadingAudit.UpdatedDate = storageAddressLoadingAudit.UpdatedDate.AddMinutes(randomMinutes);

            var invalidAddressLoadingAuditException =
                new InvalidAddressLoadingAuditException(
                    message: "Invalid addressLoadingAudit. Please correct the errors and try again.");

            invalidAddressLoadingAuditException.AddData(
                key: nameof(AddressLoadingAudit.CreatedDate),
                values: $"Date is not the same as {nameof(AddressLoadingAudit.CreatedDate)}");

            var expectedAddressLoadingAuditValidationException =
                new AddressLoadingAuditValidationException(
                    message: "AddressLoadingAudit validation errors occurred, please try again.",
                    innerException: invalidAddressLoadingAuditException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(invalidAddressLoadingAudit.Id))
                .ReturnsAsync(storageAddressLoadingAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<AddressLoadingAudit> modifyAddressLoadingAuditTask =
                this.addressLoadingAuditService.ModifyAddressLoadingAuditAsync(invalidAddressLoadingAudit);

            AddressLoadingAuditValidationException actualAddressLoadingAuditValidationException =
                await Assert.ThrowsAsync<AddressLoadingAuditValidationException>(
                    modifyAddressLoadingAuditTask.AsTask);

            // then
            actualAddressLoadingAuditValidationException.Should()
                .BeEquivalentTo(expectedAddressLoadingAuditValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(invalidAddressLoadingAudit.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedAddressLoadingAuditValidationException))),
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
            AddressLoadingAudit randomAddressLoadingAudit = CreateRandomModifyAddressLoadingAudit(randomDateTimeOffset);
            AddressLoadingAudit invalidAddressLoadingAudit = randomAddressLoadingAudit.DeepClone();
            AddressLoadingAudit storageAddressLoadingAudit = invalidAddressLoadingAudit.DeepClone();
            invalidAddressLoadingAudit.CreatedBy = Guid.NewGuid().ToString();
            storageAddressLoadingAudit.UpdatedDate = storageAddressLoadingAudit.CreatedDate;

            var invalidAddressLoadingAuditException =
                new InvalidAddressLoadingAuditException(
                    message: "Invalid addressLoadingAudit. Please correct the errors and try again.");

            invalidAddressLoadingAuditException.AddData(
                key: nameof(AddressLoadingAudit.CreatedBy),
                values: $"Text is not the same as {nameof(AddressLoadingAudit.CreatedBy)}");

            var expectedAddressLoadingAuditValidationException =
                new AddressLoadingAuditValidationException(
                    message: "AddressLoadingAudit validation errors occurred, please try again.",
                    innerException: invalidAddressLoadingAuditException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(invalidAddressLoadingAudit.Id))
                .ReturnsAsync(storageAddressLoadingAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<AddressLoadingAudit> modifyAddressLoadingAuditTask =
                this.addressLoadingAuditService.ModifyAddressLoadingAuditAsync(invalidAddressLoadingAudit);

            AddressLoadingAuditValidationException actualAddressLoadingAuditValidationException =
                await Assert.ThrowsAsync<AddressLoadingAuditValidationException>(
                    modifyAddressLoadingAuditTask.AsTask);

            // then
            actualAddressLoadingAuditValidationException.Should().BeEquivalentTo(expectedAddressLoadingAuditValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(invalidAddressLoadingAudit.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedAddressLoadingAuditValidationException))),
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
            AddressLoadingAudit randomAddressLoadingAudit = CreateRandomModifyAddressLoadingAudit(randomDateTimeOffset);
            AddressLoadingAudit invalidAddressLoadingAudit = randomAddressLoadingAudit;
            AddressLoadingAudit storageAddressLoadingAudit = randomAddressLoadingAudit.DeepClone();

            var invalidAddressLoadingAuditException =
                new InvalidAddressLoadingAuditException(
                    message: "Invalid addressLoadingAudit. Please correct the errors and try again.");

            invalidAddressLoadingAuditException.AddData(
                key: nameof(AddressLoadingAudit.UpdatedDate),
                values: $"Date is the same as {nameof(AddressLoadingAudit.UpdatedDate)}");

            var expectedAddressLoadingAuditValidationException =
                new AddressLoadingAuditValidationException(
                    message: "AddressLoadingAudit validation errors occurred, please try again.",
                    innerException: invalidAddressLoadingAuditException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(invalidAddressLoadingAudit.Id))
                .ReturnsAsync(storageAddressLoadingAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<AddressLoadingAudit> modifyAddressLoadingAuditTask =
                this.addressLoadingAuditService.ModifyAddressLoadingAuditAsync(invalidAddressLoadingAudit);

            // then
            await Assert.ThrowsAsync<AddressLoadingAuditValidationException>(
                modifyAddressLoadingAuditTask.AsTask);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressLoadingAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(invalidAddressLoadingAudit.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}