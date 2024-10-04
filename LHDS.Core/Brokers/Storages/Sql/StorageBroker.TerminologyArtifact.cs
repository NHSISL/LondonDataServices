// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<TerminologyArtifact> TerminologyArtifacts { get; set; }

        public async ValueTask<TerminologyArtifact> InsertTerminologyArtifactAsync(
            TerminologyArtifact terminologyArtifact) =>
                await InsertAsync(terminologyArtifact);

        public IQueryable<TerminologyArtifact> SelectAllTerminologyArtifacts() => SelectAll<TerminologyArtifact>();

        public async ValueTask<TerminologyArtifact> SelectTerminologyArtifactByIdAsync(
            Guid terminologyArtifactId) =>
                await SelectAsync<TerminologyArtifact>(terminologyArtifactId);

        public async ValueTask<TerminologyArtifact> UpdateTerminologyArtifactAsync(
            TerminologyArtifact terminologyArtifact) =>
                await UpdateAsync(terminologyArtifact);

        public async ValueTask<TerminologyArtifact> DeleteTerminologyArtifactAsync(
            TerminologyArtifact terminologyArtifact) =>
                await DeleteAsync(terminologyArtifact);
    }
}