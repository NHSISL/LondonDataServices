// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Batches;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<Batch> Batches { get; set; }

        public async ValueTask<Batch> InsertBatchAsync(Batch batch) =>
            await InsertAsync(batch);

        public IQueryable<Batch> SelectAllBatches() => ReadAll<Batch>();

        public async ValueTask<Batch> SelectBatchByIdAsync(Guid batchId) =>
            await ReadAsync<Batch>(batchId);

        public async ValueTask<Batch> UpdateBatchAsync(Batch batch) =>
            await UpdateAsync(batch);

        public async ValueTask<Batch> DeleteBatchAsync(Batch batch) =>
            await DeleteAsync(batch);
    }
}
