// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.PdsAudits.Exceptions
{
    public class InvalidPdsAuditException : Xeption
    {
        public InvalidPdsAuditException(string message)
            : base(message)
        { }
    }
}