// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.OptOuts;

namespace LHDS.Core.Services.Processings.OptOuts
{
    public interface IOptOutProcessingService
    {
        ValueTask<OptOut> RetrieveOrAddOptOutAsync(OptOut optOut);
    }
}
