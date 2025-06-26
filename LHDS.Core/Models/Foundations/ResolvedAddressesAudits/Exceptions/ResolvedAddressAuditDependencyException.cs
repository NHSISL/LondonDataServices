// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddressAudits.Exceptions
{
    public class ResolvedAddressAuditDependencyException : Xeption
    {
        public ResolvedAddressAuditDependencyException(string message, Xeption? innerException) :
            base(message, innerException)
        { }
    }
}