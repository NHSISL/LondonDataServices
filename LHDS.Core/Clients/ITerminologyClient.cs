// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.Core.Clients
{
    public interface ITerminologyClient
    {
        public ValueTask RetrieveArtifactMetadataAsync(string[] resourceTypes);
        public ValueTask RetrieveArtifactDetailsAsync();
    }
}
