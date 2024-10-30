// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Models.Processings.SpecificationObjects.Exceptions;
using Moq;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Processings.SpecificationObjects
{
    public partial class SpecificationObjectProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionsOnAddIfSpecificationObjectProcessingIsNullAndLogItAsync()
        {
            // given
            SpecificationObject nullSpecificationObject = null;

            var nullSpecificationObjectProcessingException =
                new NullSpecificationObjectProcessingException(message: $"Specification object processing is Null");

            var expectedSpecificationObjectProcessingValidationException =
                new SpecificationObjectProcessingValidationException(
                    message: "SpecificationObject processing validation error occurred, please try again.",
                    innerException: nullSpecificationObjectProcessingException);

            // when
            ValueTask<SpecificationObject> RetrieveOrAddSpecificationObjectTask =
                this.specificationObjectProcessingService.ReadOrInsertSpecificationObjectAsync(nullSpecificationObject);

            SpecificationObjectProcessingValidationException actualSpecificationObjectProcessingValidationException =
                await Assert.ThrowsAsync<SpecificationObjectProcessingValidationException>(
                    RetrieveOrAddSpecificationObjectTask.AsTask);

            //then
            actualSpecificationObjectProcessingValidationException.Should()
                .BeEquivalentTo(expectedSpecificationObjectProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSpecificationObjectProcessingValidationException))),
                        Times.Once);

            this.specificationObjectServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}