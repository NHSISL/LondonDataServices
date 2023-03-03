// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Audits.Exceptions
{
    public class InvalidAuditReferenceException : Xeption
    {
        public InvalidAuditReferenceException(Exception innerException)
            : base(message: "Invalid audit reference error occurred.", innerException) { }
    }
}