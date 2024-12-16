// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Audits.Exceptions
{
    public class NullAuditException : Xeption
    {
        public NullAuditException(string message)
            : base(message)
        { }
    }
}