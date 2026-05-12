// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.OptOuts;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<OptOut> OptOuts { get; set; }

        public async ValueTask<OptOut> InsertOptOutAsync(
            OptOut optout,
            CancellationToken cancellationToken = default) =>
                await InsertAsync(optout, cancellationToken);

        public async ValueTask<IQueryable<OptOut>> SelectAllOptOutsAsync(
            CancellationToken cancellationToken = default) =>
                await SelectAllAsync<OptOut>(cancellationToken);

        public async ValueTask<OptOut> SelectOptOutByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default) =>
                await SelectAsync<OptOut>(new object[] { id }, cancellationToken);

        public async ValueTask<OptOut> UpdateOptOutAsync(
            OptOut optout,
            CancellationToken cancellationToken = default) =>
                await UpdateAsync(optout, cancellationToken);

        public async ValueTask<OptOut> DeleteOptOutAsync(
            OptOut optout,
            CancellationToken cancellationToken = default) =>
                await DeleteAsync(optout, cancellationToken);
    }
}
