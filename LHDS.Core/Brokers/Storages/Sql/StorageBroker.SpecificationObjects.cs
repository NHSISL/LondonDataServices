// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<SpecificationObject> DataSetObjects { get; set; }

        public async ValueTask<SpecificationObject> InsertDataSetObjectAsync(SpecificationObject dataSetObject) =>
            await InsertAsync(dataSetObject);

        public IQueryable<SpecificationObject> SelectAllDataSetObjects() => ReadAll<SpecificationObject>();

        public async ValueTask<SpecificationObject> SelectDataSetObjectByIdAsync(Guid dataSetObjectId) =>
            await ReadAsync<SpecificationObject>(dataSetObjectId);

        public async ValueTask<SpecificationObject> UpdateDataSetObjectAsync(SpecificationObject dataSetObject) =>
            await UpdateAsync(dataSetObject);

        public async ValueTask<SpecificationObject> DeleteDataSetObjectAsync(SpecificationObject dataSetObject) =>
            await DeleteAsync(dataSetObject);
    }
}