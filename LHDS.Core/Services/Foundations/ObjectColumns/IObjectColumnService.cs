// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ObjectColumns;

namespace LHDS.Core.Services.Foundations.ObjectColumns
{
    public interface IObjectColumnService
    {
        ValueTask<ObjectColumn> AddObjectColumnAsync(ObjectColumn objectColumn);
        IQueryable<ObjectColumn> RetrieveAllObjectColumns();
        ValueTask<ObjectColumn> RetrieveObjectColumnByIdAsync(Guid objectColumnId);
        ValueTask<ObjectColumn> ModifyObjectColumnAsync(ObjectColumn objectColumn);
        ValueTask<ObjectColumn> RemoveObjectColumnByIdAsync(Guid objectColumnId);
    }
}