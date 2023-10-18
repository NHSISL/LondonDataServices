// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using LHDS.Core.Models.Foundations.AddressLoadingAudits.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.AddressLoadingAudits
{
    public partial class AddressLoadingAuditProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfAddressLoadingAuditIsNullAndLogItAsync()
        {
            // given
            AddressLoadingAudit nullAddressLoadingAudit = null;

            var nullAddressLoadingAuditException =
                new NullAddressLoadingAuditException(message: "Address loading audit is null.");

            var expectedAddressLoadingAuditValidationException =
                new AddressLoadingAuditValidationException(
                    message: "Address loading audit validation errors occurred, please try again.",
                    innerException: nullAddressLoadingAuditException);

            // when
            ValueTask<AddressLoadingAudit> addAddressLoadingAuditProcessingTask =
                this.addressLoadingAuditProcessingService.AddAddressLoadingAuditAsync(nullAddressLoadingAudit);

            AddressLoadingAuditValidationException actualAddressLoadingAuditValidationException =
                await Assert.ThrowsAsync<AddressLoadingAuditValidationException>(() =>
                    addAddressLoadingAuditProcessingTask.AsTask());

            // then
            actualAddressLoadingAuditValidationException.Should()
                .BeEquivalentTo(expectedAddressLoadingAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressLoadingAuditValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfAddressLoadingAuditIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            var invalidAddressLoadingAudit = new AddressLoadingAudit 
            {
                CreatedBy = invalidText,
                UpdatedBy = invalidText
            };

            var invalidAddressLoadingAuditException =
                new InvalidAddressLoadingAuditException(
                    message: "Invalid address loading audit. Please correct the errors and try again.");

            invalidAddressLoadingAuditException.AddData(
                key: nameof(AddressLoadingAudit.Id),
                values: "Id is required");

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
                    message: "Address loading audit validation errors occurred, please try again.",
                    innerException: invalidAddressLoadingAuditException);

            // when
            ValueTask<AddressLoadingAudit> addAddressLoadingAuditTask =
                this.addressLoadingAuditProcessingService.AddAddressLoadingAuditAsync(invalidAddressLoadingAudit);

            AddressLoadingAuditValidationException actualAddressLoadingAuditValidationException =
                await Assert.ThrowsAsync<AddressLoadingAuditValidationException>(() =>
                    addAddressLoadingAuditTask.AsTask());

            // then
            actualAddressLoadingAuditValidationException.Should()
                .BeEquivalentTo(expectedAddressLoadingAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressLoadingAuditValidationException))),
                        Times.Once);

            this.addressLoadingAuditServiceMock.Verify(service =>
                service.AddAddressLoadingAuditAsync(It.IsAny<AddressLoadingAudit>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressLoadingAuditServiceMock.VerifyNoOtherCalls();
        }
    }
}