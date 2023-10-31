// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

namespace LHDS.Core.Brokers.Ontologies
{
    using System.Threading.Tasks;
    using FHIR.Modules.Resources.Foundation.Bundles;

    internal partial interface IOntologyBroker
    {
        ValueTask<Bundle> GetAllCodingSystemsAsync(string url);
    }
}
