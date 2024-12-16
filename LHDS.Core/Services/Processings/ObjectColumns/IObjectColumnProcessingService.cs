// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ObjectColumns;

namespace LHDS.Core.Services.Processings.ObjectColumns
{
    public interface IObjectColumnProcessingService
    {
        ValueTask<ObjectColumn> AddObjectColumnAsync(ObjectColumn objectColumn);
        IQueryable<ObjectColumn> RetrieveAllObjectColumns();
        ValueTask<ObjectColumn> RetrieveObjectColumnByIdAsync(Guid objectColumnId);
        ValueTask<ObjectColumn> RetrieveOrAddObjectColumnAsync(ObjectColumn objectColumn);
        ValueTask<ObjectColumn> ModifyOrAddObjectColumnAsync(ObjectColumn objectColumn);
        ValueTask<ObjectColumn> ModifyObjectColumnAsync(ObjectColumn objectColumn);
        ValueTask<ObjectColumn> RemoveObjectColumnByIdAsync(Guid objectColumnId);
    }
}
