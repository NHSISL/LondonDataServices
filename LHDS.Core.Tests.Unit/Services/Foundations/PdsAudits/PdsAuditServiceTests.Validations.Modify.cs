using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Core.Models.PdsAudits;
using LHDS.Core.Models.PdsAudits.Exceptions;
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
            var nullPdsAuditException = new NullPdsAuditException();

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(nullPdsAuditException);

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
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPdsAuditValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePdsAuditAsync(It.IsAny<PdsAudit>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfPdsAuditIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            var invalidPdsAudit = new PdsAudit
            {
                // TODO:  Add default values for your properties i.e. Name = invalidText
            };

            var invalidPdsAuditException = new InvalidPdsAuditException();

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.Id),
                values: "Id is required");

            //invalidPdsAuditException.AddData(
            //    key: nameof(PdsAudit.Name),
            //    values: "Text is required");

            // TODO: Add or remove data here to suit the validation needs for the PdsAudit model

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.CreatedDate),
                values: "Date is required");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.CreatedByUserId),
                values: "Id is required");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.UpdatedDate),
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(PdsAudit.CreatedDate)}"
                });

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.UpdatedByUserId),
                values: "Id is required");

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(invalidPdsAuditException);

            // when
            ValueTask<PdsAudit> modifyPdsAuditTask =
                this.pdsAuditService.ModifyPdsAuditAsync(invalidPdsAudit);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(
                    modifyPdsAuditTask.AsTask);

            //then
            actualPdsAuditValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPdsAuditValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePdsAuditAsync(It.IsAny<PdsAudit>()),
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
            PdsAudit randomPdsAudit = CreateRandomPdsAudit(randomDateTimeOffset);
            PdsAudit invalidPdsAudit = randomPdsAudit;
            var invalidPdsAuditException = new InvalidPdsAuditException();

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.UpdatedDate),
                values: $"Date is the same as {nameof(PdsAudit.CreatedDate)}");

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(invalidPdsAuditException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<PdsAudit> modifyPdsAuditTask =
                this.pdsAuditService.ModifyPdsAuditAsync(invalidPdsAudit);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(
                    modifyPdsAuditTask.AsTask);

            // then
            actualPdsAuditValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPdsAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(invalidPdsAudit.Id),
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
            PdsAudit randomPdsAudit = CreateRandomPdsAudit(randomDateTimeOffset);
            randomPdsAudit.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var invalidPdsAuditException =
                new InvalidPdsAuditException();

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.UpdatedDate),
                values: "Date is not recent");

            var expectedPdsAuditValidatonException =
                new PdsAuditValidationException(invalidPdsAuditException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<PdsAudit> modifyPdsAuditTask =
                this.pdsAuditService.ModifyPdsAuditAsync(randomPdsAudit);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(
                    modifyPdsAuditTask.AsTask);

            // then
            actualPdsAuditValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPdsAuditValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfPdsAuditDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            PdsAudit randomPdsAudit = CreateRandomModifyPdsAudit(randomDateTimeOffset);
            PdsAudit nonExistPdsAudit = randomPdsAudit;
            PdsAudit nullPdsAudit = null;

            var notFoundPdsAuditException =
                new NotFoundPdsAuditException(nonExistPdsAudit.Id);

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(notFoundPdsAuditException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPdsAuditByIdAsync(nonExistPdsAudit.Id))
                .ReturnsAsync(nullPdsAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when 
            ValueTask<PdsAudit> modifyPdsAuditTask =
                this.pdsAuditService.ModifyPdsAuditAsync(nonExistPdsAudit);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(
                    modifyPdsAuditTask.AsTask);

            // then
            actualPdsAuditValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(nonExistPdsAudit.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPdsAuditValidationException))),
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
            PdsAudit randomPdsAudit = CreateRandomModifyPdsAudit(randomDateTimeOffset);
            PdsAudit invalidPdsAudit = randomPdsAudit.DeepClone();
            PdsAudit storagePdsAudit = invalidPdsAudit.DeepClone();
            storagePdsAudit.CreatedDate = storagePdsAudit.CreatedDate.AddMinutes(randomMinutes);
            storagePdsAudit.UpdatedDate = storagePdsAudit.UpdatedDate.AddMinutes(randomMinutes);
            var invalidPdsAuditException = new InvalidPdsAuditException();

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.CreatedDate),
                values: $"Date is not the same as {nameof(PdsAudit.CreatedDate)}");

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(invalidPdsAuditException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPdsAuditByIdAsync(invalidPdsAudit.Id))
                .ReturnsAsync(storagePdsAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<PdsAudit> modifyPdsAuditTask =
                this.pdsAuditService.ModifyPdsAuditAsync(invalidPdsAudit);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(
                    modifyPdsAuditTask.AsTask);

            // then
            actualPdsAuditValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(invalidPdsAudit.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedPdsAuditValidationException))),
                       Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCreatedUserIdDontMacthStorageAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            PdsAudit randomPdsAudit = CreateRandomModifyPdsAudit(randomDateTimeOffset);
            PdsAudit invalidPdsAudit = randomPdsAudit.DeepClone();
            PdsAudit storagePdsAudit = invalidPdsAudit.DeepClone();
            invalidPdsAudit.CreatedByUserId = Guid.NewGuid();
            storagePdsAudit.UpdatedDate = storagePdsAudit.CreatedDate;

            var invalidPdsAuditException = new InvalidPdsAuditException();

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.CreatedByUserId),
                values: $"Id is not the same as {nameof(PdsAudit.CreatedByUserId)}");

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(invalidPdsAuditException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPdsAuditByIdAsync(invalidPdsAudit.Id))
                .ReturnsAsync(storagePdsAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<PdsAudit> modifyPdsAuditTask =
                this.pdsAuditService.ModifyPdsAuditAsync(invalidPdsAudit);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(
                    modifyPdsAuditTask.AsTask);

            // then
            actualPdsAuditValidationException.Should().BeEquivalentTo(expectedPdsAuditValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(invalidPdsAudit.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedPdsAuditValidationException))),
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
            PdsAudit randomPdsAudit = CreateRandomModifyPdsAudit(randomDateTimeOffset);
            PdsAudit invalidPdsAudit = randomPdsAudit;
            PdsAudit storagePdsAudit = randomPdsAudit.DeepClone();

            var invalidPdsAuditException = new InvalidPdsAuditException();

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.UpdatedDate),
                values: $"Date is the same as {nameof(PdsAudit.UpdatedDate)}");

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(invalidPdsAuditException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPdsAuditByIdAsync(invalidPdsAudit.Id))
                .ReturnsAsync(storagePdsAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<PdsAudit> modifyPdsAuditTask =
                this.pdsAuditService.ModifyPdsAuditAsync(invalidPdsAudit);

            // then
            await Assert.ThrowsAsync<PdsAuditValidationException>(
                modifyPdsAuditTask.AsTask);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPdsAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(invalidPdsAudit.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}