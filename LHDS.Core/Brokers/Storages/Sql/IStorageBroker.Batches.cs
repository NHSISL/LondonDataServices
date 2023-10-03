// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Batches;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<Batch> InsertBatchAsync(Batch batch);
        IQueryable<Batch> SelectAllBatches();
        ValueTask<Batch> SelectBatchByIdAsync(Guid batchId);
        ValueTask<Batch> UpdateBatchAsync(Batch batch);
        ValueTask<Batch> DeleteBatchAsync(Batch batch);
    }
}
