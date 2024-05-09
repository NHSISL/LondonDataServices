// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.OptOuts.Exceptions
{
    public class OptOutProcessingDependencyException : Xeption
    {
        public OptOutProcessingDependencyException(Xeption? innerException) :
            base(message: "Opt out processing dependency error occurred, please contact support.", innerException)
        { }
    }
}
