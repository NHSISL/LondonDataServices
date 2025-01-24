// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.TerminologyArtifacts
{
    public partial class TerminologyArtifactServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidTerminologyArtifactId = Guid.Empty;

            var invalidTerminologyArtifactException =
                new InvalidTerminologyArtifactException(
                    message: "Invalid terminologyArtifact. Please correct the errors and try again.");

            invalidTerminologyArtifactException.AddData(
                key: nameof(TerminologyArtifact.Id),
                values: "Id is required");

            var expectedTerminologyArtifactValidationException =
                new TerminologyArtifactValidationException(
                    message: "TerminologyArtifact validation errors occurred, please try again.",
                    innerException: invalidTerminologyArtifactException);

            // when
            ValueTask<TerminologyArtifact> retrieveTerminologyArtifactByIdTask =
                this.terminologyArtifactService.RetrieveTerminologyArtifactByIdAsync(invalidTerminologyArtifactId);

            TerminologyArtifactValidationException actualTerminologyArtifactValidationException =
                await Assert.ThrowsAsync<TerminologyArtifactValidationException>(
                    retrieveTerminologyArtifactByIdTask.AsTask);

            // then
            actualTerminologyArtifactValidationException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyArtifactByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfTerminologyArtifactIsNotFoundAndLogItAsync()
        {
            //given
            Guid someTerminologyArtifactId = Guid.NewGuid();
            TerminologyArtifact noTerminologyArtifact = null;

            var notFoundTerminologyArtifactException =
                new NotFoundTerminologyArtifactException(someTerminologyArtifactId);

            var expectedTerminologyArtifactValidationException =
                new TerminologyArtifactValidationException(
                    message: "TerminologyArtifact validation errors occurred, please try again.",
                    innerException: notFoundTerminologyArtifactException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyArtifactByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noTerminologyArtifact);

            //when
            ValueTask<TerminologyArtifact> retrieveTerminologyArtifactByIdTask =
                this.terminologyArtifactService.RetrieveTerminologyArtifactByIdAsync(someTerminologyArtifactId);

            TerminologyArtifactValidationException actualTerminologyArtifactValidationException =
                await Assert.ThrowsAsync<TerminologyArtifactValidationException>(
                    retrieveTerminologyArtifactByIdTask.AsTask);

            //then
            actualTerminologyArtifactValidationException.Should().BeEquivalentTo(expectedTerminologyArtifactValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyArtifactByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}