// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.Core.Brokers.Ontologies
{
    internal partial class OntologyBroker
    {
        public async ValueTask<string> GetArtifactDetailsAsync(string relativeUrl) =>
            await GetAsync<string>(relativeUrl);
    }
}
