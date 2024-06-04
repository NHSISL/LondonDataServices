// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Brokers.Ontologies;
using LHDS.Core.Models.Foundations.Ontologies;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using Newtonsoft.Json;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using Tynamix.ObjectFiller;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LHDS.Core.Tests.Acceptance.Clients.Terminology
{
    public partial class TerminologyTests
    {
        [Theory]
        //[InlineData("CodeSystem")]
        [InlineData("ValueSet")]
        //[InlineData("ConceptMap")]
        public async Task ShouldRetrieveArtifactMetadataAsync(string resourceType)
        {
            //Given
            DateTimeOffset dateTimeOffset = GetRandomDateTimeOffset();
            string[] resourceTypes = new string[] { resourceType };

            string relativeUrl = $"{ontologyConfiguration.TerminologyServerBaseUrl}" +
                $"{this.ontologyConfiguration.TerminologyServerResourceRelativeUrl}";

            string authUri = $"{ontologyConfiguration.TerminologyServerBaseUrl}" +
                $"{ontologyConfiguration.TerminologyServerAuthenticationRelativeUrl}";

            OntologyAsset ontologyAsset = new OntologyAsset
            {
                FullUrl = GetRandomString(),
                ResourceType = resourceType,
                Version = GetRandomString(),
                Name = GetRandomString(),
                Title = GetRandomString(),
                Status = GetRandomString(),
                LastUpdated = dateTimeOffset
            };

            string serialisedResponseMessage = JsonConvert.SerializeObject(ontologyAsset);

            var requestContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("client_id", ontologyConfiguration.ClientId),
                    new KeyValuePair<string, string>("client_secret", ontologyConfiguration.ClientSecret)
                });

            this.wireMockServer.Given(
                Request.Create()
                        .UsingPost()
                        .WithPath($"{authUri}"))
                    .RespondWith(
                        Response.Create()
                            .WithStatusCode(HttpStatusCode.OK)
                            .WithBodyAsJson(GetRandomString()));
            
            this.wireMockServer.Given(
                Request.Create()
                        .UsingGet()
                        .WithPath($"{relativeUrl}"))
                    .RespondWith(
                        Response.Create()
                            .WithStatusCode(HttpStatusCode.OK)
                            .WithBodyAsJson(serialisedResponseMessage));

            //When
            await this.terminologyClient.RetrieveArtifactMetadataAsync(resourceTypes);

            //Then
            //IQueryable<TerminologyPoll> retrievedTerminologyPolls =
            //    this.terminologyPollService.RetrieveAllTerminologyPolls();

            //retrievedTerminologyPolls.Count().Should().BeGreaterThan(0);

            //foreach (TerminologyPoll poll in retrievedTerminologyPolls)
            //{
            //    await this.terminologyPollService.RemoveTerminologyPollByIdAsync(poll.Id);
            //}
        }
    }
}
