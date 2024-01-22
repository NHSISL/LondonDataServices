// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.OptOuts.Exceptions
{
    public class OptOutProcessingValidationException : Xeption
    {
        public OptOutProcessingValidationException(Xeption innerException)
            : base(
                message: "OptOut processing validation errors occured, please try again",
                innerException)
        { }
    }
}
