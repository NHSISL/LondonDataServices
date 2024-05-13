// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.OptOuts.Exceptions
{
    public class OptOutValidationException : Xeption
    {
        public OptOutValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}