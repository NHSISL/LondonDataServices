// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.OptOuts;

namespace LHDS.Core.Services.Processings.OptOuts
{
    public interface IOptOutProcessingService
    {
        ValueTask<IQueryable<OptOut>> RetrieveAllOptOutsAsync();
        ValueTask<OptOut> RetrieveOrAddOptOutAsync(OptOut optOut);
        ValueTask<OptOut> AddOrModifyOptOutAsync(OptOut optOut);
        ValueTask<OptOut> RemoveOptOutByIdAsync(Guid optOutId);
        ValueTask<OptOut> RetrieveOptOutByIdAsync(Guid optOutId);
        ValueTask<OptOut?> RetrieveOptOutByNhsNumberAsync(string optOutNhsNumber);
        ValueTask<List<OptOutSummary>> RetrieveAllExpiredOptOutsAsync(int olderThanDays);
        ValueTask<List<OptOut>> RetrieveAllOptOutsByBatchReferenceAsync(string batchReference);

        ValueTask<List<OptOut>> ConsolidateOptOutChangesAndReturnChangesOnly(
            List<OptOut> currentOptOutList,
            List<string> consentedItems);
    }
}
