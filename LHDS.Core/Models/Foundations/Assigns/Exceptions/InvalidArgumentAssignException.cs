// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Assigns.Exceptions
{
    public class InvalidArgumentAssignException : Xeption
    {
        public InvalidArgumentAssignException(string message)
            : base(message)
        { }
    }
}