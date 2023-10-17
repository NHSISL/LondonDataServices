// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Documents;
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
        public async Task ShouldThrowValidationExceptionsOnRetrieveIfDocumentProcessingIsNullAndLogItAsync(
            string invalidInput)
        {
            // given
            string invalidFileName = invalidInput;
            string invalidContainer = invalidInput;

            var invalidDocumentProcessingFileNameException =
                new InvalidDocumentProcessingFileNameException(
                    message: "Invalid document processing file name. Please correct the errors and try again.");

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
            ValueTask<Document> RetrieveDocumentTask =
                this.documentProcessingService.RetrieveDocumentByFileNameAsync(invalidFileName, invalidContainer);

            DocumentProcessingValidationException actualDocumentProcessingValidationException =
                await Assert.ThrowsAsync<DocumentProcessingValidationException>(RetrieveDocumentTask.AsTask);

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
