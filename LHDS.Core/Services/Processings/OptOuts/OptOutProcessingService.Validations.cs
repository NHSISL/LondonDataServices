// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Processings.OptOuts.Exceptions;
using LHDS.Core.Services.Foundations.OptOuts;

namespace LHDS.Core.Services.Processings.OptOuts
{
    public partial class OptOutProcessingService
    {
        private static void ValidateOptOutProcessingOnRetrieveOrAdd(OptOut optOut)
        {
            ValidateOptOutProcessingIsNotNull(optOut);
        }

        private static void ValidateOptOutProcessingIsNotNull(OptOut optOut)
        {
            if (optOut is null)
            {
                throw new NullOptOutProcessingException();
            }
        }
    }
}

