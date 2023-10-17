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
        public async Task ShouldThrowValidationExceptionOnAddIfAddressLoadingAuditIsNullAndLogItAsync()
        {
            // given
            AddressLoadingAudit nullAddressLoadingAudit = null;

            var nullAddressLoadingAuditException =
                new NullAddressLoadingAuditException(message: "AddressLoadingAudit is null.");

            var expectedAddressLoadingAuditValidationException =
                new AddressLoadingAuditValidationException(
                    message: "AddressLoadingAudit validation errors occurred, please try again.",
                    innerException: nullAddressLoadingAuditException);

            // when
            ValueTask<AddressLoadingAudit> addAddressLoadingAuditTask =
                this.addressLoadingAuditService.AddAddressLoadingAuditAsync(nullAddressLoadingAudit);

            AddressLoadingAuditValidationException actualAddressLoadingAuditValidationException =
                await Assert.ThrowsAsync<AddressLoadingAuditValidationException>(
                    addAddressLoadingAuditTask.AsTask);

            // then
            actualAddressLoadingAuditValidationException.Should().BeEquivalentTo(expectedAddressLoadingAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressLoadingAuditValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}