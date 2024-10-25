// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSets;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<DataSet> DataSets { get; set; }

        public async ValueTask<DataSet> InsertDataSetAsync(DataSet dataSet) =>
            await InsertAsync(dataSet);

        public IQueryable<DataSet> SelectAllDataSets() => SelectAll<DataSet>();

        public async ValueTask<DataSet> SelectDataSetByIdAsync(Guid dataSetId) =>
            await SelectAsync<DataSet>(dataSetId);

        public async ValueTask<DataSet> UpdateDataSetAsync(DataSet dataSet) =>
            await UpdateAsync(dataSet);

        public async ValueTask<DataSet> DeleteDataSetAsync(DataSet dataSet) =>
            await DeleteAsync(dataSet);
    }
}