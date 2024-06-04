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
        [Fact]
        public async Task ShouldRetrieveArtifactDetailsAsync()
        {
            //Given
            DateTimeOffset dateTimeOffset = this.dateTimeBroker.GetCurrentDateTimeOffset();
            IQueryable<TerminologyArtifact> terminologyArtifacts = CreateRandomTerminologyArtifacts(dateTimeOffset);
            string authUri = $"{ontologyConfiguration.TerminologyServerAuthenticationRelativeUrl}";

            foreach (TerminologyArtifact terminologyArtifact in terminologyArtifacts)
            {
                await this.terminologyArtifactService.AddTerminologyArtifactAsync(terminologyArtifact);

            }

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
                            .WithPath($"test"))
                        .RespondWith(
                            Response.Create()
                                .WithStatusCode(HttpStatusCode.OK)
                                .WithBody("test"));

            //When
            await this.terminologyClient.RetrieveArtifactDetailsAsync();

            //Then
            //IQueryable<TerminologyPoll> retrievedTerminologyPolls =
            //    this.terminologyPollService.RetrieveAllTerminologyPolls();

            //retrievedTerminologyPolls.Count().Should().BeGreaterThan(0);

            //foreach (TerminologyPoll poll in retrievedTerminologyPolls)
            //{
            //    await this.terminologyPollService.RemoveTerminologyPollByIdAsync(poll.Id);
            //}

            //IQueryable<TerminologyArtifact> retrievedTerminologyArtifacts =
            //    this.terminologyArtifactService.RetrieveAllTerminologyArtifacts();

            //retrievedTerminologyArtifacts.Count().Should().BeGreaterThan(0);

            //foreach (TerminologyArtifact artifact in retrievedTerminologyArtifacts)
            //{
            //    await this.terminologyArtifactService.RemoveTerminologyArtifactByIdAsync(artifact.Id);
            //}
        }
    }
}
