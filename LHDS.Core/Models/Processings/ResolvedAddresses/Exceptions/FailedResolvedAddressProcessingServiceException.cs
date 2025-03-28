// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Processings.ResolvedAddresses.Exceptions
{
    public class FailedResolvedAddressProcessingServiceException : Xeption
    {
        public FailedResolvedAddressProcessingServiceException(string message, Exception? innerException)
          : base(message, innerException)
        { }
    }
}
