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
    }
}