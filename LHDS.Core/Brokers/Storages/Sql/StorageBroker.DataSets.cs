// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<DataSet> DataSets { get; set; }

        public async ValueTask<DataSet> InsertDataSetAsync(DataSet dataSet)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<DataSet> dataSetEntityEntry =
                await broker.DataSets.AddAsync(dataSet);

            await broker.SaveChangesAsync();

            return dataSetEntityEntry.Entity;
        }

        public IQueryable<DataSet> SelectAllDataSets()
        {
            using var broker =
                new StorageBroker(this.configuration);

            return broker.DataSets;
        }

        public async ValueTask<DataSet> SelectDataSetByIdAsync(Guid dataSetId)
        {
            using var broker =
                new StorageBroker(this.configuration);

            return await broker.DataSets.FindAsync(dataSetId);
        }

        public async ValueTask<DataSet> UpdateDataSetAsync(DataSet dataSet)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<DataSet> dataSetEntityEntry =
                broker.DataSets.Update(dataSet);

            await broker.SaveChangesAsync();

            return dataSetEntityEntry.Entity;
        }

        public async ValueTask<DataSet> DeleteDataSetAsync(DataSet dataSet)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<DataSet> dataSetEntityEntry =
                broker.DataSets.Remove(dataSet);

            await broker.SaveChangesAsync();

            return dataSetEntityEntry.Entity;
        }
    }
}