// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Assigns.Exceptions
{
    public class AssignProcessingServiceException : Xeption
    {
        public AssignProcessingServiceException(string message, Xeption? innerException)
          : base(message, innerException)
        { }
    }
}
