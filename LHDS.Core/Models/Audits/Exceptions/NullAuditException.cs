// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Audits.Exceptions
{
    public class NullAuditException : Xeption
    {
        public NullAuditException()
            : base(message: "Audit is null.")
        { }
    }
}