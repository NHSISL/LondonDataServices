// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions;
using LHDS.Core.Services.Foundations.TerminologyPolls;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.TerminologyPolls
{
    public partial class TerminologyPollServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfTerminologyPollIsNullAndLogItAsync()
        {
            // given
            TerminologyPoll nullTerminologyPoll = null;
            var nullTerminologyPollException = new NullTerminologyPollException(message: "TerminologyPoll is null.");

            var expectedTerminologyPollValidationException =
                new TerminologyPollValidationException(
                    message: "TerminologyPoll validation errors occurred, please try again.",
                    innerException: nullTerminologyPollException);

            // when
            ValueTask<TerminologyPoll> modifyTerminologyPollTask =
                this.terminologyPollService.ModifyTerminologyPollAsync(nullTerminologyPoll);

            TerminologyPollValidationException actualTerminologyPollValidationException =
                await Assert.ThrowsAsync<TerminologyPollValidationException>(
                    modifyTerminologyPollTask.AsTask);

            // then
            actualTerminologyPollValidationException.Should()
                .BeEquivalentTo(expectedTerminologyPollValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedTerminologyPollValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTerminologyPollAsync(It.IsAny<TerminologyPoll>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfTerminologyPollIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            EntraUser randomEntraUser = CreateRandomEntraUser();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            var invalidTerminologyPoll = new TerminologyPoll
            {
                ResourceType = invalidText,
            };

            var terminologyPollServiceMock = new Mock<TerminologyPollService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            terminologyPollServiceMock.Setup(service =>
                service.ApplyModifyTerminologyPollAsync(invalidTerminologyPoll))
                    .ReturnsAsync(invalidTerminologyPoll);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidTerminologyPollException =
                new InvalidTerminologyPollException(
                    message: "Invalid terminologyPoll. Please correct the errors and try again.");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.Id),
                values: "Id is required");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.ResourceType),
                values: "Text is required");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.LastPoll),
                values: "Date is required");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.CreatedDate),
                values: "Date is required");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.CreatedBy),
                values: "Text is required");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.UpdatedDate),
                values:
                    [
                        "Date is required",
                        "Date is the same as CreatedDate",
                        $"Date is not recent"
                    ]);

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.UpdatedBy),
                values:
                    [
                        "Text is required",
                        $"Expected value to be '{randomEntraUser.EntraUserId}' but found " +
                        $"'{invalidTerminologyPoll.UpdatedBy}'."
                    ]);

            var expectedTerminologyPollValidationException =
                new TerminologyPollValidationException(
                    message: "TerminologyPoll validation errors occurred, please try again.",
                    innerException: invalidTerminologyPollException);

            // when
            ValueTask<TerminologyPoll> modifyTerminologyPollTask =
                terminologyPollServiceMock.Object.ModifyTerminologyPollAsync(invalidTerminologyPoll);

            TerminologyPollValidationException actualTerminologyPollValidationException =
                await Assert.ThrowsAsync<TerminologyPollValidationException>(
                    modifyTerminologyPollTask.AsTask);

            //then
            actualTerminologyPollValidationException.Should()
                .BeEquivalentTo(expectedTerminologyPollValidationException);

            terminologyPollServiceMock.Verify(service =>
                service.ApplyModifyTerminologyPollAsync(invalidTerminologyPoll),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedTerminologyPollValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTerminologyPollAsync(It.IsAny<TerminologyPoll>()),
                    Times.Never);

            terminologyPollServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            TerminologyPoll randomTerminologyPoll =
                CreateRandomTerminologyPoll(randomDateTimeOffset, randomEntraUser.EntraUserId);

            TerminologyPoll invalidTerminologyPoll = randomTerminologyPoll;

            var terminologyPollServiceMock = new Mock<TerminologyPollService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            terminologyPollServiceMock.Setup(service =>
                service.ApplyModifyTerminologyPollAsync(invalidTerminologyPoll))
                    .ReturnsAsync(invalidTerminologyPoll);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidTerminologyPollException =
                new InvalidTerminologyPollException(
                    message: "Invalid terminologyPoll. Please correct the errors and try again.");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.UpdatedDate),
                values: $"Date is the same as {nameof(TerminologyPoll.CreatedDate)}");

            var expectedTerminologyPollValidationException =
                new TerminologyPollValidationException(
                    message: "TerminologyPoll validation errors occurred, please try again.",
                    innerException: invalidTerminologyPollException);

            // when
            ValueTask<TerminologyPoll> modifyTerminologyPollTask =
                terminologyPollServiceMock.Object.ModifyTerminologyPollAsync(invalidTerminologyPoll);

            TerminologyPollValidationException actualTerminologyPollValidationException =
                await Assert.ThrowsAsync<TerminologyPollValidationException>(
                    modifyTerminologyPollTask.AsTask);

            // then
            actualTerminologyPollValidationException.Should()
                .BeEquivalentTo(expectedTerminologyPollValidationException);

            terminologyPollServiceMock.Verify(service =>
                service.ApplyModifyTerminologyPollAsync(invalidTerminologyPoll),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedTerminologyPollValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyPollByIdAsync(invalidTerminologyPoll.Id),
                    Times.Never);

            terminologyPollServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(int minutes)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            TerminologyPoll invalidTerminologyPoll =
                CreateRandomTerminologyPoll(randomDateTimeOffset, randomEntraUser.EntraUserId);

            invalidTerminologyPoll.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var terminologyPollServiceMock = new Mock<TerminologyPollService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            terminologyPollServiceMock.Setup(service =>
                service.ApplyModifyTerminologyPollAsync(invalidTerminologyPoll))
                    .ReturnsAsync(invalidTerminologyPoll);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidTerminologyPollException =
                new InvalidTerminologyPollException(
                    message: "Invalid terminologyPoll. Please correct the errors and try again.");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.UpdatedDate),
                values: "Date is not recent");

            var expectedTerminologyPollValidatonException =
                new TerminologyPollValidationException(
                    message: "TerminologyPoll validation errors occurred, please try again.",
                    innerException: invalidTerminologyPollException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<TerminologyPoll> modifyTerminologyPollTask =
                terminologyPollServiceMock.Object.ModifyTerminologyPollAsync(invalidTerminologyPoll);

            TerminologyPollValidationException actualTerminologyPollValidationException =
                await Assert.ThrowsAsync<TerminologyPollValidationException>(
                    modifyTerminologyPollTask.AsTask);

            // then
            actualTerminologyPollValidationException.Should()
                .BeEquivalentTo(expectedTerminologyPollValidatonException);

            terminologyPollServiceMock.Verify(service =>
                service.ApplyModifyTerminologyPollAsync(invalidTerminologyPoll),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedTerminologyPollValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyPollByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            terminologyPollServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfTerminologyPollDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            TerminologyPoll invalidTerminologyPoll =
                CreateRandomModifyTerminologyPoll(randomDateTimeOffset, randomEntraUser.EntraUserId);

            TerminologyPoll nonExistTerminologyPoll = invalidTerminologyPoll;
            var notFoundTerminologyPollException = new NotFoundTerminologyPollException(nonExistTerminologyPoll.Id);

            var expectedTerminologyPollValidationException =
                new TerminologyPollValidationException(
                    message: "TerminologyPoll validation errors occurred, please try again.",
                    innerException: notFoundTerminologyPollException);

            var terminologyPollServiceMock = new Mock<TerminologyPollService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            terminologyPollServiceMock.Setup(service =>
                service.ApplyModifyTerminologyPollAsync(invalidTerminologyPoll))
                    .ReturnsAsync(invalidTerminologyPoll);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when 
            ValueTask<TerminologyPoll> modifyTerminologyPollTask =
                terminologyPollServiceMock.Object.ModifyTerminologyPollAsync(nonExistTerminologyPoll);

            TerminologyPollValidationException actualTerminologyPollValidationException =
                await Assert.ThrowsAsync<TerminologyPollValidationException>(
                    modifyTerminologyPollTask.AsTask);

            // then
            actualTerminologyPollValidationException.Should()
                .BeEquivalentTo(expectedTerminologyPollValidationException);

            terminologyPollServiceMock.Verify(service =>
                service.ApplyModifyTerminologyPollAsync(invalidTerminologyPoll),
                    Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyPollByIdAsync(nonExistTerminologyPoll.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedTerminologyPollValidationException))),
                        Times.Once);

            terminologyPollServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNegativeNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            TerminologyPoll randomTerminologyPoll =
                CreateRandomModifyTerminologyPoll(randomDateTimeOffset, randomEntraUser.EntraUserId);

            TerminologyPoll invalidTerminologyPoll = randomTerminologyPoll.DeepClone();
            TerminologyPoll storageTerminologyPoll = invalidTerminologyPoll.DeepClone();
            storageTerminologyPoll.CreatedDate = storageTerminologyPoll.CreatedDate.AddMinutes(randomMinutes);
            storageTerminologyPoll.UpdatedDate = storageTerminologyPoll.UpdatedDate.AddMinutes(randomMinutes);

            var terminologyPollServiceMock = new Mock<TerminologyPollService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            terminologyPollServiceMock.Setup(service =>
                service.ApplyModifyTerminologyPollAsync(invalidTerminologyPoll))
                    .ReturnsAsync(invalidTerminologyPoll);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidTerminologyPollException =
                new InvalidTerminologyPollException(
                    message: "Invalid terminologyPoll. Please correct the errors and try again.");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.CreatedDate),
                values: $"Date is not the same as {nameof(TerminologyPoll.CreatedDate)}");

            var expectedTerminologyPollValidationException =
                new TerminologyPollValidationException(
                    message: "TerminologyPoll validation errors occurred, please try again.",
                    innerException: invalidTerminologyPollException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyPollByIdAsync(invalidTerminologyPoll.Id))
                    .ReturnsAsync(storageTerminologyPoll);

            terminologyPollServiceMock.Setup(service =>
                service.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    invalidTerminologyPoll, storageTerminologyPoll))
                        .ReturnsAsync(invalidTerminologyPoll);

            // when
            ValueTask<TerminologyPoll> modifyTerminologyPollTask =
                terminologyPollServiceMock.Object.ModifyTerminologyPollAsync(invalidTerminologyPoll);

            TerminologyPollValidationException actualTerminologyPollValidationException =
                await Assert.ThrowsAsync<TerminologyPollValidationException>(
                    modifyTerminologyPollTask.AsTask);

            // then
            actualTerminologyPollValidationException.Should()
                .BeEquivalentTo(expectedTerminologyPollValidationException);

            terminologyPollServiceMock.Verify(service =>
                service.ApplyModifyTerminologyPollAsync(invalidTerminologyPoll),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyPollByIdAsync(invalidTerminologyPoll.Id),
                    Times.Once);

            terminologyPollServiceMock.Verify(service =>
                service.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    invalidTerminologyPoll, storageTerminologyPoll),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedTerminologyPollValidationException))),
                       Times.Once);

            terminologyPollServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCreatedUserDontMatchStorageAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            TerminologyPoll randomTerminologyPoll =
                CreateRandomModifyTerminologyPoll(randomDateTimeOffset, randomEntraUser.EntraUserId);

            TerminologyPoll invalidTerminologyPoll = randomTerminologyPoll.DeepClone();
            TerminologyPoll storageTerminologyPoll = invalidTerminologyPoll.DeepClone();
            invalidTerminologyPoll.CreatedBy = Guid.NewGuid().ToString();
            storageTerminologyPoll.UpdatedDate = storageTerminologyPoll.CreatedDate;

            var invalidTerminologyPollException =
                new InvalidTerminologyPollException(
                    message: "Invalid terminologyPoll. Please correct the errors and try again.");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.CreatedBy),
                values: $"Text is not the same as {nameof(TerminologyPoll.CreatedBy)}");

            var expectedTerminologyPollValidationException =
                new TerminologyPollValidationException(
                    message: "TerminologyPoll validation errors occurred, please try again.",
                    innerException: invalidTerminologyPollException);

            var terminologyPollServiceMock = new Mock<TerminologyPollService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            terminologyPollServiceMock.Setup(service =>
                service.ApplyModifyTerminologyPollAsync(invalidTerminologyPoll))
                    .ReturnsAsync(invalidTerminologyPoll);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyPollByIdAsync(invalidTerminologyPoll.Id))
                    .ReturnsAsync(storageTerminologyPoll);

            terminologyPollServiceMock.Setup(service =>
                service.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    invalidTerminologyPoll, storageTerminologyPoll))
                        .ReturnsAsync(invalidTerminologyPoll);

            terminologyPollServiceMock.Setup(service =>
                service.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    invalidTerminologyPoll, storageTerminologyPoll))
                        .ReturnsAsync(invalidTerminologyPoll);

            // when
            ValueTask<TerminologyPoll> modifyTerminologyPollTask =
                terminologyPollServiceMock.Object.ModifyTerminologyPollAsync(invalidTerminologyPoll);

            TerminologyPollValidationException actualTerminologyPollValidationException =
                await Assert.ThrowsAsync<TerminologyPollValidationException>(
                    modifyTerminologyPollTask.AsTask);

            // then
            actualTerminologyPollValidationException.Should().BeEquivalentTo(expectedTerminologyPollValidationException);

            terminologyPollServiceMock.Verify(service =>
                service.ApplyModifyTerminologyPollAsync(invalidTerminologyPoll),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyPollByIdAsync(invalidTerminologyPoll.Id),
                    Times.Once);

            terminologyPollServiceMock.Verify(service =>
                service.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    invalidTerminologyPoll, storageTerminologyPoll),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedTerminologyPollValidationException))),
                       Times.Once);

            terminologyPollServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            TerminologyPoll randomTerminologyPoll =
                CreateRandomModifyTerminologyPoll(randomDateTimeOffset, randomEntraUser.EntraUserId);

            TerminologyPoll invalidTerminologyPoll = randomTerminologyPoll;
            TerminologyPoll storageTerminologyPoll = randomTerminologyPoll.DeepClone();
            invalidTerminologyPoll.UpdatedDate = storageTerminologyPoll.UpdatedDate;

            var invalidTerminologyPollException =
                new InvalidTerminologyPollException(
                    message: "Invalid terminologyPoll. Please correct the errors and try again.");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.UpdatedDate),
                values: $"Date is the same as {nameof(TerminologyPoll.UpdatedDate)}");

            var expectedTerminologyPollValidationException =
                new TerminologyPollValidationException(
                    message: "TerminologyPoll validation errors occurred, please try again.",
                    innerException: invalidTerminologyPollException);

            var terminologyPollServiceMock = new Mock<TerminologyPollService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            terminologyPollServiceMock.Setup(service =>
                service.ApplyModifyTerminologyPollAsync(invalidTerminologyPoll))
                    .ReturnsAsync(invalidTerminologyPoll);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyPollByIdAsync(invalidTerminologyPoll.Id))
                    .ReturnsAsync(storageTerminologyPoll);

            terminologyPollServiceMock.Setup(service =>
                service.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    invalidTerminologyPoll, storageTerminologyPoll))
                        .ReturnsAsync(invalidTerminologyPoll);

            // when
            ValueTask<TerminologyPoll> modifyTerminologyPollTask =
                terminologyPollServiceMock.Object.ModifyTerminologyPollAsync(invalidTerminologyPoll);

            // then
            await Assert.ThrowsAsync<TerminologyPollValidationException>(
                modifyTerminologyPollTask.AsTask);

            terminologyPollServiceMock.Verify(service =>
                service.ApplyModifyTerminologyPollAsync(invalidTerminologyPoll),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyPollByIdAsync(invalidTerminologyPoll.Id),
                    Times.Once);

            terminologyPollServiceMock.Verify(service =>
                service.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    invalidTerminologyPoll, storageTerminologyPoll),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedTerminologyPollValidationException))),
                        Times.Once);

            terminologyPollServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}