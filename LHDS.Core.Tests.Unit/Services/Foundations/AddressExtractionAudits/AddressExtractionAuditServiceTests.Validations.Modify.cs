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
        public async Task ShouldThrowValidationExceptionOnModifyIfAddressExtractionAuditIsNullAndLogItAsync()
        {
            // given
            AddressExtractionAudit nullAddressExtractionAudit = null;
            var nullAddressExtractionAuditException = new NullAddressExtractionAuditException(message: "AddressExtractionAudit is null.");

            var expectedAddressExtractionAuditValidationException =
                new AddressExtractionAuditValidationException(
                    message: "AddressExtractionAudit validation errors occurred, please try again.",
                    innerException: nullAddressExtractionAuditException);

            // when
            ValueTask<AddressExtractionAudit> modifyAddressExtractionAuditTask =
                this.addressExtractionAuditService.ModifyAddressExtractionAuditAsync(nullAddressExtractionAudit);

            AddressExtractionAuditValidationException actualAddressExtractionAuditValidationException =
                await Assert.ThrowsAsync<AddressExtractionAuditValidationException>(
                    modifyAddressExtractionAuditTask.AsTask);

            // then
            actualAddressExtractionAuditValidationException.Should()
                .BeEquivalentTo(expectedAddressExtractionAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressExtractionAuditValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressExtractionAuditAsync(It.IsAny<AddressExtractionAudit>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfAddressExtractionAuditIsInvalidAndLogItAsync(string invalidText)
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
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(AddressExtractionAudit.CreatedDate)}"
                });

            invalidAddressExtractionAuditException.AddData(
                key: nameof(AddressExtractionAudit.UpdatedBy),
                values: "Text is required");

            var expectedAddressExtractionAuditValidationException =
                new AddressExtractionAuditValidationException(
                    message: "AddressExtractionAudit validation errors occurred, please try again.",
                    innerException: invalidAddressExtractionAuditException);

            // when
            ValueTask<AddressExtractionAudit> modifyAddressExtractionAuditTask =
                this.addressExtractionAuditService.ModifyAddressExtractionAuditAsync(invalidAddressExtractionAudit);

            AddressExtractionAuditValidationException actualAddressExtractionAuditValidationException =
                await Assert.ThrowsAsync<AddressExtractionAuditValidationException>(
                    modifyAddressExtractionAuditTask.AsTask);

            //then
            actualAddressExtractionAuditValidationException.Should().BeEquivalentTo(expectedAddressExtractionAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressExtractionAuditValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressExtractionAuditAsync(It.IsAny<AddressExtractionAudit>()),
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
            AddressExtractionAudit randomAddressExtractionAudit = CreateRandomAddressExtractionAudit(randomDateTimeOffset);
            AddressExtractionAudit invalidAddressExtractionAudit = randomAddressExtractionAudit;
            
            var invalidAddressExtractionAuditException = 
                new InvalidAddressExtractionAuditException(
                    message: "Invalid addressExtractionAudit. Please correct the errors and try again.");

            invalidAddressExtractionAuditException.AddData(
                key: nameof(AddressExtractionAudit.UpdatedDate),
                values: $"Date is the same as {nameof(AddressExtractionAudit.CreatedDate)}");

            var expectedAddressExtractionAuditValidationException =
                new AddressExtractionAuditValidationException(
                    message: "AddressExtractionAudit validation errors occurred, please try again.",
                    innerException: invalidAddressExtractionAuditException);

            // when
            ValueTask<AddressExtractionAudit> modifyAddressExtractionAuditTask =
                this.addressExtractionAuditService.ModifyAddressExtractionAuditAsync(invalidAddressExtractionAudit);

            AddressExtractionAuditValidationException actualAddressExtractionAuditValidationException =
                await Assert.ThrowsAsync<AddressExtractionAuditValidationException>(
                    modifyAddressExtractionAuditTask.AsTask);

            // then
            actualAddressExtractionAuditValidationException.Should().BeEquivalentTo(expectedAddressExtractionAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressExtractionAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressExtractionAuditByIdAsync(invalidAddressExtractionAudit.Id),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}