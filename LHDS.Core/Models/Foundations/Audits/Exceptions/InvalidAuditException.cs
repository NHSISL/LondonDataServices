// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Audits.Exceptions
{
    public class InvalidAuditException : Xeption
    {
        public InvalidAuditException(string message)
            : base(message: "Invalid audit. Please correct the errors and try again.") { }
    }
}