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
        [Fact]
        public async Task ShouldThrowValidationExceptionsOnRetrieveIfOptOutProcessingIdIsNullAndLogItAsync()
        {
            // given
            Guid invalidIngestionTrackingId = Guid.Empty;

            var invalidOptOutProcessingIdException =
                new InvalidOptOutProcessingIdException();

            invalidOptOutProcessingIdException.AddData(
                key: nameof(OptOut.Id),
                values: "Id is required");

            var expectedOptOutProcessingValidationException =
                new OptOutProcessingValidationException(
                    innerException: invalidOptOutProcessingIdException,
                    validationSummary: GetValidationSummary(invalidOptOutProcessingIdException.Data));

            // when
            ValueTask<OptOut> RemoveOptOutTask =
                this.optOutProcessingService.RetrieveOptOutByIdAsync(invalidIngestionTrackingId);

            OptOutProcessingValidationException actualOptOutProcessingValidationException =
                await Assert.ThrowsAsync<OptOutProcessingValidationException>(RemoveOptOutTask.AsTask);

            //then
            actualOptOutProcessingValidationException.Should()
                .BeEquivalentTo(expectedOptOutProcessingValidationException);

            this.optOutServiceMock.Verify(service =>
                service.RetrieveOptOutByIdAsync(invalidIngestionTrackingId),
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