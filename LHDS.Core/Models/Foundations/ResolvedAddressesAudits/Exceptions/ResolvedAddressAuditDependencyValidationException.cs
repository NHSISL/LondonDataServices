// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddressAudits.Exceptions
{
    public class ResolvedAddressAuditDependencyValidationException : Xeption
    {
        public ResolvedAddressAuditDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}