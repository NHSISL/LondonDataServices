// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Ontologies;
using LHDS.Core.Models.Foundations.Ontologies;

namespace LHDS.Core.Services.Foundations.Ontologies
{
    internal class OntologyService : IOntologyService
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

        public ValueTask<OntologyAssets> RetrieveAllCodingSystemsAsync(string? relativeUrl) =>
            throw new NotImplementedException();

        public ValueTask<OntologyAssets> RetrieveAllConceptMapsAsync(string? relativeUrl) =>
            throw new NotImplementedException();

        public ValueTask<OntologyAssets> RetrieveAllValueSetsAsync(string? relativeUrl) =>
            throw new NotImplementedException();
    }
}
