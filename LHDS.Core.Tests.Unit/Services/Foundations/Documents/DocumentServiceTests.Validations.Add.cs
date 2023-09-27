// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Services.Foundations.Documents;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Documents
{
    public partial class DocumentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionsOnAddIfDocumentIsNullAndLogItAsync()
        {
            // given
            Document nullDocument = null;

            var nullDocumentException =
                new NullDocumentException(message: "Document is Null");

            var expectedDocumentValidationException =
                new DocumentValidationException(
                    message: "Document validation errors occured, please try again",
                    innerException: nullDocumentException);

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

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfDocumentDataIsInvalidAndLogItAsync()
        {
            // Given
            string validFileName = GetRandomString();
            byte[] invalidData = null;

            Document document = new Document
            {
                FileName = validFileName,
                DocumentData = invalidData
            };

            var invalidDocumentException = new InvalidDocumentException(
                message: "Invalid document. Please correct the errors and try again.");

            invalidDocumentException.AddData(
                 key: "DocumentData",
                 values: "Data is required");

            var expectedDocumentValidationException
                = new DocumentValidationException(
                    message: "Document validation errors occured, please try again",
                    innerException: invalidDocumentException);

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
                broker.InsertFileAsync(validFileName, It.IsAny<Stream>()),
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
            var appSettingsStub = new Dictionary<string, string> {
                {"blobContainerName", invalidInput}
            };

            var inMemoryConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(appSettingsStub)
                .Build();

            var documentService = new DocumentService(
               blobStorageBroker: this.blobStorageBrokerMock.Object,
               dateTimeBroker: this.dateTimeBrokerMock.Object,
               loggingBroker: this.loggingBrokerMock.Object,
               configuration: inMemoryConfiguration);

            string invalidFileName = invalidInput;

            Document document = new Document
            {
                FileName = invalidFileName,
                DocumentData = Encoding.ASCII.GetBytes(GetRandomString())
            };

            Stream validStream = new MemoryStream(document.DocumentData);

            var invalidDocumentException = new InvalidDocumentException(
                message: "Invalid document. Please correct the errors and try again.");

            invalidDocumentException.AddData(
                key: "FileName",
                values: "Text is required");

            var expectedDocumentValidationException
                = new DocumentValidationException(
                    message: "Document validation errors occured, please try again",
                    innerException: invalidDocumentException);

            // When
            ValueTask uploadFileTask = documentService.AddDocumentAsync(document);

            DocumentValidationException actualDocumentValidationException =
                await Assert.ThrowsAsync<DocumentValidationException>(uploadFileTask.AsTask);

            // Then
            actualDocumentValidationException.Should().BeEquivalentTo(expectedDocumentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDocumentValidationException))),
                        Times.Once);

            this.blobStorageBrokerMock.Verify(broker =>
               broker.InsertFileAsync(invalidFileName, validStream),
                   Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }
    }
}