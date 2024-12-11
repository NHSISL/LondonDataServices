// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Assigns.Exceptions
{
    public class AssignValidationException : Xeption
    {
        public AssignValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}