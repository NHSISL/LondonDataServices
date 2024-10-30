// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Models.Processings.ObjectColumns.Exceptions;
using Moq;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Processings.ObjectColumns
{
    public partial class ObjectColumnProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionsOnAddIfObjectColumnProcessingIsNullAndLogItAsync()
        {
            // given
            ObjectColumn nullObjectColumn = null;

            var nullObjectColumnProcessingException =
                new NullObjectColumnProcessingException(message: "ObjectColumn object processing is Null");

            var expectedObjectColumnProcessingValidationException =
                new ObjectColumnProcessingValidationException(
                    message: "ObjectColumn processing validation error occurred, please try again.",
                    innerException: nullObjectColumnProcessingException);

            // when
            ValueTask<ObjectColumn> RetrieveOrAddObjectColumnTask =
                this.ObjectColumnProcessingService.ReadOrInsertObjectColumnAsync(nullObjectColumn);

            ObjectColumnProcessingValidationException actualObjectColumnProcessingValidationException =
                await Assert.ThrowsAsync<ObjectColumnProcessingValidationException>(
                    RetrieveOrAddObjectColumnTask.AsTask);

            //then
            actualObjectColumnProcessingValidationException.Should()
                .BeEquivalentTo(expectedObjectColumnProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedObjectColumnProcessingValidationException))),
                        Times.Once);

            this.ObjectColumnServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}