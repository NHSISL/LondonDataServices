// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions
{
    public class InvalidResolvedAddressReferenceException : Xeption
    {
        public InvalidResolvedAddressReferenceException(string message, Exception? innerException)
            : base(message, innerException) { }
    }
}