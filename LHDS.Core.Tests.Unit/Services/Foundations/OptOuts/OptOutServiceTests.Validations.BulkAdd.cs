// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Foundations.OptOuts.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OptOuts
{
    public partial class OptOutServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task
            ShouldThrowValidationExceptionOnBulkAddIfOptOutIsInvalidAndLogItAsync(
                string invalidText)
        {
            // given
            List<OptOut> invalidOptOuts = null;
            string invalidFileName = invalidText;

            var invalidOptOutException =
                new InvalidOptOutException(
                    message: "Invalid optOut. Please correct the errors"
                        + " and try again.");

            invalidOptOutException.AddData(
                key: "optOuts",
                values: "OptOuts is required");

            invalidOptOutException.AddData(
                key: "fileName",
                values: "Text is required");

            var expectedOptOutValidationException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred,"
                        + " please try again.",
                    innerException: invalidOptOutException);

            // when
            ValueTask bulkAddOptOutTask =
                this.optOutService.BulkAddOptOutsAsync(
                    optOuts: invalidOptOuts,
                    fileName: invalidFileName);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(
                    bulkAddOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOptOutAsync(It.IsAny<OptOut>(), default),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
