// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using Moq;
using System.Text;
using System;
using Xunit;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Documents;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace LHDS.Core.Tests.Acceptance.Clients.Terminologies
{
    public partial class TerminologyTests
    {
        [Fact]
        public async Task ShouldRetrieveArtifactDetailsAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = this.dateTimeBroker.GetCurrentDateTimeOffset();
            string assembly = Assembly.GetExecutingAssembly().Location;
            TerminologyArtifact undownloadedTerminologyArtifact = CreateRandomUndownloadedTerminologyArtifact();

            string inputFilePath = Path.Combine(
                Path.GetDirectoryName(assembly),
                @"Resources/Clients/Terminology/AddressExtractions/ShouldRetrieveArtifactDetailsAsync.json");

            string randomArtifactDetail = await File.ReadAllTextAsync(inputFilePath);
            await this.terminologyArtifactService.AddTerminologyArtifactAsync(undownloadedTerminologyArtifact);

            this.ontologyBrokerMock.Setup(broker =>
                broker.GetArtifactDetailsAsync(undownloadedTerminologyArtifact.FullUrl))
                    .ReturnsAsync(randomArtifactDetail);
           

            this.documentProcessingServiceMock.Setup(service =>
                service.AddDocumentAsync(artifactDetailDocument, "Terminology"))
                    .ReturnsAsync(outputFileName);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            TerminologyArtifact downloadedTerminologyArtifact = undownloadedTerminologyArtifact.DeepClone();
            downloadedTerminologyArtifact.IsDownloaded = true;
            downloadedTerminologyArtifact.UpdatedDate = randomDateTimeOffset;

            this.terminologyArtifactProcessingServiceMock.Setup(service =>
                service.ModifyOrAddTerminologyArtifactAsync(downloadedTerminologyArtifact));

            // when
            await this.terminologyClient.RetrieveArtifactDetailsAsync();

            // then
            this.terminologyArtifactProcessingServiceMock.Verify(service =>
                service.GetNonDownloadedArtifactAsync(),
                    Times.Exactly(2));

            this.ontologyProcessingServiceMock.Verify(service =>
                service.RetrieveArtifactDetailsAsync(undownloadedTerminologyArtifact.FullUrl),
                    Times.Once());

            this.documentProcessingServiceMock.Verify(service =>
                service.AddDocumentAsync(It.Is(SameDocumentAs(artifactDetailDocument)), "Terminology"),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.terminologyArtifactProcessingServiceMock.Verify(service =>
                service.ModifyOrAddTerminologyArtifactAsync(It.Is(SameTerminologyArtifactAs(
                    downloadedTerminologyArtifact))),
                    Times.Once);
            
            this.ontologyBrokerMock.VerifyNoOtherCalls();
        }
    }
}
