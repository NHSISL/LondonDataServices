// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.PdsAudits.Exceptions
{
    public class NotFoundPdsAuditException : Xeption
    {
        public NotFoundPdsAuditException(Guid pdsAuditId)
            : base(message: $"Couldn't find pdsAudit with pdsAuditId: {pdsAuditId}.")
        { }
    }
}