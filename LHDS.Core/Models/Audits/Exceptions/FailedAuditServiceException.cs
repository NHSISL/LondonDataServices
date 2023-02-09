// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Audits.Exceptions
{
    public class FailedAuditServiceException : Xeption
    {
        public FailedAuditServiceException(Exception innerException)
            : base(message: "Failed audit service occurred, please contact support", innerException)
        { }
    }
}