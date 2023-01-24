// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Landings.Client.Models.Foundations.Documents;
using LHDS.Landings.Client.Models.Foundations.Documents.Exceptions;
using Moq;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.Downloads
{
    public partial class DocumentsServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionsOnAddIfDocumentIsNullAndLogItAsync()
        {
            // given
            Document nullDocument = null;

            var nullDocumentException =
                new NullDocumentException(nullDocument);

            var expectedDocumentValidationException =
                new DocumentValidationException(nullDocumentException);

            // when
            ValueTask AddDocumentTask =
                this.documentService.AddDocumentAsync(nullDocument);

            DocumentValidationException actualDocumentValidationException =
                await Assert.ThrowsAsync<DocumentValidationException>(AddDocumentTask.AsTask);

            //then
            actualDocumentValidationException.Should()
                .BeEquivalentTo(expectedDocumentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDocumentValidationException))),
                        Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}