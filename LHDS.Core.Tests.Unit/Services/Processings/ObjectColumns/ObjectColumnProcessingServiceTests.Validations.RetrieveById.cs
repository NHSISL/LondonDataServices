// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public async Task ShouldThrowValidationExceptionsOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidId = Guid.Empty;

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
                this.objectColumnProcessingService.RetrieveObjectColumnByIdAsync(invalidId);

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
