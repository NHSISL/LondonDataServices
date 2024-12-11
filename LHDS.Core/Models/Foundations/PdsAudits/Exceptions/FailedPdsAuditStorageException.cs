// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.PdsAudits.Exceptions
{
    public class FailedPdsAuditStorageException : Xeption
    {
        public FailedPdsAuditStorageException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}