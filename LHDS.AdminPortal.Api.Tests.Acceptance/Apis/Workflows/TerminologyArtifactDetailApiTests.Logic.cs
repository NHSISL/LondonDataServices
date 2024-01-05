// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.TerminologyArtifact;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.TerminologyPoll;
using LHDS.Core.Models.Foundations.Documents;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.Workflows.TerminologyArtifactDetails
{
    public partial class TerminologyArtifactDetailsApiTests
    {
        [Fact]
        public async Task ShouldRetrieveArtifactDetailsAsync()
        {
            // given
            string assembly = Assembly.GetExecutingAssembly().Location;
            TerminologyArtifact undownloadedTerminologyArtifact = CreateRandomUndownloadedTerminologyArtifact();

            string inputFilePath = Path.Combine(
                Path.GetDirectoryName(assembly),
                @"Resources/Clients/Terminology/ShouldRetrieveArtifactDetailsAsync.json");

            string randomArtifactDetail = await File.ReadAllTextAsync(inputFilePath);
            await this.apiBroker.PostTerminologyArtifactAsync(undownloadedTerminologyArtifact);

            this.ontologyBrokerMock.Setup(broker =>
                broker.GetArtifactDetailsAsync(It.IsAny<string>()))
                    .ReturnsAsync(randomArtifactDetail);

            string fileName = $"{undownloadedTerminologyArtifact.ResourceType}/" +
                $"{undownloadedTerminologyArtifact.Name}.json";

            // when
            await this.apiBroker.RetrieveArtifactDetailsAsync();

            // then
            this.ontologyBrokerMock.Verify(broker =>
                broker.GetArtifactDetailsAsync(It.IsAny<string>()),
                    Times.Once());

            await this.apiBroker.RemoveTerminologyArtifactByIdAsync(undownloadedTerminologyArtifact.Id);
            await this.documentService.RemoveDocumentByFileNameAsync(fileName, blobContainers.Terminology);
            this.ontologyBrokerMock.VerifyNoOtherCalls();
        }
    }
}