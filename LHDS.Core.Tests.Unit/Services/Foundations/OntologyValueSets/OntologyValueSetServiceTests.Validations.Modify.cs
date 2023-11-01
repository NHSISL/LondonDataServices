using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Core.Models.Foundations.OntologyValueSets;
using LHDS.Core.Models.Foundations.OntologyValueSets.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OntologyValueSets
{
    public partial class OntologyValueSetServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfOntologyValueSetIsNullAndLogItAsync()
        {
            // given
            OntologyValueSet nullOntologyValueSet = null;
            var nullOntologyValueSetException = new NullOntologyValueSetException(message: "OntologyValueSet is null.");

            var expectedOntologyValueSetValidationException =
                new OntologyValueSetValidationException(
                    message: "OntologyValueSet validation errors occurred, please try again.",
                    innerException: nullOntologyValueSetException);

            // when
            ValueTask<OntologyValueSet> modifyOntologyValueSetTask =
                this.ontologyValueSetService.ModifyOntologyValueSetAsync(nullOntologyValueSet);

            OntologyValueSetValidationException actualOntologyValueSetValidationException =
                await Assert.ThrowsAsync<OntologyValueSetValidationException>(
                    modifyOntologyValueSetTask.AsTask);

            // then
            actualOntologyValueSetValidationException.Should()
                .BeEquivalentTo(expectedOntologyValueSetValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyValueSetValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOntologyValueSetAsync(It.IsAny<OntologyValueSet>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfOntologyValueSetIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            var invalidOntologyValueSet = new OntologyValueSet
            {
                // TODO:  Add default values for your properties i.e. Name = invalidText
            };

            var invalidOntologyValueSetException = 
                new InvalidOntologyValueSetException(
                    message: "Invalid ontologyValueSet. Please correct the errors and try again.");

            invalidOntologyValueSetException.AddData(
                key: nameof(OntologyValueSet.Id),
                values: "Id is required");

            //invalidOntologyValueSetException.AddData(
            //    key: nameof(OntologyValueSet.Name),
            //    values: "Text is required");

            // TODO: Add or remove data here to suit the validation needs for the OntologyValueSet model

            invalidOntologyValueSetException.AddData(
                key: nameof(OntologyValueSet.CreatedDate),
                values: "Date is required");

            invalidOntologyValueSetException.AddData(
                key: nameof(OntologyValueSet.CreatedBy),
                values: "Text is required");

            invalidOntologyValueSetException.AddData(
                key: nameof(OntologyValueSet.UpdatedDate),
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(OntologyValueSet.CreatedDate)}"
                });

            invalidOntologyValueSetException.AddData(
                key: nameof(OntologyValueSet.UpdatedBy),
                values: "Text is required");

            var expectedOntologyValueSetValidationException =
                new OntologyValueSetValidationException(
                    message: "OntologyValueSet validation errors occurred, please try again.",
                    innerException: invalidOntologyValueSetException);

            // when
            ValueTask<OntologyValueSet> modifyOntologyValueSetTask =
                this.ontologyValueSetService.ModifyOntologyValueSetAsync(invalidOntologyValueSet);

            OntologyValueSetValidationException actualOntologyValueSetValidationException =
                await Assert.ThrowsAsync<OntologyValueSetValidationException>(
                    modifyOntologyValueSetTask.AsTask);

            //then
            actualOntologyValueSetValidationException.Should()
                .BeEquivalentTo(expectedOntologyValueSetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyValueSetValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOntologyValueSetAsync(It.IsAny<OntologyValueSet>()),
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
            OntologyValueSet randomOntologyValueSet = CreateRandomOntologyValueSet(randomDateTimeOffset);
            OntologyValueSet invalidOntologyValueSet = randomOntologyValueSet;
            
            var invalidOntologyValueSetException = 
                new InvalidOntologyValueSetException(
                    message: "Invalid ontologyValueSet. Please correct the errors and try again.");

            invalidOntologyValueSetException.AddData(
                key: nameof(OntologyValueSet.UpdatedDate),
                values: $"Date is the same as {nameof(OntologyValueSet.CreatedDate)}");

            var expectedOntologyValueSetValidationException =
                new OntologyValueSetValidationException(
                    message: "OntologyValueSet validation errors occurred, please try again.",
                    innerException: invalidOntologyValueSetException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<OntologyValueSet> modifyOntologyValueSetTask =
                this.ontologyValueSetService.ModifyOntologyValueSetAsync(invalidOntologyValueSet);

            OntologyValueSetValidationException actualOntologyValueSetValidationException =
                await Assert.ThrowsAsync<OntologyValueSetValidationException>(
                    modifyOntologyValueSetTask.AsTask);

            // then
            actualOntologyValueSetValidationException.Should()
                .BeEquivalentTo(expectedOntologyValueSetValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyValueSetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyValueSetByIdAsync(invalidOntologyValueSet.Id),
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
            OntologyValueSet randomOntologyValueSet = CreateRandomOntologyValueSet(randomDateTimeOffset);
            randomOntologyValueSet.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var invalidOntologyValueSetException = 
                new InvalidOntologyValueSetException(
                    message: "Invalid ontologyValueSet. Please correct the errors and try again.");

            invalidOntologyValueSetException.AddData(
                key: nameof(OntologyValueSet.UpdatedDate),
                values: "Date is not recent");

            var expectedOntologyValueSetValidatonException =
                new OntologyValueSetValidationException(
                    message: "OntologyValueSet validation errors occurred, please try again.",
                    innerException: invalidOntologyValueSetException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<OntologyValueSet> modifyOntologyValueSetTask =
                this.ontologyValueSetService.ModifyOntologyValueSetAsync(randomOntologyValueSet);

            OntologyValueSetValidationException actualOntologyValueSetValidationException =
                await Assert.ThrowsAsync<OntologyValueSetValidationException>(
                    modifyOntologyValueSetTask.AsTask);

            // then
            actualOntologyValueSetValidationException.Should()
                .BeEquivalentTo(expectedOntologyValueSetValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyValueSetValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyValueSetByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfOntologyValueSetDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            OntologyValueSet randomOntologyValueSet = CreateRandomModifyOntologyValueSet(randomDateTimeOffset);
            OntologyValueSet nonExistOntologyValueSet = randomOntologyValueSet;
            OntologyValueSet nullOntologyValueSet = null;

            var notFoundOntologyValueSetException =
                new NotFoundOntologyValueSetException(nonExistOntologyValueSet.Id);

            var expectedOntologyValueSetValidationException =
                new OntologyValueSetValidationException(
                    message: "OntologyValueSet validation errors occurred, please try again.",
                    innerException: notFoundOntologyValueSetException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOntologyValueSetByIdAsync(nonExistOntologyValueSet.Id))
                .ReturnsAsync(nullOntologyValueSet);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when 
            ValueTask<OntologyValueSet> modifyOntologyValueSetTask =
                this.ontologyValueSetService.ModifyOntologyValueSetAsync(nonExistOntologyValueSet);

            OntologyValueSetValidationException actualOntologyValueSetValidationException =
                await Assert.ThrowsAsync<OntologyValueSetValidationException>(
                    modifyOntologyValueSetTask.AsTask);

            // then
            actualOntologyValueSetValidationException.Should()
                .BeEquivalentTo(expectedOntologyValueSetValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyValueSetByIdAsync(nonExistOntologyValueSet.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyValueSetValidationException))),
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
            OntologyValueSet randomOntologyValueSet = CreateRandomModifyOntologyValueSet(randomDateTimeOffset);
            OntologyValueSet invalidOntologyValueSet = randomOntologyValueSet.DeepClone();
            OntologyValueSet storageOntologyValueSet = invalidOntologyValueSet.DeepClone();
            storageOntologyValueSet.CreatedDate = storageOntologyValueSet.CreatedDate.AddMinutes(randomMinutes);
            storageOntologyValueSet.UpdatedDate = storageOntologyValueSet.UpdatedDate.AddMinutes(randomMinutes);
            
            var invalidOntologyValueSetException = 
                new InvalidOntologyValueSetException(
                    message: "Invalid ontologyValueSet. Please correct the errors and try again.");

            invalidOntologyValueSetException.AddData(
                key: nameof(OntologyValueSet.CreatedDate),
                values: $"Date is not the same as {nameof(OntologyValueSet.CreatedDate)}");

            var expectedOntologyValueSetValidationException =
                new OntologyValueSetValidationException(
                    message: "OntologyValueSet validation errors occurred, please try again.",
                    innerException: invalidOntologyValueSetException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOntologyValueSetByIdAsync(invalidOntologyValueSet.Id))
                .ReturnsAsync(storageOntologyValueSet);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<OntologyValueSet> modifyOntologyValueSetTask =
                this.ontologyValueSetService.ModifyOntologyValueSetAsync(invalidOntologyValueSet);

            OntologyValueSetValidationException actualOntologyValueSetValidationException =
                await Assert.ThrowsAsync<OntologyValueSetValidationException>(
                    modifyOntologyValueSetTask.AsTask);

            // then
            actualOntologyValueSetValidationException.Should()
                .BeEquivalentTo(expectedOntologyValueSetValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyValueSetByIdAsync(invalidOntologyValueSet.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedOntologyValueSetValidationException))),
                       Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCreatedUserDontMacthStorageAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            OntologyValueSet randomOntologyValueSet = CreateRandomModifyOntologyValueSet(randomDateTimeOffset);
            OntologyValueSet invalidOntologyValueSet = randomOntologyValueSet.DeepClone();
            OntologyValueSet storageOntologyValueSet = invalidOntologyValueSet.DeepClone();
            invalidOntologyValueSet.CreatedBy = Guid.NewGuid().ToString();
            storageOntologyValueSet.UpdatedDate = storageOntologyValueSet.CreatedDate;

            var invalidOntologyValueSetException = 
                new InvalidOntologyValueSetException(
                    message: "Invalid ontologyValueSet. Please correct the errors and try again.");

            invalidOntologyValueSetException.AddData(
                key: nameof(OntologyValueSet.CreatedBy),
                values: $"Text is not the same as {nameof(OntologyValueSet.CreatedBy)}");

            var expectedOntologyValueSetValidationException =
                new OntologyValueSetValidationException(
                    message: "OntologyValueSet validation errors occurred, please try again.",
                    innerException: invalidOntologyValueSetException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOntologyValueSetByIdAsync(invalidOntologyValueSet.Id))
                .ReturnsAsync(storageOntologyValueSet);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<OntologyValueSet> modifyOntologyValueSetTask =
                this.ontologyValueSetService.ModifyOntologyValueSetAsync(invalidOntologyValueSet);

            OntologyValueSetValidationException actualOntologyValueSetValidationException =
                await Assert.ThrowsAsync<OntologyValueSetValidationException>(
                    modifyOntologyValueSetTask.AsTask);

            // then
            actualOntologyValueSetValidationException.Should().BeEquivalentTo(expectedOntologyValueSetValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyValueSetByIdAsync(invalidOntologyValueSet.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedOntologyValueSetValidationException))),
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
            OntologyValueSet randomOntologyValueSet = CreateRandomModifyOntologyValueSet(randomDateTimeOffset);
            OntologyValueSet invalidOntologyValueSet = randomOntologyValueSet;
            OntologyValueSet storageOntologyValueSet = randomOntologyValueSet.DeepClone();

            var invalidOntologyValueSetException = 
                new InvalidOntologyValueSetException(
                    message: "Invalid ontologyValueSet. Please correct the errors and try again.");

            invalidOntologyValueSetException.AddData(
                key: nameof(OntologyValueSet.UpdatedDate),
                values: $"Date is the same as {nameof(OntologyValueSet.UpdatedDate)}");

            var expectedOntologyValueSetValidationException =
                new OntologyValueSetValidationException(
                    message: "OntologyValueSet validation errors occurred, please try again.",
                    innerException: invalidOntologyValueSetException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOntologyValueSetByIdAsync(invalidOntologyValueSet.Id))
                .ReturnsAsync(storageOntologyValueSet);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<OntologyValueSet> modifyOntologyValueSetTask =
                this.ontologyValueSetService.ModifyOntologyValueSetAsync(invalidOntologyValueSet);

            // then
            await Assert.ThrowsAsync<OntologyValueSetValidationException>(
                modifyOntologyValueSetTask.AsTask);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyValueSetValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyValueSetByIdAsync(invalidOntologyValueSet.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}