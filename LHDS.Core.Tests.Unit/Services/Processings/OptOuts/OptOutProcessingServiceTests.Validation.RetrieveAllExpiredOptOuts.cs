// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
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
        [Fact]
        public async Task ShouldThrowValidationExceptionsOnRetrieveAllExpiredOptOutsIfOlderThanDaysIsNullAndLogItAsync()
        {
            // given
            int invalidInput = 6;
            int inputOlderThanDays = invalidInput;

            var invalidArgumentOptOutProcessingException =
                new InvalidArgumentOptOutProcessingException();

            invalidArgumentOptOutProcessingException.AddData(
                key: "OlderThanDays",
                values: "Value is required");

            var expectedOptOutProcessingValidationException =
                new OptOutProcessingValidationException(
                    innerException: invalidArgumentOptOutProcessingException);

            // when
            ValueTask<List<OptOut>> RetrieveExiredOptOutTask =
                this.optOutProcessingService.RetrieveAllExpiredOptOutsAsync(olderThanDays: inputOlderThanDays);

            OptOutProcessingValidationException actualOptOutProcessingValidationException =
                await Assert.ThrowsAsync<OptOutProcessingValidationException>(RetrieveExiredOptOutTask.AsTask);

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