// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Ontologies;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using Newtonsoft.Json;
using Tynamix.ObjectFiller;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;

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

            this.wireMockServer
                .Given(
                    Request.Create()
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithSuccess()
                        .WithBody(serialisedResponseMessage));

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
