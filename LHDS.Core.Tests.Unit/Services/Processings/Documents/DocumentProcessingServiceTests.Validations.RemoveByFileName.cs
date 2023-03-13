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
        [Fact]
        public async Task ShouldThrowValidationExceptionsOnRemoveIfDocumentProcessingIsNullAndLogItAsync()
        {
            // given
            string nullFileName = null;

            Document document = new Document
            {
                FileName = nullFileName
            };

            var nullDocumentProcessingFileNameException =
                new NullDocumentProcessingFileNameException();

            var expectedDocumentProcessingValidationException =
                new DocumentProcessingValidationException(nullDocumentProcessingFileNameException);

            // when
            ValueTask RemoveDocumentTask =
                this.documentProcessingService.RemoveDocumentByFileNameAsync(document.FileName);

            DocumentProcessingValidationException actualDocumentProcessingValidationException =
                await Assert.ThrowsAsync<DocumentProcessingValidationException>(RemoveDocumentTask.AsTask);

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
