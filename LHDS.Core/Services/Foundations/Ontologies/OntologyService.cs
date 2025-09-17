// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Ontologies;
using LHDS.Core.Models.Foundations.Ontologies;
using static Hl7.Fhir.Model.Bundle;

namespace LHDS.Core.Services.Foundations.Ontologies
{
    public partial class OntologyService : IOntologyService
    {
        private readonly IOntologyBroker ontologyBroker;
        private readonly ILoggingBroker loggingBroker;

        public OntologyService(
            IOntologyBroker ontologyBroker,
            ILoggingBroker loggingBroker)
        {
            this.ontologyBroker = ontologyBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<OntologyAssets> RetrieveAllCodingSystemsAsync(string relativeUrl) =>
            TryCatch(async () =>
            {
                await ValidateArgs(relativeUrl);
                Bundle codingSystems = await this.ontologyBroker.GetAllAsync(relativeUrl);
                string? nextPageUrl = codingSystems.NextLink?.ToString();

                var ontologyAssets = new OntologyAssets
                {
                    Assets = new List<OntologyAsset>(),
                    NextPage = nextPageUrl
                };

                foreach (EntryComponent item in codingSystems.Entry)
                {
                    CodeSystem resource = (CodeSystem)item.Resource;

                    ontologyAssets.Assets.Add(
                        new OntologyAsset
                        {
                            FullUrl = item.FullUrl,
                            ResourceType = "CodeSystem",
                            Version = resource.Version,
                            Name = resource.Name,
                            Title = resource.Title,
                            Status = resource.Status.ToString()?.ToLower(),
                            LastUpdated = resource.Meta.LastUpdated,
                        });
                }

                return ontologyAssets;
            });

        public ValueTask<OntologyAssets> RetrieveAllConceptMapsAsync(string relativeUrl) =>
            TryCatch(async () =>
            {
                await ValidateArgs(relativeUrl);
                Bundle valueSets = await this.ontologyBroker.GetAllAsync(relativeUrl);
                string? nextPageUrl = valueSets.NextLink?.ToString();

                var ontologyAssets = new OntologyAssets
                {
                    Assets = new List<OntologyAsset>(),
                    NextPage = nextPageUrl
                };

                foreach (EntryComponent item in valueSets.Entry)
                {
                    ConceptMap resource = (ConceptMap)item.Resource;

                    ontologyAssets.Assets.Add(
                        new OntologyAsset
                        {
                            FullUrl = item.FullUrl,
                            ResourceType = "ConceptMap",
                            Version = resource.Version,
                            Name = resource.Name,
                            Title = resource.Title,
                            Status = resource.Status.ToString()?.ToLower(),
                            LastUpdated = resource.Meta.LastUpdated,
                        });
                }

                return ontologyAssets;
            });

        public ValueTask<OntologyAssets> RetrieveAllValueSetsAsync(string relativeUrl) =>
            TryCatch(async () =>
            {
                await ValidateArgs(relativeUrl);
                Bundle valueSets = await this.ontologyBroker.GetAllAsync(relativeUrl);
                string? nextPageUrl = valueSets.NextLink?.ToString();

                var ontologyAssets = new OntologyAssets
                {
                    Assets = new List<OntologyAsset>(),
                    NextPage = nextPageUrl
                };

                foreach (EntryComponent item in valueSets.Entry)
                {
                    ValueSet resource = (ValueSet)item.Resource;

                    ontologyAssets.Assets.Add(
                        new OntologyAsset
                        {
                            FullUrl = item.FullUrl,
                            ResourceType = "ValueSet",
                            Version = resource.Version,
                            Name = resource.Name,
                            Title = resource.Title,
                            Status = resource.Status.ToString()?.ToLower(),
                            LastUpdated = resource.Meta.LastUpdated,
                        });
                }

                return ontologyAssets;
            });

        public ValueTask<string> RetrieveArtifactDetailsAsync(string relativeUrl) =>
            TryCatch(async () =>
            {
                await ValidateArgs(relativeUrl);

                return await this.ontologyBroker.GetArtifactDetailsAsync(relativeUrl);
            });
    }
}
