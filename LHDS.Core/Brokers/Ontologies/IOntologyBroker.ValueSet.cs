// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.Core.Brokers.Ontologies
{
    using System.Threading.Tasks;
    using Hl7.Fhir.Model;

    public partial interface IOntologyBroker
    {
        ValueTask<Bundle> GetAllValueSetsAsync(string url);
    }
}
