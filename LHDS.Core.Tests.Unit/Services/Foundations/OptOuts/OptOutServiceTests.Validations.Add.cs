using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.OptOuts;
using LHDS.Core.Models.OptOuts.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OptOuts
{
    public partial class OptOutServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfOptOutIsNullAndLogItAsync()
        {
            // given
            OptOut nullOptOut = null;

            var nullOptOutException =
                new NullOptOutException();

            var expectedOptOutValidationException =
                new OptOutValidationException(nullOptOutException);

            // when
            ValueTask<OptOut> addOptOutTask =
                this.optOutService.AddOptOutAsync(nullOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(
                    addOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should().BeEquivalentTo(expectedOptOutValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}