// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddressAudits.Exceptions
{
    public class LockedResolvedAddressAuditException : Xeption
    {
        public LockedResolvedAddressAuditException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}