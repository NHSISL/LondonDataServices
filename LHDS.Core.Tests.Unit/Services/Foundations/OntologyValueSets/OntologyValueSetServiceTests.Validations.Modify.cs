using System;
using System.Threading.Tasks;
using FluentAssertions;
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
            actualOntologyValueSetValidationException.Should().BeEquivalentTo(expectedOntologyValueSetValidationException);

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

            // when
            ValueTask<OntologyValueSet> modifyOntologyValueSetTask =
                this.ontologyValueSetService.ModifyOntologyValueSetAsync(invalidOntologyValueSet);

            OntologyValueSetValidationException actualOntologyValueSetValidationException =
                await Assert.ThrowsAsync<OntologyValueSetValidationException>(
                    modifyOntologyValueSetTask.AsTask);

            // then
            actualOntologyValueSetValidationException.Should().BeEquivalentTo(expectedOntologyValueSetValidationException);

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
    }
}