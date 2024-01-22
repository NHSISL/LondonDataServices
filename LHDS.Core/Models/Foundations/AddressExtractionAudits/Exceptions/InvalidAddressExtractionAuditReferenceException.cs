// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressExtractionAudits.Exceptions
{
    public class InvalidAddressExtractionAuditReferenceException : Xeption
    {
        public InvalidAddressExtractionAuditReferenceException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}