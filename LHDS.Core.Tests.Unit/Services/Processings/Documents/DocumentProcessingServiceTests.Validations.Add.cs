// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        public async Task ShouldThrowValidationExceptionsOnAddIfDocumentProcessingIsNullAndLogItAsync()
        {
            // given
            string randomContainer = GetRandomString();
            Document nullDocument = null;

            var nullDocumentProcessingException =
                new NullDocumentProcessingException(
                    message: $"Document processing is Null");

            var expectedDocumentProcessingValidationException =
                new DocumentProcessingValidationException(
                    message: "Document processing validation errors occured, please try again",
                    nullDocumentProcessingException);

            // when
            ValueTask<string> addDocumentTask =
                this.documentProcessingService.AddDocumentAsync(document: nullDocument, container: randomContainer);

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
