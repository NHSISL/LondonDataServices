// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.TerminologyDetails
{
    public partial class TerminologyDetailOrchestrationTests
    {
        [Fact]
        public async Task ShouldProcessRetrieveArtifactDetailsAsync()
        {
            // given
            IQueryable<TerminologyArtifact> randomTerminologyArtifacts =
                CreateRandomUndownloadedTerminologyArtifacts();

            IQueryable<TerminologyArtifact> inputTerminologyArtifacts = randomTerminologyArtifacts;
            IQueryable<TerminologyArtifact> undownloadedTerminologyArtifacts = inputTerminologyArtifacts;
            TerminologyArtifact undownloadedTerminologyArtifact = undownloadedTerminologyArtifacts.First();
            string inputFullUrl = undownloadedTerminologyArtifact.FullUrl;
            string inputFileName = undownloadedTerminologyArtifact.Id.ToString();
            string outputFileName = inputFileName;
            string outputArtifactDetail = GetRandomString();

            this.terminologyArtifactProcessingServiceMock.SetupSequence(service =>
                service.GetNonDownloadedArtifactAsync())
                    .ReturnsAsync(undownloadedTerminologyArtifact)
                    .ReturnsAsync((TerminologyArtifact)null);

            this.ontologyProcessingServiceMock.Setup(service =>
                service.RetrieveArtifactDetailsAsync(inputFullUrl))
                    .ReturnsAsync(outputArtifactDetail);

            byte[] outputArtifactDetailData = Encoding.UTF8.GetBytes(outputArtifactDetail);

            Document artifactDetailDocument = new Document
            {
                FileName = $"{undownloadedTerminologyArtifact.ResourceType}/{undownloadedTerminologyArtifact.Name}.txt",
                DocumentData = outputArtifactDetailData
            };

            this.documentProcessingServiceMock.Setup(service =>
                service.AddDocumentAsync(artifactDetailDocument, "Termminology"))
                    .ReturnsAsync(outputFileName);

            TerminologyArtifact downloadedTerminologyArtifact = undownloadedTerminologyArtifact.DeepClone();
            downloadedTerminologyArtifact.IsDownloaded = true;

            this.terminologyArtifactProcessingServiceMock.Setup(service =>
                service.ModifyOrAddTerminologyArtifactAsync(downloadedTerminologyArtifact));

            // when
            await this.terminologyDetailOrchestrationService.RetrieveArtifactDetailsAsync();

            // then
            this.terminologyArtifactProcessingServiceMock.Verify(service =>
                service.GetNonDownloadedArtifactAsync(),
                    Times.Exactly(2));

            this.ontologyProcessingServiceMock.Verify(service =>
                service.RetrieveArtifactDetailsAsync(inputFullUrl),
                    Times.Once());

            this.documentProcessingServiceMock.Verify(service =>
                service.AddDocumentAsync(It.Is<Document>(document =>
                        document.FileName == artifactDetailDocument.FileName &&
                        document.DocumentData.SequenceEqual(artifactDetailDocument.DocumentData)), It.IsAny<string>()),
                    Times.Once);

            this.terminologyArtifactProcessingServiceMock.Verify(service =>
                service.ModifyOrAddTerminologyArtifactAsync(It.Is<TerminologyArtifact>(artifact =>
                        artifact.Id == downloadedTerminologyArtifact.Id &&
                        artifact.IsDownloaded == downloadedTerminologyArtifact.IsDownloaded)),
                    Times.Once);

            this.terminologyArtifactProcessingServiceMock.VerifyNoOtherCalls();
            this.ontologyProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}