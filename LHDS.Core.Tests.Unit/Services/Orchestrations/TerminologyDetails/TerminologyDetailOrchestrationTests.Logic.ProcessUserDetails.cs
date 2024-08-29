// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.TerminologyDetails
{
    public partial class TerminologyDetailOrchestrationTests
    {
        [Fact]
        public async Task ShouldProcessRetrieveArtifactDetailsForUsersAsync()
        {
            // given
            string inputContainer = blobContainers.Terminology;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            TerminologyArtifact randomTerminologyArtifacts = CreateRandomUndownloadedUserTerminologyArtifact();
            TerminologyArtifact undownloadedTerminologyArtifact = randomTerminologyArtifacts;
            string outputArtifactDetail = GetRandomString();

            this.terminologyArtifactProcessingServiceMock.SetupSequence(service =>
                service.GetNonDownloadedUserArtifactAsync())
                    .ReturnsAsync(undownloadedTerminologyArtifact)
                    .ReturnsAsync((TerminologyArtifact?)null);

            this.ontologyProcessingServiceMock.Setup(service =>
                service.RetrieveArtifactDetailsAsync(undownloadedTerminologyArtifact.FullUrl))
                    .ReturnsAsync(outputArtifactDetail);

            byte[] outputArtifactDetailData = Encoding.UTF8.GetBytes(outputArtifactDetail);
            Stream inputStream = new MemoryStream(outputArtifactDetailData);
            Stream expectedStream = inputStream;
            Stream actualStream = new MemoryStream();
            string inputFileName = $"Subscribers/{undownloadedTerminologyArtifact.ResourceType}/" +
                    $"{undownloadedTerminologyArtifact.Name}.json";

            this.documentProcessingServiceMock
                .Setup(service => service.AddDocumentAsync(It.Is(SameStreamAs(inputStream)), inputFileName, inputContainer))
                .Callback<Stream, string, string>((input, fileName, container) =>
                {
                    input.Position = 0;
                    input.CopyTo(actualStream);
                })
                .Returns(ValueTask.CompletedTask);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            TerminologyArtifact downloadedTerminologyArtifact = undownloadedTerminologyArtifact.DeepClone();
            downloadedTerminologyArtifact.IsDownloadedForUser = true;
            downloadedTerminologyArtifact.UpdatedDate = randomDateTimeOffset;

            this.terminologyArtifactProcessingServiceMock.Setup(service =>
                service.ModifyOrAddTerminologyArtifactAsync(downloadedTerminologyArtifact));

            // when
            await this.terminologyDetailOrchestrationService.RetrieveUserArtifactDetailsAsync();

            // then
            Assert.True(IsSameStream(expectedStream, actualStream));

            this.terminologyArtifactProcessingServiceMock.Verify(service =>
                service.GetNonDownloadedUserArtifactAsync(),
                    Times.Exactly(2));

            this.ontologyProcessingServiceMock.Verify(service =>
                service.RetrieveArtifactDetailsAsync(undownloadedTerminologyArtifact.FullUrl),
                    Times.Once());

            this.documentProcessingServiceMock.Verify(service =>
                service.AddDocumentAsync(It.IsAny<Stream>(), inputFileName, inputContainer),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.terminologyArtifactProcessingServiceMock.Verify(service =>
                service.ModifyOrAddTerminologyArtifactAsync(It.Is(SameTerminologyArtifactAs(
                    downloadedTerminologyArtifact))),
                    Times.Once);

            this.terminologyArtifactProcessingServiceMock.VerifyNoOtherCalls();
            this.ontologyProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}