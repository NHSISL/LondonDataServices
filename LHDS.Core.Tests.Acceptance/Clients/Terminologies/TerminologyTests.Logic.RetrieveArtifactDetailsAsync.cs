// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
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

            string inputFilePath = Path.Combine(
                Path.GetDirectoryName(assembly),
                @"Resources/Clients/Terminology/ShouldRetrieveArtifactDetailsAsync.json");

            string randomArtifactDetail = await File.ReadAllTextAsync(inputFilePath);

            this.ontologyBrokerMock.Setup(broker =>
                broker.GetArtifactDetailsAsync(It.IsAny<string>()))
                    .ReturnsAsync(randomArtifactDetail);

            string fileName = $"{undownloadedTerminologyArtifact.ResourceType}/" +
                $"{undownloadedTerminologyArtifact.Name}.json";

            // when
            await this.terminologyClient.RetrieveArtifactDetailsAsync();

            // then
            this.ontologyBrokerMock.Verify(broker =>
                broker.GetArtifactDetailsAsync(It.IsAny<string>()),
                    Times.Once());
       
            this.ontologyBrokerMock.VerifyNoOtherCalls();
        }
    }
}
