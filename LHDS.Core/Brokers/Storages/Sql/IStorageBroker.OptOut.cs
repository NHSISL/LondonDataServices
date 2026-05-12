// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.OptOuts;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<OptOut> InsertOptOutAsync(OptOut optOut, CancellationToken cancellationToken = default);
        ValueTask<IQueryable<OptOut>> SelectAllOptOutsAsync(CancellationToken cancellationToken = default);
        ValueTask<OptOut> SelectOptOutByIdAsync(Guid optOutId, CancellationToken cancellationToken = default);
        ValueTask<OptOut> UpdateOptOutAsync(OptOut optOut, CancellationToken cancellationToken = default);
        ValueTask<OptOut> DeleteOptOutAsync(OptOut optOut, CancellationToken cancellationToken = default);
    }
}
