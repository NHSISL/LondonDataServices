using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.AddressExtractionAudits;
using LHDS.Core.Models.Foundations.AddressExtractionAudits.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressExtractionAudits
{
    public partial class AddressExtractionAuditServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfAddressExtractionAuditIsNullAndLogItAsync()
        {
            // given
            AddressExtractionAudit nullAddressExtractionAudit = null;

            var nullAddressExtractionAuditException =
                new NullAddressExtractionAuditException(message: "AddressExtractionAudit is null.");

            var expectedAddressExtractionAuditValidationException =
                new AddressExtractionAuditValidationException(
                    message: "AddressExtractionAudit validation errors occurred, please try again.",
                    innerException: nullAddressExtractionAuditException);

            // when
            ValueTask<AddressExtractionAudit> addAddressExtractionAuditTask =
                this.addressExtractionAuditService.AddAddressExtractionAuditAsync(nullAddressExtractionAudit);

            AddressExtractionAuditValidationException actualAddressExtractionAuditValidationException =
                await Assert.ThrowsAsync<AddressExtractionAuditValidationException>(
                    addAddressExtractionAuditTask.AsTask);

            // then
            actualAddressExtractionAuditValidationException.Should()
                .BeEquivalentTo(expectedAddressExtractionAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressExtractionAuditValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfAddressExtractionAuditIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            var invalidAddressExtractionAudit = new AddressExtractionAudit
            {
                // TODO:  Add default values for your properties i.e. Name = invalidText
            };

            var invalidAddressExtractionAuditException =
                new InvalidAddressExtractionAuditException(
                    message: "Invalid addressExtractionAudit. Please correct the errors and try again.");

            invalidAddressExtractionAuditException.AddData(
                key: nameof(AddressExtractionAudit.Id),
                values: "Id is required");

            //invalidAddressExtractionAuditException.AddData(
            //    key: nameof(AddressExtractionAudit.Name),
            //    values: "Text is required");

            // TODO: Add or remove data here to suit the validation needs for the AddressExtractionAudit model

            invalidAddressExtractionAuditException.AddData(
                key: nameof(AddressExtractionAudit.CreatedDate),
                values: "Date is required");

            invalidAddressExtractionAuditException.AddData(
                key: nameof(AddressExtractionAudit.CreatedBy),
                values: "Text is required");

            invalidAddressExtractionAuditException.AddData(
                key: nameof(AddressExtractionAudit.UpdatedDate),
                values: "Date is required");

            invalidAddressExtractionAuditException.AddData(
                key: nameof(AddressExtractionAudit.UpdatedBy),
                values: "Text is required");

            var expectedAddressExtractionAuditValidationException =
                new AddressExtractionAuditValidationException(
                    message: "AddressExtractionAudit validation errors occurred, please try again.",
                    innerException: invalidAddressExtractionAuditException);

            // when
            ValueTask<AddressExtractionAudit> addAddressExtractionAuditTask =
                this.addressExtractionAuditService.AddAddressExtractionAuditAsync(invalidAddressExtractionAudit);

            AddressExtractionAuditValidationException actualAddressExtractionAuditValidationException =
                await Assert.ThrowsAsync<AddressExtractionAuditValidationException>(
                    addAddressExtractionAuditTask.AsTask);

            // then
            actualAddressExtractionAuditValidationException.Should()
                .BeEquivalentTo(expectedAddressExtractionAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressExtractionAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressExtractionAuditAsync(It.IsAny<AddressExtractionAudit>()),
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
            AddressExtractionAudit randomAddressExtractionAudit = CreateRandomAddressExtractionAudit(randomDateTimeOffset);
            AddressExtractionAudit invalidAddressExtractionAudit = randomAddressExtractionAudit;

            invalidAddressExtractionAudit.UpdatedDate =
                invalidAddressExtractionAudit.CreatedDate.AddDays(randomNumber);

            var invalidAddressExtractionAuditException = 
                new InvalidAddressExtractionAuditException(
                    message: "Invalid addressExtractionAudit. Please correct the errors and try again.");

            invalidAddressExtractionAuditException.AddData(
                key: nameof(AddressExtractionAudit.UpdatedDate),
                values: $"Date is not the same as {nameof(AddressExtractionAudit.CreatedDate)}");

            var expectedAddressExtractionAuditValidationException =
                new AddressExtractionAuditValidationException(
                    message: "AddressExtractionAudit validation errors occurred, please try again.",
                    innerException: invalidAddressExtractionAuditException);

            // when
            ValueTask<AddressExtractionAudit> addAddressExtractionAuditTask =
                this.addressExtractionAuditService.AddAddressExtractionAuditAsync(invalidAddressExtractionAudit);

            AddressExtractionAuditValidationException actualAddressExtractionAuditValidationException =
                await Assert.ThrowsAsync<AddressExtractionAuditValidationException>(
                    addAddressExtractionAuditTask.AsTask);

            // then
            actualAddressExtractionAuditValidationException.Should()
                .BeEquivalentTo(expectedAddressExtractionAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressExtractionAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressExtractionAuditAsync(It.IsAny<AddressExtractionAudit>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateUsersIsNotSameAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            AddressExtractionAudit randomAddressExtractionAudit = CreateRandomAddressExtractionAudit(randomDateTimeOffset);
            AddressExtractionAudit invalidAddressExtractionAudit = randomAddressExtractionAudit;
            invalidAddressExtractionAudit.UpdatedBy = Guid.NewGuid().ToString();

            var invalidAddressExtractionAuditException =
                new InvalidAddressExtractionAuditException(
                    message: "Invalid addressExtractionAudit. Please correct the errors and try again.");

            invalidAddressExtractionAuditException.AddData(
                key: nameof(AddressExtractionAudit.UpdatedBy),
                values: $"Text is not the same as {nameof(AddressExtractionAudit.CreatedBy)}");

            var expectedAddressExtractionAuditValidationException =
                new AddressExtractionAuditValidationException(
                    message: "AddressExtractionAudit validation errors occurred, please try again.",
                    innerException: invalidAddressExtractionAuditException);

            // when
            ValueTask<AddressExtractionAudit> addAddressExtractionAuditTask =
                this.addressExtractionAuditService.AddAddressExtractionAuditAsync(invalidAddressExtractionAudit);

            AddressExtractionAuditValidationException actualAddressExtractionAuditValidationException =
                await Assert.ThrowsAsync<AddressExtractionAuditValidationException>(
                    addAddressExtractionAuditTask.AsTask);

            // then
            actualAddressExtractionAuditValidationException.Should()
                .BeEquivalentTo(expectedAddressExtractionAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressExtractionAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressExtractionAuditAsync(It.IsAny<AddressExtractionAudit>()),
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

            AddressExtractionAudit randomAddressExtractionAudit = CreateRandomAddressExtractionAudit(invalidDateTime);
            AddressExtractionAudit invalidAddressExtractionAudit = randomAddressExtractionAudit;

            var invalidAddressExtractionAuditException =
                new InvalidAddressExtractionAuditException(
                    message: "Invalid addressExtractionAudit. Please correct the errors and try again.");

            invalidAddressExtractionAuditException.AddData(
                key: nameof(AddressExtractionAudit.CreatedDate),
                values: "Date is not recent");

            var expectedAddressExtractionAuditValidationException =
                new AddressExtractionAuditValidationException(
                    message: "AddressExtractionAudit validation errors occurred, please try again.",
                    innerException: invalidAddressExtractionAuditException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<AddressExtractionAudit> addAddressExtractionAuditTask =
                this.addressExtractionAuditService.AddAddressExtractionAuditAsync(invalidAddressExtractionAudit);

            AddressExtractionAuditValidationException actualAddressExtractionAuditValidationException =
                await Assert.ThrowsAsync<AddressExtractionAuditValidationException>(
                    addAddressExtractionAuditTask.AsTask);

            // then
            actualAddressExtractionAuditValidationException.Should()
                .BeEquivalentTo(expectedAddressExtractionAuditValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressExtractionAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressExtractionAuditAsync(It.IsAny<AddressExtractionAudit>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}