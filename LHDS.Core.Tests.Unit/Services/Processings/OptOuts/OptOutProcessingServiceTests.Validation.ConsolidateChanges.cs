// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
        public async Task ShouldThrowValidationExceptionsOnConsolidateChangesIfOptOutProcessingListIsNullAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            List<OptOut> nullOptOutList = null;
            List<string> randomStringList = RandomStringList(randomNumber);

            var nullOptOutListProcessingException =
                new NullOptOutListProcessingException();

            var expectedOptOutProcessingValidationException =
                new OptOutProcessingValidationException(innerException: nullOptOutListProcessingException);

            // when
            ValueTask<List<OptOut>> ConsolidateChangesOptOutTask =
                this.optOutProcessingService.ConsolidateOptOutChangesAndReturnChangesOnly(nullOptOutList, randomStringList);

            OptOutProcessingValidationException actualOptOutProcessingValidationException =
                await Assert.ThrowsAsync<OptOutProcessingValidationException>(ConsolidateChangesOptOutTask.AsTask);

            //then
            actualOptOutProcessingValidationException.Should()
                .BeEquivalentTo(expectedOptOutProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOptOutProcessingValidationException))),
                        Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionsOnConsolidateChangesIfOptOutProcessingItemIsNullAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            List<OptOut> optOutListWithNullOptOuts = CreateRandomOptOutListWithNullOptOut();
            List<string> randomStringList = RandomStringList(randomNumber);

            var nullOptOutProcessingException =
                new NullOptOutProcessingException();

            var expectedOptOutProcessingValidationException =
                new OptOutProcessingValidationException(innerException: nullOptOutProcessingException);

            // when
            ValueTask<List<OptOut>> ConsolidateChangesOptOutTask =
                this.optOutProcessingService.ConsolidateOptOutChangesAndReturnChangesOnly(
                    optOutListWithNullOptOuts, 
                    randomStringList);

            OptOutProcessingValidationException actualOptOutProcessingValidationException =
                await Assert.ThrowsAsync<OptOutProcessingValidationException>(ConsolidateChangesOptOutTask.AsTask);

            //then
            actualOptOutProcessingValidationException.Should()
                .BeEquivalentTo(expectedOptOutProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOptOutProcessingValidationException))),
                        Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}