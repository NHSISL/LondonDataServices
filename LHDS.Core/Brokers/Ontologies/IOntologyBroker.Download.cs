// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.Core.Brokers.Ontologies
{
    using System.Threading.Tasks;

    public partial interface IOntologyBroker
    {
        ValueTask<string> GetArtifactDetailsAsync(string relativeUrl);
    }
}
