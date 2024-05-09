// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.OptOuts.Exceptions
{
    public class OptOutDependencyException : Xeption
    {
        public OptOutDependencyException(string message, Xeption? innerException) :
            base(message, innerException)
        { }
    }
}