// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FHIR.Modules.Resources.Foundation.Bundles;

namespace LHDS.Core.Brokers.Ontologies
{
    internal partial class OntologyBroker
    {
        public async ValueTask<Bundle> GetAllValueSetsAsync(string relativeUrl) =>
            await GetAsync<Bundle>(relativeUrl);
    }
}
