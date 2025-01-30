// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ObjectColumns;
using LHDS.Core.Models.Processings.ObjectColumns.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.ObjectColumns
{
    public partial class ObjectColumnProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionsOnModifyOrAddIfObjectColumnProcessingIsNullAndLogItAsync()
        {
            // given
            ObjectColumn nullObjectColumn = null;

            var nullObjectColumnProcessingException =
                new NullObjectColumnProcessingException(message: "ObjectColumn is null.");

            var expectedObjectColumnProcessingValidationException =
                new ObjectColumnProcessingValidationException(
                    message: "ObjectColumn processing validation error occurred, please try again.",
                    innerException: nullObjectColumnProcessingException);

            // when
            ValueTask<ObjectColumn> AddObjectColumnTask =
                this.objectColumnProcessingService.ModifyOrAddObjectColumnAsync(nullObjectColumn);

            ObjectColumnProcessingValidationException actualObjectColumnProcessingValidationException =
                await Assert.ThrowsAsync<ObjectColumnProcessingValidationException>(AddObjectColumnTask.AsTask);

            //then
            actualObjectColumnProcessingValidationException.Should()
                .BeEquivalentTo(expectedObjectColumnProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedObjectColumnProcessingValidationException))),
                        Times.Once);

            this.objectColumnServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionsOnModifyOrAddIfIdIsInvalidAndLogItAsync()
        {
            // given
            ObjectColumn invalidObjectColumn = new ObjectColumn();

            var invalidArgumentObjectColumnProcessingException =
                new InvalidArgumentObjectColumnProcessingException(
                    message: "Invalid argument(s). Please correct the errors and try again.");

            invalidArgumentObjectColumnProcessingException.AddData(
                key: "Id",
                values: "Id is required");

            var expectedObjectColumnProcessingValidationException =
                new ObjectColumnProcessingValidationException(
                    message: "ObjectColumn processing validation error occurred, please try again.",
                    innerException: invalidArgumentObjectColumnProcessingException);

            // when
            ValueTask<ObjectColumn> RetrieveObjectColumnTask =
                this.objectColumnProcessingService.ModifyOrAddObjectColumnAsync(invalidObjectColumn);

            ObjectColumnProcessingValidationException actualObjectColumnProcessingValidationException =
                await Assert.ThrowsAsync<ObjectColumnProcessingValidationException>(RetrieveObjectColumnTask.AsTask);

            //then
            actualObjectColumnProcessingValidationException.Should()
                .BeEquivalentTo(expectedObjectColumnProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedObjectColumnProcessingValidationException))),
                        Times.Once);

            this.objectColumnServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
