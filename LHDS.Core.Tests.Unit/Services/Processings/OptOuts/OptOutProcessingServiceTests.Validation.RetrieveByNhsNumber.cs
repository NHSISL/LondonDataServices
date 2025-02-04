// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Processings.OptOuts.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.OptOuts
{
    public partial class OptOutProcessingServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionsOnRetrieveIfOptOutProcessingNhsNumberIsNullAndLogItAsync(string invalidInput)
        {
            // given
            string invalidOptOutNhsNumber = invalidInput;

            var invalidArgumentOptOutProcessingException =
                new InvalidArgumentOptOutProcessingException(
                    message: "Invalid opt out processing argument. Please correct the errors and try again.");

            invalidArgumentOptOutProcessingException.AddData(
                key: nameof(OptOut.NhsNumber),
                values: "Text is required");

            var expectedOptOutProcessingValidationException =
                new OptOutProcessingValidationException(
                    innerException: invalidArgumentOptOutProcessingException);

            // when
            ValueTask<OptOut> RemoveOptOutTask =
                this.optOutProcessingService.RetrieveOptOutByNhsNumberAsync(invalidOptOutNhsNumber);

            OptOutProcessingValidationException actualOptOutProcessingValidationException =
                await Assert.ThrowsAsync<OptOutProcessingValidationException>(RemoveOptOutTask.AsTask);

            //then
            actualOptOutProcessingValidationException.Should()
                .BeEquivalentTo(expectedOptOutProcessingValidationException);

            this.optOutServiceMock.Verify(service =>
                service.RetrieveAllOptOutsAsync(),
                    Times.Never());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutProcessingValidationException))),
                        Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}