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
        public async Task ShouldThrowValidationExceptionsOnConsolidateChangesIfOptOutProcessingListIsNullAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            List<OptOut> nullOptOutList = null;
            List<string> randomStringList = RandomStringList(randomNumber);

            var invalidArgumentOptOutProcessingException =
                new InvalidArgumentOptOutProcessingException();

            invalidArgumentOptOutProcessingException.AddData(
                key: "OptOutList",
                values: "Opt out list is required");

            var expectedOptOutProcessingValidationException =
                new OptOutProcessingValidationException(innerException: invalidArgumentOptOutProcessingException);

            // when
            ValueTask<List<OptOut>> ConsolidateChangesOptOutTask =
                this.optOutProcessingService.ConsolidateOptOutChangesAndReturnChangesOnly(nullOptOutList, randomStringList);

            OptOutProcessingValidationException actualOptOutProcessingValidationException =
                await Assert.ThrowsAsync<OptOutProcessingValidationException>(ConsolidateChangesOptOutTask.AsTask);

            //then
            actualOptOutProcessingValidationException.Should()
                .BeEquivalentTo(expectedOptOutProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutProcessingValidationException))),
                        Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowValidationExceptionsOnConsolidateChangesIfConsentedItemsProcessingListIsNullAndLogItAsync()
        {
            // given
            string randomString = GetRandomString();
            List<OptOut> randomOptOutsList = CreateRandomOptOutList(randomString);
            List<string> nullStringList = null;

            var invalidArgumentOptOutProcessingException =
                new InvalidArgumentOptOutProcessingException();

            invalidArgumentOptOutProcessingException.AddData(
                key: "ConsentedItemsList",
                values: "String list is required");

            var expectedOptOutProcessingValidationException =
                new OptOutProcessingValidationException(
                    innerException: invalidArgumentOptOutProcessingException);

            // when
            ValueTask<List<OptOut>> ConsolidateChangesOptOutTask =
                this.optOutProcessingService.ConsolidateOptOutChangesAndReturnChangesOnly(
                    randomOptOutsList,
                    nullStringList);

            OptOutProcessingValidationException actualOptOutProcessingValidationException =
                await Assert.ThrowsAsync<OptOutProcessingValidationException>(ConsolidateChangesOptOutTask.AsTask);

            //then
            actualOptOutProcessingValidationException.Should()
                .BeEquivalentTo(expectedOptOutProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutProcessingValidationException))),
                        Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}