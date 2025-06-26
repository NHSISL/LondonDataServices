// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddressAudits.Exceptions
{
    public class NotFoundResolvedAddressAuditException : Xeption
    {
        public NotFoundResolvedAddressAuditException(Guid resolvedAddressAuditId)
            : base(
                message: $"Couldn't find resolvedAddressAudit with resolvedAddressAuditId: {resolvedAddressAuditId}.")
        { }
    }
}