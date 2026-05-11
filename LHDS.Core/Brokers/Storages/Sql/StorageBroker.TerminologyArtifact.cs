// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<TerminologyArtifact> TerminologyArtifacts { get; set; }

        public async ValueTask<TerminologyArtifact> InsertTerminologyArtifactAsync(
            TerminologyArtifact terminologyArtifact,
            CancellationToken cancellationToken = default) =>
                await InsertAsync(terminologyArtifact, cancellationToken);

        public async ValueTask<IQueryable<TerminologyArtifact>> SelectAllTerminologyArtifactsAsync(
            CancellationToken cancellationToken = default) =>
                await SelectAllAsync<TerminologyArtifact>(cancellationToken);

        public async ValueTask<TerminologyArtifact> SelectTerminologyArtifactByIdAsync(
            Guid terminologyArtifactId,
            CancellationToken cancellationToken = default) =>
                await SelectAsync<TerminologyArtifact>(new object[] { terminologyArtifactId }, cancellationToken);

        public async ValueTask<TerminologyArtifact> UpdateTerminologyArtifactAsync(
            TerminologyArtifact terminologyArtifact,
            CancellationToken cancellationToken = default) =>
                await UpdateAsync(terminologyArtifact, cancellationToken);

        public async ValueTask<TerminologyArtifact> DeleteTerminologyArtifactAsync(
            TerminologyArtifact terminologyArtifact,
            CancellationToken cancellationToken = default) =>
                await DeleteAsync(terminologyArtifact, cancellationToken);
    }
}