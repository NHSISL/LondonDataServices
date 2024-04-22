// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressLoadingAudits.Exceptions
{
    public class InvalidAddressLoadingAuditReferenceException : Xeption
    {
        public InvalidAddressLoadingAuditReferenceException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}