using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.OntologyCodeSystems;
using LHDS.Core.Models.Foundations.OntologyCodeSystems.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OntologyCodeSystems
{
    public partial class OntologyCodeSystemServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfOntologyCodeSystemIsNullAndLogItAsync()
        {
            // given
            OntologyCodeSystem nullOntologyCodeSystem = null;
            var nullOntologyCodeSystemException = new NullOntologyCodeSystemException(message: "OntologyCodeSystem is null.");

            var expectedOntologyCodeSystemValidationException =
                new OntologyCodeSystemValidationException(
                    message: "OntologyCodeSystem validation errors occurred, please try again.",
                    innerException: nullOntologyCodeSystemException);

            // when
            ValueTask<OntologyCodeSystem> modifyOntologyCodeSystemTask =
                this.ontologyCodeSystemService.ModifyOntologyCodeSystemAsync(nullOntologyCodeSystem);

            OntologyCodeSystemValidationException actualOntologyCodeSystemValidationException =
                await Assert.ThrowsAsync<OntologyCodeSystemValidationException>(
                    modifyOntologyCodeSystemTask.AsTask);

            // then
            actualOntologyCodeSystemValidationException.Should()
                .BeEquivalentTo(expectedOntologyCodeSystemValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyCodeSystemValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOntologyCodeSystemAsync(It.IsAny<OntologyCodeSystem>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfOntologyCodeSystemIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            var invalidOntologyCodeSystem = new OntologyCodeSystem
            {
                // TODO:  Add default values for your properties i.e. Name = invalidText
            };

            var invalidOntologyCodeSystemException = 
                new InvalidOntologyCodeSystemException(
                    message: "Invalid ontologyCodeSystem. Please correct the errors and try again.");

            invalidOntologyCodeSystemException.AddData(
                key: nameof(OntologyCodeSystem.Id),
                values: "Id is required");

            //invalidOntologyCodeSystemException.AddData(
            //    key: nameof(OntologyCodeSystem.Name),
            //    values: "Text is required");

            // TODO: Add or remove data here to suit the validation needs for the OntologyCodeSystem model

            invalidOntologyCodeSystemException.AddData(
                key: nameof(OntologyCodeSystem.CreatedDate),
                values: "Date is required");

            invalidOntologyCodeSystemException.AddData(
                key: nameof(OntologyCodeSystem.CreatedBy),
                values: "Text is required");

            invalidOntologyCodeSystemException.AddData(
                key: nameof(OntologyCodeSystem.UpdatedDate),
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(OntologyCodeSystem.CreatedDate)}"
                });

            invalidOntologyCodeSystemException.AddData(
                key: nameof(OntologyCodeSystem.UpdatedBy),
                values: "Text is required");

            var expectedOntologyCodeSystemValidationException =
                new OntologyCodeSystemValidationException(
                    message: "OntologyCodeSystem validation errors occurred, please try again.",
                    innerException: invalidOntologyCodeSystemException);

            // when
            ValueTask<OntologyCodeSystem> modifyOntologyCodeSystemTask =
                this.ontologyCodeSystemService.ModifyOntologyCodeSystemAsync(invalidOntologyCodeSystem);

            OntologyCodeSystemValidationException actualOntologyCodeSystemValidationException =
                await Assert.ThrowsAsync<OntologyCodeSystemValidationException>(
                    modifyOntologyCodeSystemTask.AsTask);

            //then
            actualOntologyCodeSystemValidationException.Should()
                .BeEquivalentTo(expectedOntologyCodeSystemValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyCodeSystemValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOntologyCodeSystemAsync(It.IsAny<OntologyCodeSystem>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            OntologyCodeSystem randomOntologyCodeSystem = CreateRandomOntologyCodeSystem(randomDateTimeOffset);
            OntologyCodeSystem invalidOntologyCodeSystem = randomOntologyCodeSystem;

            var invalidOntologyCodeSystemException = 
                new InvalidOntologyCodeSystemException(
                    message: "Invalid ontologyCodeSystem. Please correct the errors and try again.");

            invalidOntologyCodeSystemException.AddData(
                key: nameof(OntologyCodeSystem.UpdatedDate),
                values: $"Date is the same as {nameof(OntologyCodeSystem.CreatedDate)}");

            var expectedOntologyCodeSystemValidationException =
                new OntologyCodeSystemValidationException(
                    message: "OntologyCodeSystem validation errors occurred, please try again.",
                    innerException: invalidOntologyCodeSystemException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<OntologyCodeSystem> modifyOntologyCodeSystemTask =
                this.ontologyCodeSystemService.ModifyOntologyCodeSystemAsync(invalidOntologyCodeSystem);

            OntologyCodeSystemValidationException actualOntologyCodeSystemValidationException =
                await Assert.ThrowsAsync<OntologyCodeSystemValidationException>(
                    modifyOntologyCodeSystemTask.AsTask);

            // then
            actualOntologyCodeSystemValidationException.Should()
                .BeEquivalentTo(expectedOntologyCodeSystemValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyCodeSystemValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyCodeSystemByIdAsync(invalidOntologyCodeSystem.Id),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(int minutes)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            OntologyCodeSystem randomOntologyCodeSystem = CreateRandomOntologyCodeSystem(randomDateTimeOffset);
            randomOntologyCodeSystem.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var invalidOntologyCodeSystemException = 
                new InvalidOntologyCodeSystemException(
                    message: "Invalid ontologyCodeSystem. Please correct the errors and try again.");

            invalidOntologyCodeSystemException.AddData(
                key: nameof(OntologyCodeSystem.UpdatedDate),
                values: "Date is not recent");

            var expectedOntologyCodeSystemValidatonException =
                new OntologyCodeSystemValidationException(
                    message: "OntologyCodeSystem validation errors occurred, please try again.",
                    innerException: invalidOntologyCodeSystemException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<OntologyCodeSystem> modifyOntologyCodeSystemTask =
                this.ontologyCodeSystemService.ModifyOntologyCodeSystemAsync(randomOntologyCodeSystem);

            OntologyCodeSystemValidationException actualOntologyCodeSystemValidationException =
                await Assert.ThrowsAsync<OntologyCodeSystemValidationException>(
                    modifyOntologyCodeSystemTask.AsTask);

            // then
            actualOntologyCodeSystemValidationException.Should()
                .BeEquivalentTo(expectedOntologyCodeSystemValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyCodeSystemValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyCodeSystemByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfOntologyCodeSystemDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            OntologyCodeSystem randomOntologyCodeSystem = CreateRandomModifyOntologyCodeSystem(randomDateTimeOffset);
            OntologyCodeSystem nonExistOntologyCodeSystem = randomOntologyCodeSystem;
            OntologyCodeSystem nullOntologyCodeSystem = null;

            var notFoundOntologyCodeSystemException =
                new NotFoundOntologyCodeSystemException(nonExistOntologyCodeSystem.Id);

            var expectedOntologyCodeSystemValidationException =
                new OntologyCodeSystemValidationException(
                    message: "OntologyCodeSystem validation errors occurred, please try again.",
                    innerException: notFoundOntologyCodeSystemException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOntologyCodeSystemByIdAsync(nonExistOntologyCodeSystem.Id))
                .ReturnsAsync(nullOntologyCodeSystem);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when 
            ValueTask<OntologyCodeSystem> modifyOntologyCodeSystemTask =
                this.ontologyCodeSystemService.ModifyOntologyCodeSystemAsync(nonExistOntologyCodeSystem);

            OntologyCodeSystemValidationException actualOntologyCodeSystemValidationException =
                await Assert.ThrowsAsync<OntologyCodeSystemValidationException>(
                    modifyOntologyCodeSystemTask.AsTask);

            // then
            actualOntologyCodeSystemValidationException.Should()
                .BeEquivalentTo(expectedOntologyCodeSystemValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyCodeSystemByIdAsync(nonExistOntologyCodeSystem.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyCodeSystemValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}