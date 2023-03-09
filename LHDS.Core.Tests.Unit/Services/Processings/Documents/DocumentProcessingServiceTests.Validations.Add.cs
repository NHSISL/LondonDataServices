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
        public async Task ShouldThrowValidationExceptionsOnAddIfDocumentProcessingIsNullAndLogItAsync()
        {
            // given
            Document nullDocument = null;

            var nullDocumentProcessingException =
                new NullDocumentProcessingException();

            var expectedDocumentProcessingValidationException =
                new DocumentProcessingValidationException(nullDocumentProcessingException);

            this.documentServiceMock.Setup(service =>
                service.AddDocumentAsync(nullDocument))
                    .Throws(expectedDocumentProcessingValidationException);

            // when
            ValueTask AddDocumentTask =
                this.documentProcessingService.AddDocumentAsync(nullDocument);

            DocumentProcessingValidationException actualDocumentProcessingValidationException =
                await Assert.ThrowsAsync<DocumentProcessingValidationException>(AddDocumentTask.AsTask);

            //then
            actualDocumentProcessingValidationException.Should()
                .BeEquivalentTo(expectedDocumentProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDocumentProcessingValidationException))),
                        Times.Once);

            this.documentServiceMock.Verify(service =>
                service.AddDocumentAsync(nullDocument),
                    Times.Once); 

            this.documentServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
