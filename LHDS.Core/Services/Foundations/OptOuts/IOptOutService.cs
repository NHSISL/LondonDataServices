// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.OptOuts;

namespace LHDS.Core.Services.Foundations.OptOuts
{
    public interface IOptOutService
    {
        ValueTask<OptOut> AddOptOutAsync(OptOut optOut);
        ValueTask BulkAddOptOutsAsync(List<OptOut> optOuts, string fileName);
        ValueTask BulkModifyOptOutsAsync(List<OptOut> optOuts, string fileName);
        ValueTask<IQueryable<OptOut>> RetrieveAllOptOutsAsync();
        ValueTask<OptOut> RetrieveOptOutByIdAsync(Guid optOutId);
        ValueTask<OptOut> ModifyOptOutAsync(OptOut optOut);
        ValueTask<OptOut> RemoveOptOutByIdAsync(Guid optOutId);
    }
}