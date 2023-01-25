// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Landings.Client.Models.Foundations.Documents.Exceptions;
using LHDS.Landings.Client.Services.Foundations.Downloads;
using Microsoft.Extensions.Configuration;
using Moq;
using NEL.Premises.Api.Models.Documents.Exceptions;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.Downloads
{
    public partial class DocumentsServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnSelectByFileNameIfInputsIsInvalid(string invalidInput)
        {
            // Given
            string fileName = invalidInput;
            string containerName = invalidInput;

            var invalidDocumentException =
                new InvalidDocumentException();

            invalidDocumentException.AddData(
                key: "fileName",
                values: "Text is required");

            invalidDocumentException.AddData(
                key: "container",
                values: "Text is required");

            var expectedDocumentBlobValidationException
                = new DocumentValidationException(invalidDocumentException);

            var appSettingsStub = new Dictionary<string, string> {
                {"blobContainerName", invalidInput}
            };

            var inMemoryConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(appSettingsStub)
                .Build();

            var documentService = new DocumentService(
                    blobStorageBroker: this.blobStorageBrokerMock.Object,
                    loggingBroker: this.loggingBrokerMock.Object,
                    configuration: inMemoryConfiguration);

            // When
            ValueTask<string> getDownloadLinkTask = documentService.SelectDocumentByFileNameAsync(fileName);

            DocumentValidationException actualDocumentBlobValidationException =
                await Assert.ThrowsAsync<DocumentValidationException>(getDownloadLinkTask.AsTask);

            // Then
            actualDocumentBlobValidationException.Should().BeEquivalentTo(expectedDocumentBlobValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDocumentBlobValidationException))),
                        Times.Once);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.SelectByFileNameAsync(It.IsAny<string>(), It.IsAny<string>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }
    }
}