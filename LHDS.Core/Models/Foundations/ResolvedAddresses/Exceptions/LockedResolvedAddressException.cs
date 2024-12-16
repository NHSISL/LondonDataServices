// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions
{
    public class LockedResolvedAddressException : Xeption
    {
        public LockedResolvedAddressException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}