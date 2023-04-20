// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.OptOuts;

namespace LHDS.Core.Services.Processings.OptOuts
{
    public interface IOptOutProcessingService
    {
        ValueTask<OptOut> RetrieveOrAddOptOutAsync(OptOut optOut);
        ValueTask<OptOut> ModifyOptOutAsync(OptOut optOut);
        ValueTask<OptOut> RemoveOptOutByIdAsync(Guid optOutId);
        ValueTask<OptOut> RetrieveOptOutByIdAsync(Guid optOutId);
        ValueTask<OptOut> RetrieveOptOutByNhsNumberAsync(string optOutNhsNumber);
        ValueTask<List<OptOut>> RetrieveAllExpiredOptOutsAsync(int olderThanDays);
    }
}
