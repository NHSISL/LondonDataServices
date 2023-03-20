// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
        public async Task ShouldThrowValidationExceptionsOnGetDownloadLinkIfDocumentProcessingIsNullAndLogItAsync(string invalidInput)
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
            ValueTask<string> GetDownloadLinkTask =
                this.documentProcessingService.GetDownloadLinkAsync(invalidFileName);

            DocumentProcessingValidationException actualDocumentProcessingValidationException =
                await Assert.ThrowsAsync<DocumentProcessingValidationException>(GetDownloadLinkTask.AsTask);

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
