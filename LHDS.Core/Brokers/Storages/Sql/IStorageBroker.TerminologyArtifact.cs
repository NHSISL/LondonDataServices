// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<TerminologyArtifact> InsertTerminologyArtifactAsync(
            TerminologyArtifact terminologyArtifact,
            CancellationToken cancellationToken = default);

        ValueTask<IQueryable<TerminologyArtifact>> SelectAllTerminologyArtifactsAsync(
            CancellationToken cancellationToken = default);

        ValueTask<TerminologyArtifact> SelectTerminologyArtifactByIdAsync(
            Guid terminologyArtifactId,
            CancellationToken cancellationToken = default);

        ValueTask<TerminologyArtifact> UpdateTerminologyArtifactAsync(
            TerminologyArtifact terminologyArtifact,
            CancellationToken cancellationToken = default);

        ValueTask<TerminologyArtifact> DeleteTerminologyArtifactAsync(
            TerminologyArtifact terminologyArtifact,
            CancellationToken cancellationToken = default);
    }
}
