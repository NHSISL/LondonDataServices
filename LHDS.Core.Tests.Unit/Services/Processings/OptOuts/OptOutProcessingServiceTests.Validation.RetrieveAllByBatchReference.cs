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
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionsOnRetrieveIfOptOutProcessingBatchReferenceIsNullAndLogItAsync(string invalidInput)
        {
            // given
            string invalidBatchReference = invalidInput;

            var invalidArgumentOptOutProcessingException =
                new InvalidArgumentOptOutProcessingException();

            invalidArgumentOptOutProcessingException.AddData(
                key: nameof(OptOut.BatchReference),
                values: "Text is required");

            var expectedOptOutProcessingValidationException =
                new OptOutProcessingValidationException(
                    innerException: invalidArgumentOptOutProcessingException);

            // when
            ValueTask<List<OptOut>> RetrieveOptOutTask =
                this.optOutProcessingService.RetrieveAllOptOutsByBatchReferenceAsync(batchReference: invalidBatchReference);

            OptOutProcessingValidationException actualOptOutProcessingValidationException =
                await Assert.ThrowsAsync<OptOutProcessingValidationException>(RetrieveOptOutTask.AsTask);

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