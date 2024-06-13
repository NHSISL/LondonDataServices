// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Processings.AddressMatchers.Exceptions
{
    public class FailedAddressMatcherServiceException : Xeption
    {
        public FailedAddressMatcherServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}