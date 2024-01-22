// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.OptOuts.Exceptions
{
    public class OptOutDependencyException : Xeption
    {
        public OptOutDependencyException(Xeption innerException) :
            base(message: "OptOut dependency error occurred, contact support.", innerException)
        { }
    }
}