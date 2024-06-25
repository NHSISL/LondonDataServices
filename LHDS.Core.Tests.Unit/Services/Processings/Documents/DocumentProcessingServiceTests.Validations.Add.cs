// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Processings.Documents.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Documents
{
    public partial class DocumentProcessingServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionsOnAddIfDocumentProcessingIsNullAndLogItAsync(
            string invalidInput)
        {
            // given
            Stream invalidStream = null;
            string invalidFileName = invalidInput;
            string invalidContainer = invalidInput;

            var invalidArgumentsDocumentProcessingException =
                new InvalidArgumentsDocumentProcessingException(
                    message: "Invalid document processing arguments. Please correct the errors and try again.");

            invalidArgumentsDocumentProcessingException.AddData(
                key: "Output",
                values: "Stream is required");

            invalidArgumentsDocumentProcessingException.AddData(
                key: "FileName",
                values: "Text is required");

            invalidArgumentsDocumentProcessingException.AddData(
                key: "Container",
                values: "Text is required");

            var expectedDocumentProcessingValidationException =
                new DocumentProcessingValidationException(
                    message: "Document processing validation errors occured, please try again",
                    innerException: invalidArgumentsDocumentProcessingException);

            // when
            ValueTask addDocumentTask =
                this.documentProcessingService.AddDocumentAsync(
                    input: invalidStream,
                    fileName: invalidFileName,
                    container: invalidContainer);

            DocumentProcessingValidationException actualDocumentProcessingValidationException =
                await Assert.ThrowsAsync<DocumentProcessingValidationException>(addDocumentTask.AsTask);

            //then
            actualDocumentProcessingValidationException.Should()
                .BeEquivalentTo(expectedDocumentProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDocumentProcessingValidationException))),
                        Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
