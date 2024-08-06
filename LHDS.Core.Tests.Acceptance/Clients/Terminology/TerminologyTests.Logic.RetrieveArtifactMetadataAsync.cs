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

            string returnedJsonString = $@"{{
                ""resourceType"": ""Bundle"",
                ""id"": ""{Guid.NewGuid}"",
                ""meta"": {{
                    ""lastUpdated"": ""2024-06-04T10:59:29.125+00:00""
                }},
                ""type"": ""searchset"",
                ""total"": 18,
                ""link"": [
                    {{
                        ""relation"": ""self"",
                        ""url"": ""https://ontology.onelondon.online/authoring/fhir/ValueSet?_count=1&_elements=name%2Ctitle%2Curl%2Cversion&_lastUpdated=ge2023-06-20T09%3A16%3A18.872%2000%3A00""
                    }}
                ],
                ""entry"": [
                    {{
                        ""fullUrl"": ""https://ontology.onelondon.online/authoring/fhir/ValueSet/6d5030f7-5575-4500-beb5-2a72d0c6e10e"",
                        ""resource"": {{
                            ""resourceType"": ""{resourceType}"",
                            ""id"": ""{Guid.NewGuid}"",
                            ""meta"": {{
                                ""versionId"": ""1"",
                                ""lastUpdated"": ""2023-07-01T13:18:41.902+00:00"",
                                ""tag"": [
                                    {{
                                        ""system"": ""http://terminology.hl7.org/CodeSystem/v3-ObservationValue"",
                                        ""code"": ""SUBSETTED"",
                                        ""display"": ""Resource encoded in summary mode""
                                    }}
                                ]
                            }},
                            ""url"": ""https://kch.htndisorderltc"",
                            ""version"": ""1.0"",
                            ""name"": ""Kch_htndisorderltc"",
                            ""title"": ""kch-htndisorderltc""
                        }}
                    }}
                ]
            }}";

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

            //foreach (TerminologyPoll poll in retrievedTerminologyPolls)
            //{
            //    await this.terminologyPollService.RemoveTerminologyPollByIdAsync(poll.Id);
            //}

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
