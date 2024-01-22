// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressExtractionAudits.Exceptions
{
    public class AddressExtractionAuditDependencyValidationException : Xeption
    {
        public AddressExtractionAuditDependencyValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}