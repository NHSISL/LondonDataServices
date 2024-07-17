// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Assigns.Exceptions
{
    public class AssignProcessingDependencyException : Xeption
    {
        public AssignProcessingDependencyException(string message, Xeption? innerException) :
            base(message, innerException)
        { }
    }
}
