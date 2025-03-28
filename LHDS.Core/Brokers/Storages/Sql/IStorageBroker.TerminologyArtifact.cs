// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<TerminologyArtifact> InsertTerminologyArtifactAsync(
            TerminologyArtifact terminologyArtifact);

        ValueTask<IQueryable<TerminologyArtifact>> SelectAllTerminologyArtifactsAsync();
        ValueTask<TerminologyArtifact> SelectTerminologyArtifactByIdAsync(Guid terminologyArtifactId);

        ValueTask<TerminologyArtifact> UpdateTerminologyArtifactAsync(
            TerminologyArtifact terminologyArtifact);

        ValueTask<TerminologyArtifact> DeleteTerminologyArtifactAsync(
            TerminologyArtifact terminologyArtifact);
    }
}
