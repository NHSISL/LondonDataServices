using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.PdsAudits;
using LHDS.Core.Models.PdsAudits.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.PdsAudits
{
    public partial class PdsAuditServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfPdsAuditIsNullAndLogItAsync()
        {
            // given
            PdsAudit nullPdsAudit = null;

            var nullPdsAuditException =
                new NullPdsAuditException();

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(nullPdsAuditException);

            // when
            ValueTask<PdsAudit> addPdsAuditTask =
                this.pdsAuditService.AddPdsAuditAsync(nullPdsAudit);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(
                    addPdsAuditTask.AsTask);

            // then
            actualPdsAuditValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPdsAuditValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfPdsAuditIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            var invalidPdsAudit = new PdsAudit
            {
                // TODO:  Add default values for your properties i.e. Name = invalidText
            };

            var invalidPdsAuditException =
                new InvalidPdsAuditException();

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
                values: "Date is required");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.UpdatedByUserId),
                values: "Id is required");

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(invalidPdsAuditException);

            // when
            ValueTask<PdsAudit> addPdsAuditTask =
                this.pdsAuditService.AddPdsAuditAsync(invalidPdsAudit);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(
                    addPdsAuditTask.AsTask);

            // then
            actualPdsAuditValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPdsAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPdsAuditAsync(It.IsAny<PdsAudit>()),
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
            PdsAudit randomPdsAudit = CreateRandomPdsAudit(randomDateTimeOffset);
            PdsAudit invalidPdsAudit = randomPdsAudit;

            invalidPdsAudit.UpdatedDate =
                invalidPdsAudit.CreatedDate.AddDays(randomNumber);

            var invalidPdsAuditException = new InvalidPdsAuditException();

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.UpdatedDate),
                values: $"Date is not the same as {nameof(PdsAudit.CreatedDate)}");

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(invalidPdsAuditException);

            // when
            ValueTask<PdsAudit> addPdsAuditTask =
                this.pdsAuditService.AddPdsAuditAsync(invalidPdsAudit);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(
                    addPdsAuditTask.AsTask);

            // then
            actualPdsAuditValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPdsAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPdsAuditAsync(It.IsAny<PdsAudit>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}