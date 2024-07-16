// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Processings.AssignAddresses.Exceptions
{
    public class FailedAssignAddressProcessingServiceException : Xeption
    {
        public FailedAssignAddressProcessingServiceException(string message, Exception? innerException)
          : base(message, innerException)
        { }
    }
}
