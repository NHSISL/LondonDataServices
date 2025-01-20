// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        public async Task ShouldThrowValidationExceptionsOnGetDownloadLinkIfDocumentProcessingIsNullAndLogItAsync(
            string invalidInput)
        {
            // given
            string invalidContainer = invalidInput;
            string invalidFileName = invalidInput;

            var invalidDocumentProcessingFileNameException =
                new InvalidArgumentsDocumentProcessingException(
                    message: "Invalid document processing arguments. Please correct the errors and try again.");

            invalidDocumentProcessingFileNameException.AddData(
                key: "FileName",
                values: "Text is required");

            invalidDocumentProcessingFileNameException.AddData(
                key: "Container",
                values: "Text is required");

            var expectedDocumentProcessingValidationException =
                new DocumentProcessingValidationException(
                    message: "Document processing validation errors occured, please try again",
                    innerException: invalidDocumentProcessingFileNameException);

            // when
            ValueTask<string> GetDownloadLinkTask =
                this.documentProcessingService
                    .GetDownloadLinkAsync(fileName: invalidFileName, container: invalidContainer);

            DocumentProcessingValidationException actualDocumentProcessingValidationException =
                await Assert.ThrowsAsync<DocumentProcessingValidationException>(GetDownloadLinkTask.AsTask);

            //then
            actualDocumentProcessingValidationException.Should()
                .BeEquivalentTo(expectedDocumentProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDocumentProcessingValidationException))),
                        Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
