// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Brokers.Ontologies;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Terminology
{
    public partial class TerminologyTests
    {
        [Theory]
        [InlineData("CodeSystem")]
        [InlineData("ValueSet")]
        [InlineData("ConceptMap")]
        public async Task ShouldRetrieveArtifactMetadataAsync(string resourceType)
        {
            //Given
            DateTimeOffset dateTimeOffset = GetRandomDateTimeOffset();
            string[] resourceTypes = new string[] { resourceType };
            string relativeUrl = $"/authoring/fhir/{resourceType}";
            string authUri = $"{ontologyConfiguration.TerminologyServerAuthenticationRelativeUrl}";

            OntologyAccessToken token = new OntologyAccessToken
            {
                AccessToken = GetRandomString(),
                ExpiresIn = 1800,
                TokenType = "Bearer",
                NotBeforePolicy = 0,
                Scope = ""
            };

            this.wireMockServer.Given(
                Request.Create()
                        .WithPath($"{authUri}")
                        .UsingPost())
                    .RespondWith(
                        Response.Create()
                            .WithStatusCode(HttpStatusCode.OK)
                            .WithBodyAsJson(token));

            this.wireMockServer.Given(
                Request.Create()
                        .UsingGet()
                        .WithPath($"{relativeUrl}"))
                    .RespondWith(
                        Response.Create()
                            .WithStatusCode(HttpStatusCode.OK)
                            .WithBody(returnedJsonString));

            //When
            await this.terminologyClient.RetrieveArtifactMetadataAsync(resourceTypes);

            //Then
            IQueryable<TerminologyPoll> retrievedTerminologyPolls =
                this.terminologyPollService.RetrieveAllTerminologyPolls();

            retrievedTerminologyPolls.Count().Should().BeGreaterThan(0);

            foreach (TerminologyPoll poll in retrievedTerminologyPolls)
            {
                await this.terminologyPollService.RemoveTerminologyPollByIdAsync(poll.Id);
            }

            IQueryable<TerminologyArtifact> retrievedTerminologyArtifacts =
                this.terminologyArtifactService.RetrieveAllTerminologyArtifacts()
                    .Where(artifact => artifact.ResourceType == resourceType);

            retrievedTerminologyArtifacts.Count().Should().BeGreaterThan(0);

            foreach (TerminologyArtifact artifact in retrievedTerminologyArtifacts)
            {
                await this.terminologyArtifactService.RemoveTerminologyArtifactByIdAsync(artifact.Id);
            }
        }
    }
}
