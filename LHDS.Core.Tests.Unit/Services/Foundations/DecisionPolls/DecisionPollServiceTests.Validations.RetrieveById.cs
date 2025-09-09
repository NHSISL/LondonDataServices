// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DecisionPolls;
using LHDS.Core.Models.Foundations.DecisionPolls.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DecisionPolls
{
    public partial class DecisionPollServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidDecisionPollId = Guid.Empty;

            var invalidDecisionPollException =
                new InvalidDecisionPollException(
                    message: "Invalid decisionPoll. Please correct the errors and try again.");

            invalidDecisionPollException.AddData(
                key: nameof(DecisionPoll.Id),
                values: "Id is required");

            var expectedDecisionPollValidationException =
                new DecisionPollValidationException(
                    message: "DecisionPoll validation errors occurred, please try again.",
                    innerException: invalidDecisionPollException);

            // when
            ValueTask<DecisionPoll> retrieveDecisionPollByIdTask =
                this.decisionPollService.RetrieveDecisionPollByIdAsync(invalidDecisionPollId);

            DecisionPollValidationException actualDecisionPollValidationException =
                await Assert.ThrowsAsync<DecisionPollValidationException>(
                    retrieveDecisionPollByIdTask.AsTask);

            // then
            actualDecisionPollValidationException.Should()
                .BeEquivalentTo(expectedDecisionPollValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionPollValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDecisionPollByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfDecisionPollIsNotFoundAndLogItAsync()
        {
            //given
            Guid someDecisionPollId = Guid.NewGuid();
            DecisionPoll noDecisionPoll = null;

            var notFoundDecisionPollException =
                new NotFoundDecisionPollException(someDecisionPollId);

            var expectedDecisionPollValidationException =
                new DecisionPollValidationException(
                    message: "DecisionPoll validation errors occurred, please try again.",
                    innerException: notFoundDecisionPollException);

            this.storageBrokerMock.Setup(broker =>
                    broker.SelectDecisionPollByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(noDecisionPoll);

            //when
            ValueTask<DecisionPoll> retrieveDecisionPollByIdTask =
                this.decisionPollService.RetrieveDecisionPollByIdAsync(someDecisionPollId);

            DecisionPollValidationException actualDecisionPollValidationException =
                await Assert.ThrowsAsync<DecisionPollValidationException>(
                    retrieveDecisionPollByIdTask.AsTask);

            //then
            actualDecisionPollValidationException.Should().BeEquivalentTo(expectedDecisionPollValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDecisionPollByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionPollValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
