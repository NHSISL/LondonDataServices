// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.PdsAudits.Exceptions
{
    public class InvalidPdsAuditReferenceException : Xeption
    {
        public InvalidPdsAuditReferenceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}