// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions
{
    public class FailedResolvedAddressServiceException : Xeption
    {
        public FailedResolvedAddressServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}