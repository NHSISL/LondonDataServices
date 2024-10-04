// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using Microsoft.EntityFrameworkCore;

namespace LHDS.ConfigImportExportTool.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<ObjectColumn> ObjectColumns { get; set; }

        public async ValueTask<ObjectColumn> InsertObjectColumnAsync(ObjectColumn objectColumn) =>
            await InsertAsync(objectColumn);

        public async ValueTask<IQueryable<ObjectColumn>> SelectAllObjectColumnsAsync() =>
            await SelectAllAsync<ObjectColumn>();

        public async ValueTask<ObjectColumn> SelectObjectColumnByIdAsync(Guid objectColumnId) =>
            await SelectAsync<ObjectColumn>(objectColumnId);

        public async ValueTask<ObjectColumn> UpdateObjectColumnAsync(ObjectColumn objectColumn) =>
            await UpdateAsync(objectColumn);

        public async ValueTask<ObjectColumn> DeleteObjectColumnAsync(ObjectColumn objectColumn) =>
            await DeleteAsync(objectColumn);
    }
}