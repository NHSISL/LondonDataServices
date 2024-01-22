// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressExtractionAudits.Exceptions
{
    public class NotFoundAddressExtractionAuditException : Xeption
    {
        public NotFoundAddressExtractionAuditException(Guid addressExtractionAuditId)
            : base(message: $"Couldn't find addressExtractionAudit with addressExtractionAuditId: {addressExtractionAuditId}.")
        { }
    }
}