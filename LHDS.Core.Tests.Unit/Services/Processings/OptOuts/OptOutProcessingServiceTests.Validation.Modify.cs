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
        [Fact]
        public async Task ShouldThrowValidationExceptionsOnModifyIfOptOutProcessingIsNullAndLogItAsync()
        {
            // given
            OptOut nullOptOut = null;

            var nullOptOutProcessingException =
                new NullOptOutProcessingException();

            var expectedOptOutProcessingValidationException =
                new OptOutProcessingValidationException(innerException: nullOptOutProcessingException);

            // when
            ValueTask<OptOut> RetrieveOrAddOptOutTask =
                this.optOutProcessingService.AddOrModifyOptOutAsync(nullOptOut);

            OptOutProcessingValidationException actualOptOutProcessingValidationException =
                await Assert.ThrowsAsync<OptOutProcessingValidationException>(RetrieveOrAddOptOutTask.AsTask);

            //then
            actualOptOutProcessingValidationException.Should()
                .BeEquivalentTo(expectedOptOutProcessingValidationException);

            this.optOutServiceMock.Verify(service =>
                service.ModifyOptOutAsync(nullOptOut),
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