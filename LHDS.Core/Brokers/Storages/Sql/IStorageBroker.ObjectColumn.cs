// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ObjectColumns;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<ObjectColumn> InsertObjectColumnAsync(
            ObjectColumn objectColumn,
            CancellationToken cancellationToken = default);

        ValueTask<IQueryable<ObjectColumn>> SelectAllObjectColumnsAsync(
            CancellationToken cancellationToken = default);

        ValueTask<ObjectColumn> SelectObjectColumnByIdAsync(
            Guid objectColumnId,
            CancellationToken cancellationToken = default);

        ValueTask<ObjectColumn> UpdateObjectColumnAsync(
            ObjectColumn objectColumn,
            CancellationToken cancellationToken = default);

        ValueTask<ObjectColumn> DeleteObjectColumnAsync(
            ObjectColumn objectColumn,
            CancellationToken cancellationToken = default);
    }
}