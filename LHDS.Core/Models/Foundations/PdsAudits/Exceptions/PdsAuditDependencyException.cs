// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.PdsAudits.Exceptions
{
    public class PdsAuditDependencyException : Xeption
    {
        public PdsAuditDependencyException(string message, Xeption? innerException) :
            base(message, innerException)
        { }
    }
}