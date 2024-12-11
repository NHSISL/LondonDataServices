// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.OptOuts.Exceptions
{
    public class OptOutDependencyValidationException : Xeption
    {
        public OptOutDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}