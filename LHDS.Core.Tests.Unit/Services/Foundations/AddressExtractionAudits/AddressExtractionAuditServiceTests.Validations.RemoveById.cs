// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressExtractionAudits;
using LHDS.Core.Models.Foundations.AddressExtractionAudits.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressExtractionAudits
{
    public partial class AddressExtractionAuditServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidAddressExtractionAuditId = Guid.Empty;

            var invalidAddressExtractionAuditException =
                new InvalidAddressExtractionAuditException(
                    message: "Invalid addressExtractionAudit. Please correct the errors and try again.");

            invalidAddressExtractionAuditException.AddData(
                key: nameof(AddressExtractionAudit.Id),
                values: "Id is required");

            var expectedAddressExtractionAuditValidationException =
                new AddressExtractionAuditValidationException(
                    message: "AddressExtractionAudit validation errors occurred, please try again.",
                    innerException: invalidAddressExtractionAuditException);

            // when
            ValueTask<AddressExtractionAudit> removeAddressExtractionAuditByIdTask =
                this.addressExtractionAuditService.RemoveAddressExtractionAuditByIdAsync(invalidAddressExtractionAuditId);

            AddressExtractionAuditValidationException actualAddressExtractionAuditValidationException =
                await Assert.ThrowsAsync<AddressExtractionAuditValidationException>(
                    removeAddressExtractionAuditByIdTask.AsTask);

            // then
            actualAddressExtractionAuditValidationException.Should()
                .BeEquivalentTo(expectedAddressExtractionAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressExtractionAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAddressExtractionAuditAsync(It.IsAny<AddressExtractionAudit>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}