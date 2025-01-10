// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.TerminologyPolls
{
    public partial class TerminologyPollServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidTerminologyPollId = Guid.Empty;

            var invalidTerminologyPollException =
                new InvalidTerminologyPollException(
                    message: "Invalid terminologyPoll. Please correct the errors and try again.");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.Id),
                values: "Id is required");

            var expectedTerminologyPollValidationException =
                new TerminologyPollValidationException(
                    message: "TerminologyPoll validation errors occurred, please try again.",
                    innerException: invalidTerminologyPollException);

            // when
            ValueTask<TerminologyPoll> retrieveTerminologyPollByIdTask =
                this.terminologyPollService.RetrieveTerminologyPollByIdAsync(invalidTerminologyPollId);

            TerminologyPollValidationException actualTerminologyPollValidationException =
                await Assert.ThrowsAsync<TerminologyPollValidationException>(
                    retrieveTerminologyPollByIdTask.AsTask);

            // then
            actualTerminologyPollValidationException.Should()
                .BeEquivalentTo(expectedTerminologyPollValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedTerminologyPollValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyPollByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfTerminologyPollIsNotFoundAndLogItAsync()
        {
            //given
            Guid someTerminologyPollId = Guid.NewGuid();
            TerminologyPoll noTerminologyPoll = null;

            var notFoundTerminologyPollException =
                new NotFoundTerminologyPollException(someTerminologyPollId);

            var expectedTerminologyPollValidationException =
                new TerminologyPollValidationException(
                    message: "TerminologyPoll validation errors occurred, please try again.",
                    innerException: notFoundTerminologyPollException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyPollByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noTerminologyPoll);

            //when
            ValueTask<TerminologyPoll> retrieveTerminologyPollByIdTask =
                this.terminologyPollService.RetrieveTerminologyPollByIdAsync(someTerminologyPollId);

            TerminologyPollValidationException actualTerminologyPollValidationException =
                await Assert.ThrowsAsync<TerminologyPollValidationException>(
                    retrieveTerminologyPollByIdTask.AsTask);

            //then
            actualTerminologyPollValidationException.Should().BeEquivalentTo(expectedTerminologyPollValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyPollByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyPollValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}