// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.OptOuts;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<OptOut> InsertOptOutAsync(OptOut optOut);
        ValueTask<IQueryable<OptOut>> SelectAllOptOutsAsync();
        ValueTask<OptOut> SelectOptOutByIdAsync(Guid optOutId);
        ValueTask<OptOut> UpdateOptOutAsync(OptOut optOut);
        ValueTask<OptOut> DeleteOptOutAsync(OptOut optOut);
    }
}
