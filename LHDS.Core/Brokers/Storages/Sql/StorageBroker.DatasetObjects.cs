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
        public DbSet<DataSetObject> DataSetObjects { get; set; }

        public async ValueTask<DataSetObject> InsertDataSetObjectAsync(DataSetObject datasetObject) =>
            await InsertAsync(datasetObject);

        public IQueryable<DataSetObject> SelectAllDataSetObjects() => ReadAll<DataSetObject>();

        public async ValueTask<DataSetObject> SelectDataSetObjectByIdAsync(Guid datasetObjectId) =>
            await ReadAsync<DataSetObject>(datasetObjectId);

        public async ValueTask<DataSetObject> UpdateDataSetObjectAsync(DataSetObject datasetObject) =>
            await UpdateAsync(datasetObject);

        public async ValueTask<DataSetObject> DeleteDataSetObjectAsync(DataSetObject datasetObject) =>
            await DeleteAsync(datasetObject);
    }
}