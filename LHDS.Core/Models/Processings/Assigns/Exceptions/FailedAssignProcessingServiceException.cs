// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Processings.Assigns.Exceptions
{
    public class FailedAssignProcessingServiceException : Xeption
    {
        public FailedAssignProcessingServiceException(string message, Exception? innerException)
          : base(message, innerException)
        { }
    }
}
