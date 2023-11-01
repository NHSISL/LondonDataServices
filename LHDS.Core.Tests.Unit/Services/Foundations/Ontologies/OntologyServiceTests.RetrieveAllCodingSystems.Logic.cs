// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentAssertions;
using Hl7.Fhir.Model;
using LHDS.Core.Models.Foundations.Ontologies;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Ontologys
{
    public partial class OntologyServiceTests
    {
        [Fact]
        public async System.Threading.Tasks.Task ShouldRetrieveAllCodingSystemsByRelativeUrlAsync()
        {
            // given
            string randomRelativeUrl = GetRandomString();
            string inputRelativeUrl = randomRelativeUrl;
            string nextPageUrl = "http://localhost:5000/api/fhir/ValueSet?_page=2";
            string artifactType = "CodeSystem";

            List<dynamic> randomArtifactProperties = CreateRandomArtifactProperties(artifactType);

            var remoteCodingSystemBundle = CreateCodeSystemBundleFromRandomData(randomArtifactProperties, nextPageUrl);
            var expectedOntologyAssets = CreateArtiFactFromRandomData(randomArtifactProperties, nextPageUrl);

            this.ontologyBrokerMock.Setup(broker =>
                broker.GetAllCodingSystemsAsync(inputRelativeUrl))
                    .ReturnsAsync(remoteCodingSystemBundle);

            // when
            OntologyAssets actualOntologyAssets =
                await this.ontologyService.RetrieveAllCodingSystemsAsync(inputRelativeUrl);

            // then
            actualOntologyAssets.Should().BeEquivalentTo(expectedOntologyAssets);

            this.ontologyBrokerMock.Verify(broker =>
                broker.GetAllCodingSystemsAsync(inputRelativeUrl),
                    Times.Once);

            this.ontologyBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        private static Bundle CreateCodeSystemBundleFromRandomData(List<dynamic> randomArtifactProperties, string nextPageUrl)
        {
            Bundle externalBundleResult = new Bundle
            {
                Id = Guid.NewGuid().ToString(),
                Type = Bundle.BundleType.Searchset,
                Total = randomArtifactProperties.Count,
                Link = new List<Bundle.LinkComponent>
                {
                    new Bundle.LinkComponent
                    {
                        Relation = "self",
                        Url = "http://localhost:5000/api/fhir/ValueSet"
                    },
                    new Bundle.LinkComponent
                    {
                        Relation = "next",
                        Url = nextPageUrl
                    }
                },
                Entry = new List<Bundle.EntryComponent>()
            };

            foreach (var item in randomArtifactProperties)
            {
                externalBundleResult.Entry.Add(
                    new Bundle.EntryComponent
                    {
                        FullUrl = item.FullUrl,
                        Resource = new CodeSystem
                        {
                            Meta = new Meta
                            {
                                LastUpdated = item.LastUpdated,
                            },

                            Version = item.Version,
                            Name = item.Name,
                            Title = item.Title,

                            Status = (PublicationStatus)Enum.Parse(
                                typeof(PublicationStatus), item.Status, ignoreCase: true),
                        }
                    });
            }

            return externalBundleResult;
        }

        private static OntologyAssets CreateArtiFactFromRandomData(
            List<dynamic> randomArtifactProperties,
            string nextPageUrl)
        {
            var ontologyAssets = new OntologyAssets
            {
                Assets = new List<OntologyAsset>(),
                NextPage = nextPageUrl
            };

            foreach (var item in randomArtifactProperties)
            {
                ontologyAssets.Assets.Add(
                    new OntologyAsset
                    {
                        FullUrl = item.FullUrl,
                        ResourceType = item.ResourceType,
                        Version = item.Version,
                        Name = item.Name,
                        Title = item.Title,
                        Status = item.Status,
                        LastUpdated = item.LastUpdated
                    });
            }

            return ontologyAssets;
        }
    }
}