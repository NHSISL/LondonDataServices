// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Terminologies
{
    public partial class TerminologyTests
    {
        [Fact]
        public async Task ShouldRetrieveArtifactDetailsAsync()
        {
            // given
            DateTimeOffset dateTimeOffset = this.dateTimeBroker.GetCurrentDateTimeOffset();
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

            // when
            await this.terminologyClient.RetrieveArtifactDetailsAsync();

            // then
            this.ontologyBrokerMock.Verify(broker =>
                broker.GetArtifactDetailsAsync(undownloadedTerminologyArtifact.FullUrl),
                    Times.Once());

            undownloadedTerminologyArtifact.IsDownloaded.Should().BeTrue();

            await this.terminologyArtifactService.
                RemoveTerminologyArtifactByIdAsync(undownloadedTerminologyArtifact.Id);

            this.ontologyBrokerMock.VerifyNoOtherCalls();
        }
    }
}
