// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Hl7.Fhir.Model;

namespace LHDS.Core.Brokers.Ontologies
{
    public partial interface IOntologyBroker
    {
        ValueTask<Bundle> GetAllAsync(string relativeUrl);
        ValueTask<string> GetArtifactDetailsAsync(string relativeUrl);
    }
}
