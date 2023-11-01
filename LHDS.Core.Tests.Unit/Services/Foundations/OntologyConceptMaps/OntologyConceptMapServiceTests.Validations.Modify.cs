using System;
using System.Threading.Tasks;
using FluentAssertions;
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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
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
            actualOntologyConceptMapValidationException.Should().BeEquivalentTo(expectedOntologyConceptMapValidatonException);

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
    }
}