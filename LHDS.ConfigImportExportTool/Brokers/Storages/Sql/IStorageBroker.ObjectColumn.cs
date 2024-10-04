// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;

namespace LHDS.ConfigImportExportTool.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<ObjectColumn> InsertObjectColumnAsync(ObjectColumn objectColumn);
        IQueryable<ObjectColumn> SelectAllObjectColumns();
        ValueTask<ObjectColumn> SelectObjectColumnByIdAsync(Guid objectColumnId);
        ValueTask<ObjectColumn> UpdateObjectColumnAsync(ObjectColumn objectColumn);
        ValueTask<ObjectColumn> DeleteObjectColumnAsync(ObjectColumn objectColumn);
    }
}