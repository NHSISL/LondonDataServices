// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddressAudits.Exceptions
{
    public class ResolvedAddressAuditServiceException : Xeption
    {
        public ResolvedAddressAuditServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}