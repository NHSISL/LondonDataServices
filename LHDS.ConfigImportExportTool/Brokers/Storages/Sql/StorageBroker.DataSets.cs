// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.ConfigImportExportTool.Models.Foundations.Datasets;
using Microsoft.EntityFrameworkCore;

namespace LHDS.ConfigImportExportTool.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<DataSet> DataSets { get; set; }

        public async ValueTask<DataSet> InsertDataSetAsync(DataSet dataSet) =>
            await InsertAsync(dataSet);

        public async ValueTask<IQueryable<DataSet>> SelectAllDataSetsAsync() =>
            await SelectAllAsync<DataSet>();

        public async ValueTask<DataSet> SelectDataSetByIdAsync(Guid dataSetId) =>
            await SelectAsync<DataSet>(dataSetId);

        public async ValueTask<DataSet> UpdateDataSetAsync(DataSet dataSet) =>
            await UpdateAsync(dataSet);

        public async ValueTask<DataSet> DeleteDataSetAsync(DataSet dataSet) =>
            await DeleteAsync(dataSet);
    }
}