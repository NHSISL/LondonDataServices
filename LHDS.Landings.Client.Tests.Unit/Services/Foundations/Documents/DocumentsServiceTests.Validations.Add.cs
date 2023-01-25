// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Landings.Client.Models.Foundations.Documents;
using LHDS.Landings.Client.Models.Foundations.Documents.Exceptions;
using Microsoft.Extensions.Configuration;
using Moq;
using NEL.Premises.Api.Models.Documents.Exceptions;

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
                new NullDocumentException();

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

        [Theory]
        [InlineData(null)]
        public async Task ShouldThrowValidationExceptionOnAddIfDocumentStreamIsInvalidAndLogItAsync(Stream invalidInput)
        {
            // Given
            var blobContainerName = this.inMemoryConfiguration.GetValue<string>("blobContainerName");

            string validFileName = GetRandomString();
            Stream invalidStream = invalidInput;

            Document document = new Document
            {
                FileName = validFileName,
                DocumentStream = invalidStream
            };

            var invalidDocumentException = new InvalidDocumentException();

            invalidDocumentException.AddData(
                 key: "DocumentStream",
                 values: "Stream is required");

            var expectedDocumentValidationException
                = new DocumentValidationException(invalidDocumentException);

            // When
            ValueTask uploadFileTask = this.documentService.AddDocumentAsync(document);

            DocumentValidationException actualDocumentValidationException =
                await Assert.ThrowsAsync<DocumentValidationException>(uploadFileTask.AsTask);

            // Then
            actualDocumentValidationException.Should().BeEquivalentTo(expectedDocumentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDocumentValidationException))),
                        Times.Once);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.InsertFileAsync(validFileName, invalidStream, blobContainerName),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnAddIfFileNameIsInvalid(string invalidInput)
        {
            // Given
            var blobContainerName = invalidInput;

            string invalidFileName = invalidInput;
            Stream validStream = new MemoryStream(Encoding.ASCII.GetBytes(GetRandomString()));

            Document document = new Document
            {
                FileName = invalidFileName,
                DocumentStream = validStream
            };

            var invalidDocumentException = new InvalidDocumentException();

            invalidDocumentException.AddData(
                key: "FileName",
                values: "Text is required");

            var expectedDocumentValidationException
                = new DocumentValidationException(invalidDocumentException);

            // When
            ValueTask uploadFileTask = this.documentService.AddDocumentAsync(document);

            DocumentValidationException actualDocumentValidationException =
                await Assert.ThrowsAsync<DocumentValidationException>(uploadFileTask.AsTask);

            // Then
            actualDocumentValidationException.Should().BeEquivalentTo(expectedDocumentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDocumentValidationException))),
                        Times.Once);

            this.blobStorageBrokerMock.Verify(broker =>
               broker.InsertFileAsync(invalidFileName, validStream, blobContainerName),
                   Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }
    }
}