// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Audits.Exceptions
{
    public class LockedAuditException : Xeption
    {
        public LockedAuditException(Exception innerException)
            : base(message: "Locked audit record exception, please try again later", innerException)
        {
        }
    }
}