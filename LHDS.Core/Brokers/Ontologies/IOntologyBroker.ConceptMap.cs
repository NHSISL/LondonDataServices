// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

namespace LHDS.Core.Brokers.Ontologies
{
    using System.Threading.Tasks;
    using Hl7.Fhir.Model;

    internal partial interface IOntologyBroker
    {
        ValueTask<Bundle> GetAllConceptMapsAsync(string url);
    }
}
