// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressExtractionAudits.Exceptions
{
    public class AddressExtractionAuditServiceException : Xeption
    {
        public AddressExtractionAuditServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}