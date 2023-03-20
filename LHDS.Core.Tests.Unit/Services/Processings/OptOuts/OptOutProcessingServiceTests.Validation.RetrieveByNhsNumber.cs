// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Processings.Documents.Exceptions;
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
                new InvalidArgumentOptOutProcessingException();

            invalidArgumentOptOutProcessingException.AddData(
                key: nameof(OptOut.NhsNumber),
                values: "Text is required");

            var expectedOptOutProcessingValidationException =
                new OptOutProcessingValidationException(
                    innerException: invalidArgumentOptOutProcessingException,
                    validationSummary: GetValidationSummary(invalidArgumentOptOutProcessingException.Data));

            // when
            ValueTask<OptOut> RemoveOptOutTask =
                this.optOutProcessingService.RetrieveOptOutByNhsNumberAsync(invalidOptOutNhsNumber);

            OptOutProcessingValidationException actualOptOutProcessingValidationException =
                await Assert.ThrowsAsync<OptOutProcessingValidationException>(RemoveOptOutTask.AsTask);

            //then
            actualOptOutProcessingValidationException.Should()
                .BeEquivalentTo(expectedOptOutProcessingValidationException);

            this.optOutServiceMock.Verify(service =>
                service.RetrieveAllOptOuts(),
                    Times.Never());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOptOutProcessingValidationException))),
                        Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}