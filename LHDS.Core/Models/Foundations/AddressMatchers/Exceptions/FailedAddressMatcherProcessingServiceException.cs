// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressMatchers.Exceptions
{
    public class FailedAddressMatcherProcessingServiceException : Xeption
    {
        public FailedAddressMatcherProcessingServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}