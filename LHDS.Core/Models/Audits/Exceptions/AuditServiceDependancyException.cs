// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Audits.Exceptions
{
    public class AuditServiceDependencyException : Xeption
    {
        public AuditServiceDependencyException(Exception innerException)
            : base(message: "Audit service dependancy error occurred, contact support.", innerException)
        { }
    }
}