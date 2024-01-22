// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Hl7.Fhir.Model;

namespace LHDS.Core.Brokers.Ontologies
{
    internal partial class OntologyBroker
    {
        public async ValueTask<Bundle> GetAllValueSetsAsync(string relativeUrl) =>
            await GetAsync<Bundle>(relativeUrl);
    }
}
