// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddressAudits.Exceptions
{
    public class ResolvedAddressAuditValidationException : Xeption
    {
        public ResolvedAddressAuditValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}