// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.OntologyCodeSystems;
using LHDS.Core.Models.Foundations.OntologyCodeSystems.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OntologyCodeSystems
{
    public partial class OntologyCodeSystemServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfOntologyCodeSystemIsNullAndLogItAsync()
        {
            // given
            OntologyCodeSystem nullOntologyCodeSystem = null;

            var nullOntologyCodeSystemException =
                new NullOntologyCodeSystemException(message: "OntologyCodeSystem is null.");

            var expectedOntologyCodeSystemValidationException =
                new OntologyCodeSystemValidationException(
                    message: "OntologyCodeSystem validation errors occurred, please try again.",
                    innerException: nullOntologyCodeSystemException);

            // when
            ValueTask<OntologyCodeSystem> addOntologyCodeSystemTask =
                this.ontologyCodeSystemService.AddOntologyCodeSystemAsync(nullOntologyCodeSystem);

            OntologyCodeSystemValidationException actualOntologyCodeSystemValidationException =
                await Assert.ThrowsAsync<OntologyCodeSystemValidationException>(() =>
                    addOntologyCodeSystemTask.AsTask());

            // then
            actualOntologyCodeSystemValidationException.Should()
                .BeEquivalentTo(expectedOntologyCodeSystemValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyCodeSystemValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfOntologyCodeSystemIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            var invalidOntologyCodeSystem = new OntologyCodeSystem
            {
                FullUrl = invalidText,
                ResourceType = invalidText,
            };

            var invalidOntologyCodeSystemException =
                new InvalidOntologyCodeSystemException(
                    message: "Invalid ontologyCodeSystem. Please correct the errors and try again.");

            invalidOntologyCodeSystemException.AddData(
                key: nameof(OntologyCodeSystem.Id),
                values: "Id is required");

            invalidOntologyCodeSystemException.AddData(
                key: nameof(OntologyCodeSystem.FullUrl),
                values: "Text is required");

            invalidOntologyCodeSystemException.AddData(
                key: nameof(OntologyCodeSystem.ResourceType),
                values: "Text is required");

            invalidOntologyCodeSystemException.AddData(
                key: nameof(OntologyCodeSystem.CreatedDate),
                values: "Date is required");

            invalidOntologyCodeSystemException.AddData(
                key: nameof(OntologyCodeSystem.CreatedBy),
                values: "Text is required");

            invalidOntologyCodeSystemException.AddData(
                key: nameof(OntologyCodeSystem.UpdatedDate),
                values: "Date is required");

            invalidOntologyCodeSystemException.AddData(
                key: nameof(OntologyCodeSystem.UpdatedBy),
                values: "Text is required");

            var expectedOntologyCodeSystemValidationException =
                new OntologyCodeSystemValidationException(
                    message: "OntologyCodeSystem validation errors occurred, please try again.",
                    innerException: invalidOntologyCodeSystemException);

            // when
            ValueTask<OntologyCodeSystem> addOntologyCodeSystemTask =
                this.ontologyCodeSystemService.AddOntologyCodeSystemAsync(invalidOntologyCodeSystem);

            OntologyCodeSystemValidationException actualOntologyCodeSystemValidationException =
                await Assert.ThrowsAsync<OntologyCodeSystemValidationException>(() =>
                    addOntologyCodeSystemTask.AsTask());

            // then
            actualOntologyCodeSystemValidationException.Should()
                .BeEquivalentTo(expectedOntologyCodeSystemValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyCodeSystemValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOntologyCodeSystemAsync(It.IsAny<OntologyCodeSystem>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            OntologyCodeSystem randomOntologyCodeSystem = CreateRandomOntologyCodeSystem(randomDateTimeOffset);
            OntologyCodeSystem invalidOntologyCodeSystem = randomOntologyCodeSystem;

            invalidOntologyCodeSystem.UpdatedDate =
                invalidOntologyCodeSystem.CreatedDate.AddDays(randomNumber);

            var invalidOntologyCodeSystemException =
                new InvalidOntologyCodeSystemException(
                    message: "Invalid ontologyCodeSystem. Please correct the errors and try again.");

            invalidOntologyCodeSystemException.AddData(
                key: nameof(OntologyCodeSystem.UpdatedDate),
                values: $"Date is not the same as {nameof(OntologyCodeSystem.CreatedDate)}");

            var expectedOntologyCodeSystemValidationException =
                new OntologyCodeSystemValidationException(
                    message: "OntologyCodeSystem validation errors occurred, please try again.",
                    innerException: invalidOntologyCodeSystemException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<OntologyCodeSystem> addOntologyCodeSystemTask =
                this.ontologyCodeSystemService.AddOntologyCodeSystemAsync(invalidOntologyCodeSystem);

            OntologyCodeSystemValidationException actualOntologyCodeSystemValidationException =
                await Assert.ThrowsAsync<OntologyCodeSystemValidationException>(() =>
                    addOntologyCodeSystemTask.AsTask());

            // then
            actualOntologyCodeSystemValidationException.Should()
                .BeEquivalentTo(expectedOntologyCodeSystemValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyCodeSystemValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOntologyCodeSystemAsync(It.IsAny<OntologyCodeSystem>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateUsersIsNotSameAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            OntologyCodeSystem randomOntologyCodeSystem = CreateRandomOntologyCodeSystem(randomDateTimeOffset);
            OntologyCodeSystem invalidOntologyCodeSystem = randomOntologyCodeSystem;
            invalidOntologyCodeSystem.UpdatedBy = Guid.NewGuid().ToString();

            var invalidOntologyCodeSystemException =
                new InvalidOntologyCodeSystemException(
                    message: "Invalid ontologyCodeSystem. Please correct the errors and try again.");

            invalidOntologyCodeSystemException.AddData(
                key: nameof(OntologyCodeSystem.UpdatedBy),
                values: $"Text is not the same as {nameof(OntologyCodeSystem.CreatedBy)}");

            var expectedOntologyCodeSystemValidationException =
                new OntologyCodeSystemValidationException(
                    message: "OntologyCodeSystem validation errors occurred, please try again.",
                    innerException: invalidOntologyCodeSystemException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<OntologyCodeSystem> addOntologyCodeSystemTask =
                this.ontologyCodeSystemService.AddOntologyCodeSystemAsync(invalidOntologyCodeSystem);

            OntologyCodeSystemValidationException actualOntologyCodeSystemValidationException =
                await Assert.ThrowsAsync<OntologyCodeSystemValidationException>(() =>
                    addOntologyCodeSystemTask.AsTask());

            // then
            actualOntologyCodeSystemValidationException.Should()
                .BeEquivalentTo(expectedOntologyCodeSystemValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyCodeSystemValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOntologyCodeSystemAsync(It.IsAny<OntologyCodeSystem>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync(
            int minutesBeforeOrAfter)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            DateTimeOffset invalidDateTime =
                randomDateTimeOffset.AddMinutes(minutesBeforeOrAfter);

            OntologyCodeSystem randomOntologyCodeSystem = CreateRandomOntologyCodeSystem(invalidDateTime);
            OntologyCodeSystem invalidOntologyCodeSystem = randomOntologyCodeSystem;

            var invalidOntologyCodeSystemException =
                new InvalidOntologyCodeSystemException(
                    message: "Invalid ontologyCodeSystem. Please correct the errors and try again.");

            invalidOntologyCodeSystemException.AddData(
                key: nameof(OntologyCodeSystem.CreatedDate),
                values: "Date is not recent");

            var expectedOntologyCodeSystemValidationException =
                new OntologyCodeSystemValidationException(
                    message: "OntologyCodeSystem validation errors occurred, please try again.",
                    innerException: invalidOntologyCodeSystemException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<OntologyCodeSystem> addOntologyCodeSystemTask =
                this.ontologyCodeSystemService.AddOntologyCodeSystemAsync(invalidOntologyCodeSystem);

            OntologyCodeSystemValidationException actualOntologyCodeSystemValidationException =
                await Assert.ThrowsAsync<OntologyCodeSystemValidationException>(() =>
                    addOntologyCodeSystemTask.AsTask());

            // then
            actualOntologyCodeSystemValidationException.Should()
                .BeEquivalentTo(expectedOntologyCodeSystemValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyCodeSystemValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOntologyCodeSystemAsync(It.IsAny<OntologyCodeSystem>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}