// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using Microsoft.EntityFrameworkCore;

namespace LHDS.ConfigImportExportTool.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<SpecificationObject> SpecificationObjects { get; set; }

        public async ValueTask<SpecificationObject> InsertSpecificationObjectAsync(
            SpecificationObject specificationObject) => await InsertAsync(specificationObject);

        public async ValueTask<IQueryable<SpecificationObject>> SelectAllSpecificationObjectsAsync() =>
            await SelectAllAsync<SpecificationObject>();

        public async ValueTask<SpecificationObject> SelectSpecificationObjectByIdAsync(Guid specificationObjectId) =>
            await SelectAsync<SpecificationObject>(specificationObjectId);

        public async ValueTask<SpecificationObject> UpdateSpecificationObjectAsync(
            SpecificationObject specificationObject) => await UpdateAsync(specificationObject);

        public async ValueTask<SpecificationObject> DeleteSpecificationObjectAsync(
            SpecificationObject specificationObject) => await DeleteAsync(specificationObject);
    }
}