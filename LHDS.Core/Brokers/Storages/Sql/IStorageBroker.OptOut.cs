// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.OptOuts;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<OptOut> InsertOptOutAsync(OptOut supplier);
        IQueryable<OptOut> SelectAllOptOuts();
        ValueTask<OptOut> SelectOptOutByIdAsync(Guid supplierId);
        ValueTask<OptOut> UpdateOptOutAsync(OptOut supplier);
        ValueTask<OptOut> DeleteOptOutAsync(OptOut supplier);
    }
}
