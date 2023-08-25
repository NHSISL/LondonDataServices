// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSets;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<DatasetSpecification> DataSets { get; set; }

        public async ValueTask<DatasetSpecification> InsertDataSetAsync(DatasetSpecification dataSet) =>
            await InsertAsync(dataSet);

        public IQueryable<DatasetSpecification> SelectAllDataSets() => ReadAll<DatasetSpecification>();

        public async ValueTask<DatasetSpecification> SelectDataSetByIdAsync(Guid dataSetId) =>
            await ReadAsync<DatasetSpecification>(dataSetId);

        public async ValueTask<DatasetSpecification> UpdateDataSetAsync(DatasetSpecification dataSet) =>
            await UpdateAsync(dataSet);

        public async ValueTask<DatasetSpecification> DeleteDataSetAsync(DatasetSpecification dataSet) =>
            await DeleteAsync(dataSet);
    }
}