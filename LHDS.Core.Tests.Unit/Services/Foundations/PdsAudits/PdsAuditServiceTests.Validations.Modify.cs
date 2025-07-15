// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Models.Foundations.PdsAudits.Exceptions;
using LHDS.Core.Services.Foundations.PdsAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.PdsAudits
{
    public partial class PdsAuditServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfPdsAuditIsNullAndLogItAsync()
        {
            // given
            PdsAudit nullPdsAudit = null;
            var nullPdsAuditException = new NullPdsAuditException(message: "PdsAudit is null.");

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(
                    message: "PdsAudit validation errors occurred, please try again.",
                    innerException: nullPdsAuditException);

            // when
            ValueTask<PdsAudit> modifyPdsAuditTask =
                this.pdsAuditService.ModifyPdsAuditAsync(nullPdsAudit);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(
                    modifyPdsAuditTask.AsTask);

            // then
            actualPdsAuditValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedPdsAuditValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePdsAuditAsync(It.IsAny<PdsAudit>()),
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
        public async Task ShouldThrowValidationExceptionOnModifyIfPdsAuditIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            EntraUser randomEntraUser = CreateRandomEntraUser();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            var invalidPdsAudit = new PdsAudit
            {
                FileName = invalidText,
                Message = invalidText,
                MessageId = invalidText,
                RequestType = invalidText,
            };

            var pdsAuditServiceMock = new Mock<PdsAuditService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            pdsAuditServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidPdsAudit))
                    .ReturnsAsync(invalidPdsAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidPdsAuditException =
                new InvalidPdsAuditException(
                    message: "Invalid pdsAudit. Please correct the errors and try again.");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.Id),
                values: "Id is required");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.CorrelationId),
                values: "Id is required");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.FileName),
                values: "Text is required");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.Message),
                values: "Text is required");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.MessageId),
                values: "Text is required");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.CreatedDate),
                values: "Date is required");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.CreatedBy),
                values: "Text is required");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.UpdatedDate),
                values:
                    [
                        "Date is required",
                        "Date is the same as CreatedDate",
                        $"Date is not recent"
                    ]);

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.UpdatedBy),
                values:
                    [
                        "Text is required",
                        $"Expected value to be '{randomEntraUser.EntraUserId}' but found " +
                        $"'{invalidPdsAudit.UpdatedBy}'."
                    ]);

            invalidPdsAuditException.AddData(
               key: nameof(PdsAudit.RequestType),
               values: "Text is required");

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(
                    message: "PdsAudit validation errors occurred, please try again.",
                    innerException: invalidPdsAuditException);

            // when
            ValueTask<PdsAudit> modifyPdsAuditTask =
                pdsAuditServiceMock.Object.ModifyPdsAuditAsync(invalidPdsAudit);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(
                    modifyPdsAuditTask.AsTask);

            //then
            actualPdsAuditValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditValidationException);

            pdsAuditServiceMock.Verify(service =>
                service.ApplyModifyAuditAsync(invalidPdsAudit),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedPdsAuditValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePdsAuditAsync(It.IsAny<PdsAudit>()),
                    Times.Never);

            pdsAuditServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfPdsAuditIsInvalidLengthAndLogItAsync()
        {
            // given 
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser(entraUserId: GetRandomStringWithLengthOf(256));

            PdsAudit invalidPdsAudit =
                CreateRandomModifyPdsAudit(randomDateTimeOffset, randomEntraUser.EntraUserId);

            var inputCreatedByUpdatedByString = randomEntraUser.EntraUserId;
            invalidPdsAudit.CreatedBy = inputCreatedByUpdatedByString;
            invalidPdsAudit.UpdatedBy = inputCreatedByUpdatedByString;

            var pdsAuditServiceMock = new Mock<PdsAuditService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };
                        
            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidPdsAuditException =
                new InvalidPdsAuditException(
                    message: "Invalid pdsAudit. Please correct the errors and try again.");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.CreatedBy),
                values: "Text is exceeding max length");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.UpdatedBy),
                values: "Text is exceeding max length");

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(
                    message: "PdsAudit validation errors occurred, please try again.",
                    innerException: invalidPdsAuditException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<PdsAudit> modifyPdsAuditTask =
                pdsAuditServiceMock.Object.ModifyPdsAuditAsync(invalidPdsAudit);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(
                    modifyPdsAuditTask.AsTask);

            //then
            actualPdsAuditValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditValidationException);

            pdsAuditServiceMock.Verify(service =>
                service.ApplyModifyAuditAsync(invalidPdsAudit),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(2));

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedPdsAuditValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePdsAuditAsync(It.IsAny<PdsAudit>()),
                    Times.Never);

            pdsAuditServiceMock.VerifyNoOtherCalls();
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

            PdsAudit randomPdsAudit =
                CreateRandomPdsAudit(randomDateTimeOffset, randomEntraUser.EntraUserId);

            PdsAudit invalidPdsAudit = randomPdsAudit;

            var pdsAuditServiceMock = new Mock<PdsAuditService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            pdsAuditServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidPdsAudit))
                    .ReturnsAsync(invalidPdsAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidPdsAuditException =
                new InvalidPdsAuditException(
                    message: "Invalid pdsAudit. Please correct the errors and try again.");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.UpdatedDate),
                values: $"Date is the same as {nameof(PdsAudit.CreatedDate)}");

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(
                    message: "PdsAudit validation errors occurred, please try again.",
                    innerException: invalidPdsAuditException);

            // when
            ValueTask<PdsAudit> modifyPdsAuditTask =
                pdsAuditServiceMock.Object.ModifyPdsAuditAsync(invalidPdsAudit);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(
                    modifyPdsAuditTask.AsTask);

            // then
            actualPdsAuditValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditValidationException);

            pdsAuditServiceMock.Verify(service =>
                service.ApplyModifyAuditAsync(invalidPdsAudit),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedPdsAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(invalidPdsAudit.Id),
                    Times.Never);

            pdsAuditServiceMock.VerifyNoOtherCalls();
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

            PdsAudit invalidPdsAudit =
                CreateRandomPdsAudit(randomDateTimeOffset, randomEntraUser.EntraUserId);

            invalidPdsAudit.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var pdsAuditServiceMock = new Mock<PdsAuditService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            pdsAuditServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidPdsAudit))
                    .ReturnsAsync(invalidPdsAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidPdsAuditException =
                new InvalidPdsAuditException(
                    message: "Invalid pdsAudit. Please correct the errors and try again.");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.UpdatedDate),
                values: "Date is not recent");

            var expectedPdsAuditValidatonException =
                new PdsAuditValidationException(
                    message: "PdsAudit validation errors occurred, please try again.",
                    innerException: invalidPdsAuditException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<PdsAudit> modifyPdsAuditTask =
                pdsAuditServiceMock.Object.ModifyPdsAuditAsync(invalidPdsAudit);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(
                    modifyPdsAuditTask.AsTask);

            // then
            actualPdsAuditValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditValidatonException);

            pdsAuditServiceMock.Verify(service =>
                service.ApplyModifyAuditAsync(invalidPdsAudit),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedPdsAuditValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            pdsAuditServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfPdsAuditDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            PdsAudit invalidPdsAudit =
                CreateRandomModifyPdsAudit(randomDateTimeOffset, randomEntraUser.EntraUserId);

            PdsAudit nonExistPdsAudit = invalidPdsAudit;
            var notFoundPdsAuditException = new NotFoundPdsAuditException(nonExistPdsAudit.Id);

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(
                    message: "PdsAudit validation errors occurred, please try again.",
                    innerException: notFoundPdsAuditException);

            var pdsAuditServiceMock = new Mock<PdsAuditService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            pdsAuditServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidPdsAudit))
                    .ReturnsAsync(invalidPdsAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when 
            ValueTask<PdsAudit> modifyPdsAuditTask =
                pdsAuditServiceMock.Object.ModifyPdsAuditAsync(nonExistPdsAudit);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(
                    modifyPdsAuditTask.AsTask);

            // then
            actualPdsAuditValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditValidationException);

            pdsAuditServiceMock.Verify(service =>
                service.ApplyModifyAuditAsync(invalidPdsAudit),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(nonExistPdsAudit.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedPdsAuditValidationException))),
                        Times.Once);

            pdsAuditServiceMock.VerifyNoOtherCalls();
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

            PdsAudit randomPdsAudit =
                CreateRandomModifyPdsAudit(randomDateTimeOffset, randomEntraUser.EntraUserId);

            PdsAudit invalidPdsAudit = randomPdsAudit.DeepClone();
            PdsAudit storagePdsAudit = invalidPdsAudit.DeepClone();
            storagePdsAudit.CreatedDate = storagePdsAudit.CreatedDate.AddMinutes(randomMinutes);
            storagePdsAudit.UpdatedDate = storagePdsAudit.UpdatedDate.AddMinutes(randomMinutes);

            var pdsAuditServiceMock = new Mock<PdsAuditService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            pdsAuditServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidPdsAudit))
                    .ReturnsAsync(invalidPdsAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidPdsAuditException =
                new InvalidPdsAuditException(
                    message: "Invalid pdsAudit. Please correct the errors and try again.");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.CreatedDate),
                values: $"Date is not the same as {nameof(PdsAudit.CreatedDate)}");

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(
                    message: "PdsAudit validation errors occurred, please try again.",
                    innerException: invalidPdsAuditException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPdsAuditByIdAsync(invalidPdsAudit.Id))
                    .ReturnsAsync(storagePdsAudit);

            // when
            ValueTask<PdsAudit> modifyPdsAuditTask =
                pdsAuditServiceMock.Object.ModifyPdsAuditAsync(invalidPdsAudit);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(
                    modifyPdsAuditTask.AsTask);

            // then
            actualPdsAuditValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditValidationException);

            pdsAuditServiceMock.Verify(service =>
                service.ApplyModifyAuditAsync(invalidPdsAudit),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(invalidPdsAudit.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedPdsAuditValidationException))),
                       Times.Once);

            pdsAuditServiceMock.VerifyNoOtherCalls();
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

            PdsAudit randomPdsAudit =
                CreateRandomModifyPdsAudit(randomDateTimeOffset, randomEntraUser.EntraUserId);

            PdsAudit invalidPdsAudit = randomPdsAudit.DeepClone();
            PdsAudit storagePdsAudit = invalidPdsAudit.DeepClone();
            invalidPdsAudit.CreatedBy = Guid.NewGuid().ToString();
            storagePdsAudit.UpdatedDate = storagePdsAudit.CreatedDate;

            var invalidPdsAuditException =
                new InvalidPdsAuditException(
                    message: "Invalid pdsAudit. Please correct the errors and try again.");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.CreatedBy),
                values: $"Text is not the same as {nameof(PdsAudit.CreatedBy)}");

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(
                    message: "PdsAudit validation errors occurred, please try again.",
                    innerException: invalidPdsAuditException);

            var pdsAuditServiceMock = new Mock<PdsAuditService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            pdsAuditServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidPdsAudit))
                    .ReturnsAsync(invalidPdsAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPdsAuditByIdAsync(invalidPdsAudit.Id))
                    .ReturnsAsync(storagePdsAudit);

            // when
            ValueTask<PdsAudit> modifyPdsAuditTask =
                pdsAuditServiceMock.Object.ModifyPdsAuditAsync(invalidPdsAudit);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(
                    modifyPdsAuditTask.AsTask);

            // then
            actualPdsAuditValidationException.Should().BeEquivalentTo(expectedPdsAuditValidationException);

            pdsAuditServiceMock.Verify(service =>
                service.ApplyModifyAuditAsync(invalidPdsAudit),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(invalidPdsAudit.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedPdsAuditValidationException))),
                       Times.Once);

            pdsAuditServiceMock.VerifyNoOtherCalls();
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

            PdsAudit randomPdsAudit =
                CreateRandomModifyPdsAudit(randomDateTimeOffset, randomEntraUser.EntraUserId);

            PdsAudit invalidPdsAudit = randomPdsAudit;
            PdsAudit storagePdsAudit = randomPdsAudit.DeepClone();
            invalidPdsAudit.UpdatedDate = storagePdsAudit.UpdatedDate;

            var invalidPdsAuditException =
                new InvalidPdsAuditException(
                    message: "Invalid pdsAudit. Please correct the errors and try again.");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.UpdatedDate),
                values: $"Date is the same as {nameof(PdsAudit.UpdatedDate)}");

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(
                    message: "PdsAudit validation errors occurred, please try again.",
                    innerException: invalidPdsAuditException);

            var pdsAuditServiceMock = new Mock<PdsAuditService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            pdsAuditServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidPdsAudit))
                    .ReturnsAsync(invalidPdsAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPdsAuditByIdAsync(invalidPdsAudit.Id))
                    .ReturnsAsync(storagePdsAudit);

            // when
            ValueTask<PdsAudit> modifyPdsAuditTask =
                pdsAuditServiceMock.Object.ModifyPdsAuditAsync(invalidPdsAudit);

            // then
            await Assert.ThrowsAsync<PdsAuditValidationException>(
                modifyPdsAuditTask.AsTask);

            pdsAuditServiceMock.Verify(service =>
                service.ApplyModifyAuditAsync(invalidPdsAudit),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedPdsAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(invalidPdsAudit.Id),
                    Times.Once);

            pdsAuditServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}