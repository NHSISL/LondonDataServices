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

        public async ValueTask<DataSetObject> InsertDataSetObjectAsync(DataSetObject dataSetObject) =>
            await InsertAsync(dataSetObject);

        public IQueryable<DataSetObject> SelectAllDataSetObjects() => ReadAll<DataSetObject>();

        public async ValueTask<DataSetObject> SelectDataSetObjectByIdAsync(Guid dataSetObjectId) =>
            await ReadAsync<DataSetObject>(dataSetObjectId);

        public async ValueTask<DataSetObject> UpdateDataSetObjectAsync(DataSetObject dataSetObject) =>
            await UpdateAsync(dataSetObject);

        public async ValueTask<DataSetObject> DeleteDataSetObjectAsync(DataSetObject dataSetObject) =>
            await DeleteAsync(dataSetObject);
    }
}