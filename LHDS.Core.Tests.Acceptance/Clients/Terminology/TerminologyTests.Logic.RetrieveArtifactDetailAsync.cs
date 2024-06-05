// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LHDS.Core.Models.Brokers.Ontologies;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
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
            await this.terminologyArtifactService.RemoveTerminologyArtifactByIdAsync(terminologyArtifacts.First().Id);
        }
    }
}
