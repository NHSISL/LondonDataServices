// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Landings.Client.Models.Foundations.Documents.Exceptions;
using LHDS.Landings.Client.Services.Foundations.Documents;
using Microsoft.Extensions.Configuration;
using Moq;
using NEL.Premises.Api.Models.Documents.Exceptions;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.Documents
{
    public partial class DocumentServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnDeleteFileIfInputsIsInvalid(string invalidInput)
        {
            // Given
            string fileName = invalidInput;
            string containerName = invalidInput;
            var isDecrypted = false;

            var invalidDocumentException =
                new InvalidDocumentException();

            invalidDocumentException.AddData(
                key: "fileName",
                values: "Text is required");

            var expectedDocumentValidationException
                = new DocumentValidationException(invalidDocumentException);

            var appSettingsStub = new Dictionary<string, string> {
                {"blobContainerName", invalidInput},
                {"blobUriValidMinutes", "1"}
            };

            var inMemoryConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(appSettingsStub)
                .Build();

            var documentService = new DocumentService(
                    blobStorageBroker: this.blobStorageBrokerMock.Object,
                    loggingBroker: this.loggingBrokerMock.Object,
                    configuration: inMemoryConfiguration);


            // When
            ValueTask deleteFileTask = documentService.RemoveDocumentByFileNameAsync(fileName, isDecrypted);

            DocumentValidationException actualDocumentValidationException =
                await Assert.ThrowsAsync<DocumentValidationException>(deleteFileTask.AsTask);

            // Then
            actualDocumentValidationException.Should().BeEquivalentTo(expectedDocumentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDocumentValidationException))),
                        Times.Once);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.DeleteFileAsync(It.IsAny<string>(), It.IsAny<bool>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
