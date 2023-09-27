// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Audits.Exceptions
{
    public class AuditServiceDependencyException : Xeption
    {
        public AuditServiceDependencyException(string message, Exception innerException)
            : base(message, innerException) 
        { }
    }
}