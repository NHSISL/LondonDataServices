// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Audits.Exceptions
{
    public class AuditDependencyException : Xeption
    {
        public AuditDependencyException(Xeption innerException) :
            base(message: "Audit dependency error occurred, contact support.", innerException)
        { }
    }
}