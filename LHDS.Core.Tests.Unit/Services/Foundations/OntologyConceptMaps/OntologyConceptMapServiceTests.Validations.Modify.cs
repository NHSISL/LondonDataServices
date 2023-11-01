using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Core.Models.Foundations.OntologyConceptMaps;
using LHDS.Core.Models.Foundations.OntologyConceptMaps.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OntologyConceptMaps
{
    public partial class OntologyConceptMapServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfOntologyConceptMapIsNullAndLogItAsync()
        {
            // given
            OntologyConceptMap nullOntologyConceptMap = null;
            var nullOntologyConceptMapException = new NullOntologyConceptMapException(message: "OntologyConceptMap is null.");

            var expectedOntologyConceptMapValidationException =
                new OntologyConceptMapValidationException(
                    message: "OntologyConceptMap validation errors occurred, please try again.",
                    innerException: nullOntologyConceptMapException);

            // when
            ValueTask<OntologyConceptMap> modifyOntologyConceptMapTask =
                this.ontologyConceptMapService.ModifyOntologyConceptMapAsync(nullOntologyConceptMap);

            OntologyConceptMapValidationException actualOntologyConceptMapValidationException =
                await Assert.ThrowsAsync<OntologyConceptMapValidationException>(
                    modifyOntologyConceptMapTask.AsTask);

            // then
            actualOntologyConceptMapValidationException.Should()
                .BeEquivalentTo(expectedOntologyConceptMapValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyConceptMapValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOntologyConceptMapAsync(It.IsAny<OntologyConceptMap>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfOntologyConceptMapIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            var invalidOntologyConceptMap = new OntologyConceptMap
            {
                // TODO:  Add default values for your properties i.e. Name = invalidText
            };

            var invalidOntologyConceptMapException = 
                new InvalidOntologyConceptMapException(
                    message: "Invalid ontologyConceptMap. Please correct the errors and try again.");

            invalidOntologyConceptMapException.AddData(
                key: nameof(OntologyConceptMap.Id),
                values: "Id is required");

            //invalidOntologyConceptMapException.AddData(
            //    key: nameof(OntologyConceptMap.Name),
            //    values: "Text is required");

            // TODO: Add or remove data here to suit the validation needs for the OntologyConceptMap model

            invalidOntologyConceptMapException.AddData(
                key: nameof(OntologyConceptMap.CreatedDate),
                values: "Date is required");

            invalidOntologyConceptMapException.AddData(
                key: nameof(OntologyConceptMap.CreatedBy),
                values: "Text is required");

            invalidOntologyConceptMapException.AddData(
                key: nameof(OntologyConceptMap.UpdatedDate),
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(OntologyConceptMap.CreatedDate)}"
                });

            invalidOntologyConceptMapException.AddData(
                key: nameof(OntologyConceptMap.UpdatedBy),
                values: "Text is required");

            var expectedOntologyConceptMapValidationException =
                new OntologyConceptMapValidationException(
                    message: "OntologyConceptMap validation errors occurred, please try again.",
                    innerException: invalidOntologyConceptMapException);

            // when
            ValueTask<OntologyConceptMap> modifyOntologyConceptMapTask =
                this.ontologyConceptMapService.ModifyOntologyConceptMapAsync(invalidOntologyConceptMap);

            OntologyConceptMapValidationException actualOntologyConceptMapValidationException =
                await Assert.ThrowsAsync<OntologyConceptMapValidationException>(
                    modifyOntologyConceptMapTask.AsTask);

            //then
            actualOntologyConceptMapValidationException.Should()
                .BeEquivalentTo(expectedOntologyConceptMapValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyConceptMapValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOntologyConceptMapAsync(It.IsAny<OntologyConceptMap>()),
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
            OntologyConceptMap randomOntologyConceptMap = CreateRandomOntologyConceptMap(randomDateTimeOffset);
            OntologyConceptMap invalidOntologyConceptMap = randomOntologyConceptMap;
            
            var invalidOntologyConceptMapException = 
                new InvalidOntologyConceptMapException(
                    message: "Invalid ontologyConceptMap. Please correct the errors and try again.");

            invalidOntologyConceptMapException.AddData(
                key: nameof(OntologyConceptMap.UpdatedDate),
                values: $"Date is the same as {nameof(OntologyConceptMap.CreatedDate)}");

            var expectedOntologyConceptMapValidationException =
                new OntologyConceptMapValidationException(
                    message: "OntologyConceptMap validation errors occurred, please try again.",
                    innerException: invalidOntologyConceptMapException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<OntologyConceptMap> modifyOntologyConceptMapTask =
                this.ontologyConceptMapService.ModifyOntologyConceptMapAsync(invalidOntologyConceptMap);

            OntologyConceptMapValidationException actualOntologyConceptMapValidationException =
                await Assert.ThrowsAsync<OntologyConceptMapValidationException>(
                    modifyOntologyConceptMapTask.AsTask);

            // then
            actualOntologyConceptMapValidationException.Should()
                .BeEquivalentTo(expectedOntologyConceptMapValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyConceptMapValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyConceptMapByIdAsync(invalidOntologyConceptMap.Id),
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
            OntologyConceptMap randomOntologyConceptMap = CreateRandomOntologyConceptMap(randomDateTimeOffset);
            randomOntologyConceptMap.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var invalidOntologyConceptMapException = 
                new InvalidOntologyConceptMapException(
                    message: "Invalid ontologyConceptMap. Please correct the errors and try again.");

            invalidOntologyConceptMapException.AddData(
                key: nameof(OntologyConceptMap.UpdatedDate),
                values: "Date is not recent");

            var expectedOntologyConceptMapValidatonException =
                new OntologyConceptMapValidationException(
                    message: "OntologyConceptMap validation errors occurred, please try again.",
                    innerException: invalidOntologyConceptMapException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<OntologyConceptMap> modifyOntologyConceptMapTask =
                this.ontologyConceptMapService.ModifyOntologyConceptMapAsync(randomOntologyConceptMap);

            OntologyConceptMapValidationException actualOntologyConceptMapValidationException =
                await Assert.ThrowsAsync<OntologyConceptMapValidationException>(
                    modifyOntologyConceptMapTask.AsTask);

            // then
            actualOntologyConceptMapValidationException.Should()
                .BeEquivalentTo(expectedOntologyConceptMapValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyConceptMapValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyConceptMapByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfOntologyConceptMapDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            OntologyConceptMap randomOntologyConceptMap = CreateRandomModifyOntologyConceptMap(randomDateTimeOffset);
            OntologyConceptMap nonExistOntologyConceptMap = randomOntologyConceptMap;
            OntologyConceptMap nullOntologyConceptMap = null;

            var notFoundOntologyConceptMapException =
                new NotFoundOntologyConceptMapException(nonExistOntologyConceptMap.Id);

            var expectedOntologyConceptMapValidationException =
                new OntologyConceptMapValidationException(
                    message: "OntologyConceptMap validation errors occurred, please try again.",
                    innerException: notFoundOntologyConceptMapException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOntologyConceptMapByIdAsync(nonExistOntologyConceptMap.Id))
                .ReturnsAsync(nullOntologyConceptMap);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when 
            ValueTask<OntologyConceptMap> modifyOntologyConceptMapTask =
                this.ontologyConceptMapService.ModifyOntologyConceptMapAsync(nonExistOntologyConceptMap);

            OntologyConceptMapValidationException actualOntologyConceptMapValidationException =
                await Assert.ThrowsAsync<OntologyConceptMapValidationException>(
                    modifyOntologyConceptMapTask.AsTask);

            // then
            actualOntologyConceptMapValidationException.Should()
                .BeEquivalentTo(expectedOntologyConceptMapValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyConceptMapByIdAsync(nonExistOntologyConceptMap.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyConceptMapValidationException))),
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
            OntologyConceptMap randomOntologyConceptMap = CreateRandomModifyOntologyConceptMap(randomDateTimeOffset);
            OntologyConceptMap invalidOntologyConceptMap = randomOntologyConceptMap.DeepClone();
            OntologyConceptMap storageOntologyConceptMap = invalidOntologyConceptMap.DeepClone();
            storageOntologyConceptMap.CreatedDate = storageOntologyConceptMap.CreatedDate.AddMinutes(randomMinutes);
            storageOntologyConceptMap.UpdatedDate = storageOntologyConceptMap.UpdatedDate.AddMinutes(randomMinutes);
            
            var invalidOntologyConceptMapException = 
                new InvalidOntologyConceptMapException(
                    message: "Invalid ontologyConceptMap. Please correct the errors and try again.");

            invalidOntologyConceptMapException.AddData(
                key: nameof(OntologyConceptMap.CreatedDate),
                values: $"Date is not the same as {nameof(OntologyConceptMap.CreatedDate)}");

            var expectedOntologyConceptMapValidationException =
                new OntologyConceptMapValidationException(
                    message: "OntologyConceptMap validation errors occurred, please try again.",
                    innerException: invalidOntologyConceptMapException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOntologyConceptMapByIdAsync(invalidOntologyConceptMap.Id))
                .ReturnsAsync(storageOntologyConceptMap);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<OntologyConceptMap> modifyOntologyConceptMapTask =
                this.ontologyConceptMapService.ModifyOntologyConceptMapAsync(invalidOntologyConceptMap);

            OntologyConceptMapValidationException actualOntologyConceptMapValidationException =
                await Assert.ThrowsAsync<OntologyConceptMapValidationException>(
                    modifyOntologyConceptMapTask.AsTask);

            // then
            actualOntologyConceptMapValidationException.Should()
                .BeEquivalentTo(expectedOntologyConceptMapValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyConceptMapByIdAsync(invalidOntologyConceptMap.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedOntologyConceptMapValidationException))),
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
            OntologyConceptMap randomOntologyConceptMap = CreateRandomModifyOntologyConceptMap(randomDateTimeOffset);
            OntologyConceptMap invalidOntologyConceptMap = randomOntologyConceptMap.DeepClone();
            OntologyConceptMap storageOntologyConceptMap = invalidOntologyConceptMap.DeepClone();
            invalidOntologyConceptMap.CreatedBy = Guid.NewGuid().ToString();
            storageOntologyConceptMap.UpdatedDate = storageOntologyConceptMap.CreatedDate;

            var invalidOntologyConceptMapException = 
                new InvalidOntologyConceptMapException(
                    message: "Invalid ontologyConceptMap. Please correct the errors and try again.");

            invalidOntologyConceptMapException.AddData(
                key: nameof(OntologyConceptMap.CreatedBy),
                values: $"Text is not the same as {nameof(OntologyConceptMap.CreatedBy)}");

            var expectedOntologyConceptMapValidationException =
                new OntologyConceptMapValidationException(
                    message: "OntologyConceptMap validation errors occurred, please try again.",
                    innerException: invalidOntologyConceptMapException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOntologyConceptMapByIdAsync(invalidOntologyConceptMap.Id))
                .ReturnsAsync(storageOntologyConceptMap);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<OntologyConceptMap> modifyOntologyConceptMapTask =
                this.ontologyConceptMapService.ModifyOntologyConceptMapAsync(invalidOntologyConceptMap);

            OntologyConceptMapValidationException actualOntologyConceptMapValidationException =
                await Assert.ThrowsAsync<OntologyConceptMapValidationException>(
                    modifyOntologyConceptMapTask.AsTask);

            // then
            actualOntologyConceptMapValidationException.Should().BeEquivalentTo(expectedOntologyConceptMapValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyConceptMapByIdAsync(invalidOntologyConceptMap.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedOntologyConceptMapValidationException))),
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
            OntologyConceptMap randomOntologyConceptMap = CreateRandomModifyOntologyConceptMap(randomDateTimeOffset);
            OntologyConceptMap invalidOntologyConceptMap = randomOntologyConceptMap;
            OntologyConceptMap storageOntologyConceptMap = randomOntologyConceptMap.DeepClone();

            var invalidOntologyConceptMapException = 
                new InvalidOntologyConceptMapException(
                    message: "Invalid ontologyConceptMap. Please correct the errors and try again.");

            invalidOntologyConceptMapException.AddData(
                key: nameof(OntologyConceptMap.UpdatedDate),
                values: $"Date is the same as {nameof(OntologyConceptMap.UpdatedDate)}");

            var expectedOntologyConceptMapValidationException =
                new OntologyConceptMapValidationException(
                    message: "OntologyConceptMap validation errors occurred, please try again.",
                    innerException: invalidOntologyConceptMapException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOntologyConceptMapByIdAsync(invalidOntologyConceptMap.Id))
                .ReturnsAsync(storageOntologyConceptMap);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<OntologyConceptMap> modifyOntologyConceptMapTask =
                this.ontologyConceptMapService.ModifyOntologyConceptMapAsync(invalidOntologyConceptMap);

            // then
            await Assert.ThrowsAsync<OntologyConceptMapValidationException>(
                modifyOntologyConceptMapTask.AsTask);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyConceptMapValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyConceptMapByIdAsync(invalidOntologyConceptMap.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}