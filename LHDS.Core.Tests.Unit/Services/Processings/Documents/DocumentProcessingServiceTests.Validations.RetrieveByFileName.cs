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
        public async Task ShouldThrowValidationExceptionsOnRetrieveIfDocumentProcessingIsNullAndLogItAsync(string invalidInput)
        {
            // given
            string invalidFileName = invalidInput;

            var invalidDocumentProcessingFileNameException =
                new InvalidDocumentProcessingFileNameException();

            invalidDocumentProcessingFileNameException.AddData(
                key: "fileName",
                values: "Text is required");

            var expectedDocumentProcessingValidationException =
                new DocumentProcessingValidationException(
                    innerException: invalidDocumentProcessingFileNameException,
                    validationSummary: GetValidationSummary(invalidDocumentProcessingFileNameException.Data));

            // when
            ValueTask<Document> RetrieveDocumentTask =
                this.documentProcessingService.RetrieveDocumentByFileNameAsync(invalidFileName);

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
