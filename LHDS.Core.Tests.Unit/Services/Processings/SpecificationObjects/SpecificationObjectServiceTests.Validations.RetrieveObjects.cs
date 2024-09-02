// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Processings.SpecificationObjects.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.SpecificationObjects
{
    public partial class SpecificationObjectProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionsOnRetrieveObjectsByDataSetSpecificationIdAndLogItAsync()
        {
            // given
            Guid invalidId = Guid.Empty;

            var invalidArgumentSpecificationObjectProcessingException =
                new InvalidArgumentSpecificationObjectProcessingException(
                    message: "Invalid argument(s). Please correct the errors and try again.");

            invalidArgumentSpecificationObjectProcessingException.AddData(
                key: "Id",
                values: "Id is required");

            var expectedSpecificationObjectProcessingValidationException =
                new SpecificationObjectProcessingValidationException(
                    message: "SpecificationObject processing validation error occurred, please try again.",
                    innerException: invalidArgumentSpecificationObjectProcessingException);

            // when
            ValueTask<List<string>> RetrieveSpecificationObjectTask =
                this.specificationObjectProcessingService
                    .RetrieveSpecificationObjectsByDataSetSpecificationId(invalidId);

            SpecificationObjectProcessingValidationException actualSpecificationObjectProcessingValidationException =
                await Assert.ThrowsAsync<SpecificationObjectProcessingValidationException>(
                    RetrieveSpecificationObjectTask.AsTask);

            //then
            actualSpecificationObjectProcessingValidationException.Should()
                .BeEquivalentTo(expectedSpecificationObjectProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSpecificationObjectProcessingValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.specificationObjectServiceMock.VerifyNoOtherCalls();
        }
    }
}
