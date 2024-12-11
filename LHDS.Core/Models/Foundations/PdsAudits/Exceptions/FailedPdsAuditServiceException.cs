// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.PdsAudits.Exceptions
{
    public class FailedPdsAuditServiceException : Xeption
    {
        public FailedPdsAuditServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}