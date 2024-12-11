// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;

namespace LHDS.ConfigImportExportTool.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<ObjectColumn> InsertObjectColumnAsync(ObjectColumn objectColumn);
        ValueTask<IQueryable<ObjectColumn>> SelectAllObjectColumnsAsync();
        ValueTask<ObjectColumn> SelectObjectColumnByIdAsync(Guid objectColumnId);
        ValueTask<ObjectColumn> UpdateObjectColumnAsync(ObjectColumn objectColumn);
        ValueTask<ObjectColumn> DeleteObjectColumnAsync(ObjectColumn objectColumn);
    }
}