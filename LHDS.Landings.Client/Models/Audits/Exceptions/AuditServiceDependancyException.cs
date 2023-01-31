// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Landings.Client.Models.Audits.Exceptions
{
    public class AuditServiceDependancyException : Xeption
    {
        public AuditServiceDependancyException(Exception innerException)
            : base(message: "Audit service dependancy error occurred, contact support.", innerException)
        { }
    }
}