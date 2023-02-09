// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Audits.Exceptions
{
    public class AuditDependencyValidationException : Xeption
    {
        public AuditDependencyValidationException(Xeption innerException)
            : base(message: "Audit dependency validation occurred, please try again.", innerException)
        { }
    }
}