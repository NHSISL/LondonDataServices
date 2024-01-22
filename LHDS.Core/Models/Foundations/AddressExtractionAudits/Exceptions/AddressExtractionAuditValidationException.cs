// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressExtractionAudits.Exceptions
{
    public class AddressExtractionAuditValidationException : Xeption
    {
        public AddressExtractionAuditValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}