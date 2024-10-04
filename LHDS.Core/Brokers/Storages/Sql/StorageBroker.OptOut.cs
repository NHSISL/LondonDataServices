// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.OptOuts;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<OptOut> OptOuts { get; set; }

        public async ValueTask<OptOut> InsertOptOutAsync(OptOut optout) =>
            await InsertAsync(optout);

        public IQueryable<OptOut> SelectAllOptOuts() => SelectAll<OptOut>();

        public async ValueTask<OptOut> SelectOptOutByIdAsync(Guid id) =>
            await SelectAsync<OptOut>(id);

        public async ValueTask<OptOut> UpdateOptOutAsync(OptOut optout) =>
            await UpdateAsync(optout);

        public async ValueTask<OptOut> DeleteOptOutAsync(OptOut optout) =>
            await DeleteAsync(optout);
    }
}
