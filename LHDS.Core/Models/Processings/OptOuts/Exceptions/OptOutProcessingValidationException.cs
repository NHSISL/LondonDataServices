// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.OptOuts.Exceptions
{
    public class OptOutProcessingValidationException : Xeption
    {
        public OptOutProcessingValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
