// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AssignAddresses;
using LHDS.Core.Models.Foundations.Assigns.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Assigns
{
    public partial class AssignServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfAssignAddressIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            string invalidAssignAddress = invalidText;

            var invalidAssignAddressException =
                new InvalidArgumentAssignException(
                    message: "Invalid address. Please correct the errors and try again.");

            invalidAssignAddressException.AddData(
                key: "address",
                values: "Text is required");

            var expectedAssignValidationException =
                new AssignValidationException(
                    message: "Assign validation errors occurred, please try again.",
                    innerException: invalidAssignAddressException);

            // when
            ValueTask<AssignAddress> addAssignAddressTask =
                this.assignService.MatchAddressAsync(invalidAssignAddress);

            AssignValidationException actualAssignValidationException =
                await Assert.ThrowsAsync<AssignValidationException>(addAssignAddressTask.AsTask);

            // then
            actualAssignValidationException.Should()
                .BeEquivalentTo(expectedAssignValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAssignValidationException))),
                        Times.Once);

            this.assignBrokerMock.Verify(broker =>
                broker.MatchAddressAsync(It.IsAny<string>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.assignBrokerMock.VerifyNoOtherCalls();
        }
    }
}
