// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Addresses.Exceptions
{
    public class FailedAddressServiceException : Xeption
    {
        public FailedAddressServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}