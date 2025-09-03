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
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidSubscriberPracticeId = Guid.Empty;

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
            ValueTask<SubscriberPractice> retrieveSubscriberPracticeByIdTask =
                this.subscriberPracticeService.RetrieveSubscriberPracticeByIdAsync(invalidSubscriberPracticeId);

            SubscriberPracticeValidationException actualSubscriberPracticeValidationException =
                await Assert.ThrowsAsync<SubscriberPracticeValidationException>(
                    retrieveSubscriberPracticeByIdTask.AsTask);

            // then
            actualSubscriberPracticeValidationException.Should()
                .BeEquivalentTo(expectedSubscriberPracticeValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberPracticeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberPracticeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfSubscriberPracticeIsNotFoundAndLogItAsync()
        {
            //given
            Guid someSubscriberPracticeId = Guid.NewGuid();
            SubscriberPractice noSubscriberPractice = null;

            var notFoundSubscriberPracticeException =
                new NotFoundSubscriberPracticeException(
                    message: $"Couldn't find subscriberPractice with " +
                    $"subscriberPracticeId: {someSubscriberPracticeId}.");

            var expectedSubscriberPracticeValidationException =
                new SubscriberPracticeValidationException(
                    message: "SubscriberPractice validation errors occurred, please try again.",
                    innerException: notFoundSubscriberPracticeException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSubscriberPracticeByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noSubscriberPractice);

            //when
            ValueTask<SubscriberPractice> retrieveSubscriberPracticeByIdTask =
                this.subscriberPracticeService.RetrieveSubscriberPracticeByIdAsync(someSubscriberPracticeId);

            SubscriberPracticeValidationException actualSubscriberPracticeValidationException =
                await Assert.ThrowsAsync<SubscriberPracticeValidationException>(
                    retrieveSubscriberPracticeByIdTask.AsTask);

            //then
            actualSubscriberPracticeValidationException.Should().BeEquivalentTo(expectedSubscriberPracticeValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberPracticeByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberPracticeValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}