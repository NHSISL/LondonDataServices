// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using Xunit;

namespace LHDS.Core.Tests.Integration.Terminology
{
    public partial class TerminologyTests
    {
        [Fact]
        public async Task ShouldRetrieveArtifactDetailsAsync()
        {
            //Given
            DateTimeOffset dateTimeOffset = this.dateTimeBroker.GetCurrentDateTimeOffset();
            TerminologyArtifact terminologyArtifact = CreateRandomTerminologyArtifact(dateTimeOffset);
            await this.terminologyArtifactService.AddTerminologyArtifactAsync(terminologyArtifact);

            //When
            await this.terminologyClient.RetrieveArtifactDetailsAsync();

            //Then
            TerminologyArtifact retrievedTerminologyArtifact =
                await this.terminologyArtifactService.RetrieveTerminologyArtifactByIdAsync(terminologyArtifact.Id);

            retrievedTerminologyArtifact.IsDownloaded.Should().BeTrue();
            string fileName = $"{retrievedTerminologyArtifact.ResourceType}/{retrievedTerminologyArtifact.Name}.json";
            await this.documentService.RemoveDocumentByFileNameAsync(fileName, "terminology");
            await this.terminologyArtifactService.RemoveTerminologyArtifactByIdAsync(retrievedTerminologyArtifact.Id);
        }
    }
}
