// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.TerminologyArtifacts
{
    public partial class TerminologyArtifactServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfTerminologyArtifactIsNullAndLogItAsync()
        {
            // given
            TerminologyArtifact nullTerminologyArtifact = null;
            var nullTerminologyArtifactException = new NullTerminologyArtifactException(message: "TerminologyArtifact is null.");

            var expectedTerminologyArtifactValidationException =
                new TerminologyArtifactValidationException(
                    message: "TerminologyArtifact validation errors occurred, please try again.",
                    innerException: nullTerminologyArtifactException);

            // when
            ValueTask<TerminologyArtifact> modifyTerminologyArtifactTask =
                this.terminologyArtifactService.ModifyTerminologyArtifactAsync(nullTerminologyArtifact);

            TerminologyArtifactValidationException actualTerminologyArtifactValidationException =
                await Assert.ThrowsAsync<TerminologyArtifactValidationException>(
                    modifyTerminologyArtifactTask.AsTask);

            // then
            actualTerminologyArtifactValidationException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTerminologyArtifactAsync(It.IsAny<TerminologyArtifact>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfTerminologyArtifactIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            var invalidTerminologyArtifact = new TerminologyArtifact
            {
                FullUrl = invalidText,
                ResourceType = invalidText,
            };

            var invalidTerminologyArtifactException =
                new InvalidTerminologyArtifactException(
                    message: "Invalid terminologyArtifact. Please correct the errors and try again.");

            invalidTerminologyArtifactException.AddData(
                key: nameof(TerminologyArtifact.Id),
                values: "Id is required");

            invalidTerminologyArtifactException.AddData(
                key: nameof(TerminologyArtifact.FullUrl),
                values: "Text is required");

            invalidTerminologyArtifactException.AddData(
                key: nameof(TerminologyArtifact.ResourceType),
                values: "Text is required");

            invalidTerminologyArtifactException.AddData(
                key: nameof(TerminologyArtifact.CreatedDate),
                values: "Date is required");

            invalidTerminologyArtifactException.AddData(
                key: nameof(TerminologyArtifact.CreatedBy),
                values: "Text is required");

            invalidTerminologyArtifactException.AddData(
                key: nameof(TerminologyArtifact.UpdatedDate),
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(TerminologyArtifact.CreatedDate)}"
                });

            invalidTerminologyArtifactException.AddData(
                key: nameof(TerminologyArtifact.UpdatedBy),
                values: "Text is required");

            var expectedTerminologyArtifactValidationException =
                new TerminologyArtifactValidationException(
                    message: "TerminologyArtifact validation errors occurred, please try again.",
                    innerException: invalidTerminologyArtifactException);

            // when
            ValueTask<TerminologyArtifact> modifyTerminologyArtifactTask =
                this.terminologyArtifactService.ModifyTerminologyArtifactAsync(invalidTerminologyArtifact);

            TerminologyArtifactValidationException actualTerminologyArtifactValidationException =
                await Assert.ThrowsAsync<TerminologyArtifactValidationException>(
                    modifyTerminologyArtifactTask.AsTask);

            //then
            actualTerminologyArtifactValidationException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTerminologyArtifactAsync(It.IsAny<TerminologyArtifact>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            TerminologyArtifact randomTerminologyArtifact = CreateRandomTerminologyArtifact(randomDateTimeOffset);
            TerminologyArtifact invalidTerminologyArtifact = randomTerminologyArtifact;

            var invalidTerminologyArtifactException =
                new InvalidTerminologyArtifactException(
                    message: "Invalid terminologyArtifact. Please correct the errors and try again.");

            invalidTerminologyArtifactException.AddData(
                key: nameof(TerminologyArtifact.UpdatedDate),
                values: $"Date is the same as {nameof(TerminologyArtifact.CreatedDate)}");

            var expectedTerminologyArtifactValidationException =
                new TerminologyArtifactValidationException(
                    message: "TerminologyArtifact validation errors occurred, please try again.",
                    innerException: invalidTerminologyArtifactException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<TerminologyArtifact> modifyTerminologyArtifactTask =
                this.terminologyArtifactService.ModifyTerminologyArtifactAsync(invalidTerminologyArtifact);

            TerminologyArtifactValidationException actualTerminologyArtifactValidationException =
                await Assert.ThrowsAsync<TerminologyArtifactValidationException>(
                    modifyTerminologyArtifactTask.AsTask);

            // then
            actualTerminologyArtifactValidationException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyArtifactByIdAsync(invalidTerminologyArtifact.Id),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(int minutes)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            TerminologyArtifact randomTerminologyArtifact = CreateRandomTerminologyArtifact(randomDateTimeOffset);
            randomTerminologyArtifact.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var invalidTerminologyArtifactException =
                new InvalidTerminologyArtifactException(
                    message: "Invalid terminologyArtifact. Please correct the errors and try again.");

            invalidTerminologyArtifactException.AddData(
                key: nameof(TerminologyArtifact.UpdatedDate),
                values: "Date is not recent");

            var expectedTerminologyArtifactValidatonException =
                new TerminologyArtifactValidationException(
                    message: "TerminologyArtifact validation errors occurred, please try again.",
                    innerException: invalidTerminologyArtifactException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<TerminologyArtifact> modifyTerminologyArtifactTask =
                this.terminologyArtifactService.ModifyTerminologyArtifactAsync(randomTerminologyArtifact);

            TerminologyArtifactValidationException actualTerminologyArtifactValidationException =
                await Assert.ThrowsAsync<TerminologyArtifactValidationException>(
                    modifyTerminologyArtifactTask.AsTask);

            // then
            actualTerminologyArtifactValidationException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyArtifactByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfTerminologyArtifactDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            TerminologyArtifact randomTerminologyArtifact = CreateRandomModifyTerminologyArtifact(randomDateTimeOffset);
            TerminologyArtifact nonExistTerminologyArtifact = randomTerminologyArtifact;
            TerminologyArtifact nullTerminologyArtifact = null;

            var notFoundTerminologyArtifactException =
                new NotFoundTerminologyArtifactException(nonExistTerminologyArtifact.Id);

            var expectedTerminologyArtifactValidationException =
                new TerminologyArtifactValidationException(
                    message: "TerminologyArtifact validation errors occurred, please try again.",
                    innerException: notFoundTerminologyArtifactException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyArtifactByIdAsync(nonExistTerminologyArtifact.Id))
                .ReturnsAsync(nullTerminologyArtifact);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            // when 
            ValueTask<TerminologyArtifact> modifyTerminologyArtifactTask =
                this.terminologyArtifactService.ModifyTerminologyArtifactAsync(nonExistTerminologyArtifact);

            TerminologyArtifactValidationException actualTerminologyArtifactValidationException =
                await Assert.ThrowsAsync<TerminologyArtifactValidationException>(
                    modifyTerminologyArtifactTask.AsTask);

            // then
            actualTerminologyArtifactValidationException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyArtifactByIdAsync(nonExistTerminologyArtifact.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNegativeNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            TerminologyArtifact randomTerminologyArtifact = CreateRandomModifyTerminologyArtifact(randomDateTimeOffset);
            TerminologyArtifact invalidTerminologyArtifact = randomTerminologyArtifact.DeepClone();
            TerminologyArtifact storageTerminologyArtifact = invalidTerminologyArtifact.DeepClone();
            storageTerminologyArtifact.CreatedDate = storageTerminologyArtifact.CreatedDate.AddMinutes(randomMinutes);
            storageTerminologyArtifact.UpdatedDate = storageTerminologyArtifact.UpdatedDate.AddMinutes(randomMinutes);

            var invalidTerminologyArtifactException =
                new InvalidTerminologyArtifactException(
                    message: "Invalid terminologyArtifact. Please correct the errors and try again.");

            invalidTerminologyArtifactException.AddData(
                key: nameof(TerminologyArtifact.CreatedDate),
                values: $"Date is not the same as {nameof(TerminologyArtifact.CreatedDate)}");

            var expectedTerminologyArtifactValidationException =
                new TerminologyArtifactValidationException(
                    message: "TerminologyArtifact validation errors occurred, please try again.",
                    innerException: invalidTerminologyArtifactException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyArtifactByIdAsync(invalidTerminologyArtifact.Id))
                .ReturnsAsync(storageTerminologyArtifact);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<TerminologyArtifact> modifyTerminologyArtifactTask =
                this.terminologyArtifactService.ModifyTerminologyArtifactAsync(invalidTerminologyArtifact);

            TerminologyArtifactValidationException actualTerminologyArtifactValidationException =
                await Assert.ThrowsAsync<TerminologyArtifactValidationException>(
                    modifyTerminologyArtifactTask.AsTask);

            // then
            actualTerminologyArtifactValidationException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyArtifactByIdAsync(invalidTerminologyArtifact.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedTerminologyArtifactValidationException))),
                       Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCreatedUserDontMatchStorageAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            TerminologyArtifact randomTerminologyArtifact = CreateRandomModifyTerminologyArtifact(randomDateTimeOffset);
            TerminologyArtifact invalidTerminologyArtifact = randomTerminologyArtifact.DeepClone();
            TerminologyArtifact storageTerminologyArtifact = invalidTerminologyArtifact.DeepClone();
            invalidTerminologyArtifact.CreatedBy = Guid.NewGuid().ToString();
            storageTerminologyArtifact.UpdatedDate = storageTerminologyArtifact.CreatedDate;

            var invalidTerminologyArtifactException =
                new InvalidTerminologyArtifactException(
                    message: "Invalid terminologyArtifact. Please correct the errors and try again.");

            invalidTerminologyArtifactException.AddData(
                key: nameof(TerminologyArtifact.CreatedBy),
                values: $"Text is not the same as {nameof(TerminologyArtifact.CreatedBy)}");

            var expectedTerminologyArtifactValidationException =
                new TerminologyArtifactValidationException(
                    message: "TerminologyArtifact validation errors occurred, please try again.",
                    innerException: invalidTerminologyArtifactException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyArtifactByIdAsync(invalidTerminologyArtifact.Id))
                .ReturnsAsync(storageTerminologyArtifact);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<TerminologyArtifact> modifyTerminologyArtifactTask =
                this.terminologyArtifactService.ModifyTerminologyArtifactAsync(invalidTerminologyArtifact);

            TerminologyArtifactValidationException actualTerminologyArtifactValidationException =
                await Assert.ThrowsAsync<TerminologyArtifactValidationException>(
                    modifyTerminologyArtifactTask.AsTask);

            // then
            actualTerminologyArtifactValidationException.Should().BeEquivalentTo(expectedTerminologyArtifactValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyArtifactByIdAsync(invalidTerminologyArtifact.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedTerminologyArtifactValidationException))),
                       Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            TerminologyArtifact randomTerminologyArtifact = CreateRandomModifyTerminologyArtifact(randomDateTimeOffset);
            TerminologyArtifact invalidTerminologyArtifact = randomTerminologyArtifact;
            TerminologyArtifact storageTerminologyArtifact = randomTerminologyArtifact.DeepClone();

            var invalidTerminologyArtifactException =
                new InvalidTerminologyArtifactException(
                    message: "Invalid terminologyArtifact. Please correct the errors and try again.");

            invalidTerminologyArtifactException.AddData(
                key: nameof(TerminologyArtifact.UpdatedDate),
                values: $"Date is the same as {nameof(TerminologyArtifact.UpdatedDate)}");

            var expectedTerminologyArtifactValidationException =
                new TerminologyArtifactValidationException(
                    message: "TerminologyArtifact validation errors occurred, please try again.",
                    innerException: invalidTerminologyArtifactException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyArtifactByIdAsync(invalidTerminologyArtifact.Id))
                .ReturnsAsync(storageTerminologyArtifact);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<TerminologyArtifact> modifyTerminologyArtifactTask =
                this.terminologyArtifactService.ModifyTerminologyArtifactAsync(invalidTerminologyArtifact);

            // then
            await Assert.ThrowsAsync<TerminologyArtifactValidationException>(
                modifyTerminologyArtifactTask.AsTask);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyArtifactByIdAsync(invalidTerminologyArtifact.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}