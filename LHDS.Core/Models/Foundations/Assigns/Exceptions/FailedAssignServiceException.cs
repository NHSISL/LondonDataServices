// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Assigns.Exceptions
{
    public class FailedAssignServiceException : Xeption
    {
        public FailedAssignServiceException(string message, Exception? innerException)
          : base(message, innerException)
        { }
    }
}
