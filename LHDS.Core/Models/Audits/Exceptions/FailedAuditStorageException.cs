// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Audits.Exceptions
{
    public class FailedAuditStorageException : Xeption
    {
        public FailedAuditStorageException(Exception innerException)
            : base(message: "Failed audit storage error occurred, contact support.", innerException)
        { }
    }
}