// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AssignAddresses;
using LHDS.Core.Models.Processings.Assigns.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Assigns
{
    public partial class AssignProcessingServiceTests
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
                new InvalidArgumentAssignProcessingException(
                    message: "Invalid address. Please correct the errors and try again.");

            invalidAssignAddressException.AddData(
                key: "address",
                values: "Text is required");

            var expectedAssignProcessingValidationException =
                new AssignProcessingValidationException(
                    message: "Assign processing validation errors occurred, please try again.",
                    innerException: invalidAssignAddressException);

            // when
            ValueTask<AssignAddress> addAssignAddressTask =
                this.assignProcessingService.MatchAddressAsync(invalidAssignAddress);

            AssignProcessingValidationException actualAssignProcessingValidationException =
                await Assert.ThrowsAsync<AssignProcessingValidationException>(addAssignAddressTask.AsTask);

            // then
            actualAssignProcessingValidationException.Should()
                .BeEquivalentTo(expectedAssignProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAssignProcessingValidationException))),
                        Times.Once);

            this.assignServiceMock.Verify(broker =>
                broker.MatchAddressAsync(It.IsAny<string>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.assignServiceMock.VerifyNoOtherCalls();
        }
    }
}
