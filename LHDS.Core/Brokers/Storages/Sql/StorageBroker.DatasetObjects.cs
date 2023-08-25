// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSetObjects;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<DataSetObject> DatasetObjects { get; set; }

        public async ValueTask<DataSetObject> InsertDatasetObjectAsync(DataSetObject datasetObject) =>
            await InsertAsync(datasetObject);

        public IQueryable<DataSetObject> SelectAllDatasetObjects() => ReadAll<DataSetObject>();

        public async ValueTask<DataSetObject> SelectDatasetObjectByIdAsync(Guid datasetObjectId) =>
            await ReadAsync<DataSetObject>(datasetObjectId);

        public async ValueTask<DataSetObject> UpdateDatasetObjectAsync(DataSetObject datasetObject) =>
            await UpdateAsync(datasetObject);

        public async ValueTask<DataSetObject> DeleteDatasetObjectAsync(DataSetObject datasetObject) =>
            await DeleteAsync(datasetObject);
    }
}