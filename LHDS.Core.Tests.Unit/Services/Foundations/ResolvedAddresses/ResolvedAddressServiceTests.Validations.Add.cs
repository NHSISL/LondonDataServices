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
        public async Task ShouldThrowValidationExceptionOnAddIfResolvedAddressIsNullAndLogItAsync()
        {
            // given
            ResolvedAddress nullResolvedAddress = null;

            var nullResolvedAddressException =
                new NullResolvedAddressException(message: "ResolvedAddress is null.");

            var expectedResolvedAddressValidationException =
                new ResolvedAddressValidationException(
                    message: "ResolvedAddress validation errors occurred, please try again.",
                    innerException: nullResolvedAddressException);

            // when
            ValueTask<ResolvedAddress> addResolvedAddressTask =
                this.resolvedAddressService.AddResolvedAddressAsync(nullResolvedAddress);

            ResolvedAddressValidationException actualResolvedAddressValidationException =
                await Assert.ThrowsAsync<ResolvedAddressValidationException>(
                    addResolvedAddressTask.AsTask);

            // then
            actualResolvedAddressValidationException.Should().BeEquivalentTo(expectedResolvedAddressValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedResolvedAddressValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}