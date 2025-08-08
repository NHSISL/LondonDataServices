// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.SubscriberPractices;
using LHDS.Core.Models.Foundations.SubscriberPractices.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SubscriberPractices
{
    public partial class SubscriberPracticeServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidSubscriberPracticeId = Guid.Empty;

            var invalidSubscriberPracticeException =
                new InvalidSubscriberPracticeException(
                    message: "Invalid subscriberPractice. Please correct the errors and try again.");

            invalidSubscriberPracticeException.AddData(
                key: nameof(SubscriberPractice.Id),
                values: "Id is required");

            var expectedSubscriberPracticeValidationException =
                new SubscriberPracticeValidationException(
                    message: "SubscriberPractice validation errors occurred, please try again.",
                    innerException: invalidSubscriberPracticeException);

            // when
            ValueTask<SubscriberPractice> removeSubscriberPracticeByIdTask =
                this.subscriberPracticeService.RemoveSubscriberPracticeByIdAsync(invalidSubscriberPracticeId);

            SubscriberPracticeValidationException actualSubscriberPracticeValidationException =
                await Assert.ThrowsAsync<SubscriberPracticeValidationException>(
                    removeSubscriberPracticeByIdTask.AsTask);

            // then
            actualSubscriberPracticeValidationException.Should()
                .BeEquivalentTo(expectedSubscriberPracticeValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberPracticeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteSubscriberPracticeAsync(It.IsAny<SubscriberPractice>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}