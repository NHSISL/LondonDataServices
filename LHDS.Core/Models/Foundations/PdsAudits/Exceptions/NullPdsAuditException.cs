// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.PdsAudits.Exceptions
{
    public class NullPdsAuditException : Xeption
    {
        public NullPdsAuditException(string message)
            : base(message)
        { }
    }
}