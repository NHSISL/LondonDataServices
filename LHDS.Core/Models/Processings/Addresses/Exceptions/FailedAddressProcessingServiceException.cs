// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Processings.Addresses.Exceptions
{
    public class FailedAddressProcessingServiceException : Xeption
    {
        public FailedAddressProcessingServiceException(string message, Exception? innerException)
          : base(message, innerException)
        { }
    }
}
