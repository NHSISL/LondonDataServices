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
        [Theory]
        [InlineData("CodeSystem")]
        [InlineData("ValueSet")]
        [InlineData("ConceptMap")]
        public async Task ShouldRetrieveArtifactDetailsAsync(string resourceType)
        {
            //Given
            DateTimeOffset dateTimeOffset = this.dateTimeBroker.GetCurrentDateTimeOffset();
            TerminologyArtifact terminologyArtifact = CreateRandomTerminologyArtifact(dateTimeOffset);

            string relativeUrl = $"{this.ontologyConfiguration.TerminologyServerBaseUrl}" +
                $"{this.ontologyConfiguration.TerminologyServerResourceRelativeUrl}";

            relativeUrl = relativeUrl.Replace("{{resourceType}}", resourceType);
            relativeUrl = relativeUrl.Replace("{{datestamp}}", dateTimeOffset.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz"));
            terminologyArtifact.FullUrl = $"{relativeUrl}";
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
