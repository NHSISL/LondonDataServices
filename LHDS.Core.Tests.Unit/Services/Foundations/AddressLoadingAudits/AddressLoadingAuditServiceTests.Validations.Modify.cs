using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using LHDS.Core.Models.Foundations.AddressLoadingAudits.Exceptions;
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
                values: "Date is required");

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
            actualAddressLoadingAuditValidationException.Should().BeEquivalentTo(expectedAddressLoadingAuditValidationException);

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
    }
}