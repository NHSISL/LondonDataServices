// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ObjectColumns;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<ObjectColumn> ObjectColumns { get; set; }

        public async ValueTask<ObjectColumn> InsertObjectColumnAsync(
            ObjectColumn objectColumn,
            CancellationToken cancellationToken = default) =>
                await InsertAsync(objectColumn, cancellationToken);

        public async ValueTask<IQueryable<ObjectColumn>> SelectAllObjectColumnsAsync(
            CancellationToken cancellationToken = default) =>
                await SelectAllAsync<ObjectColumn>(cancellationToken);

        public async ValueTask<ObjectColumn> SelectObjectColumnByIdAsync(
            Guid objectColumnId,
            CancellationToken cancellationToken = default) =>
                await SelectAsync<ObjectColumn>(new object[] { objectColumnId }, cancellationToken);

        public async ValueTask<ObjectColumn> UpdateObjectColumnAsync(
            ObjectColumn objectColumn,
            CancellationToken cancellationToken = default) =>
                await UpdateAsync(objectColumn, cancellationToken);

        public async ValueTask<ObjectColumn> DeleteObjectColumnAsync(
            ObjectColumn objectColumn,
            CancellationToken cancellationToken = default) =>
                await DeleteAsync(objectColumn, cancellationToken);
    }
}