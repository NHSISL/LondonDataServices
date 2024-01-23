using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddresses
{
    public partial class ResolvedAddressServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfResolvedAddressIsNullAndLogItAsync()
        {
            // given
            ResolvedAddress nullResolvedAddress = null;
            var nullResolvedAddressException = new NullResolvedAddressException(message: "ResolvedAddress is null.");

            var expectedResolvedAddressValidationException =
                new ResolvedAddressValidationException(
                    message: "ResolvedAddress validation errors occurred, please try again.",
                    innerException: nullResolvedAddressException);

            // when
            ValueTask<ResolvedAddress> modifyResolvedAddressTask =
                this.resolvedAddressService.ModifyResolvedAddressAsync(nullResolvedAddress);

            ResolvedAddressValidationException actualResolvedAddressValidationException =
                await Assert.ThrowsAsync<ResolvedAddressValidationException>(
                    modifyResolvedAddressTask.AsTask);

            // then
            actualResolvedAddressValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedResolvedAddressValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateResolvedAddressAsync(It.IsAny<ResolvedAddress>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}