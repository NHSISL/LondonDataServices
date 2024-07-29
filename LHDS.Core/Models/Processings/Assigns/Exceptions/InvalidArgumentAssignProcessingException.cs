// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Assigns.Exceptions
{
    public class InvalidArgumentAssignProcessingException : Xeption
    {
        public InvalidArgumentAssignProcessingException(string message)
            : base(message)
        { }
    }
}