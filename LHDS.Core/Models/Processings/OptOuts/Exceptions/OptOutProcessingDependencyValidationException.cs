// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.OptOuts.Exceptions
{
    public class OptOutProcessingDependencyValidationException : Xeption
    {
        public OptOutProcessingDependencyValidationException(Xeption? innerException)
            : base(message: "Opt out processing dependency validation occurred, please try again.", innerException)
        { }
    }
}
