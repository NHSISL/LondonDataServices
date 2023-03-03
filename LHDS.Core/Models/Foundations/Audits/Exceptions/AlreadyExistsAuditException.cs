// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Audits.Exceptions
{
    public class AlreadyExistsAuditException : Xeption
    {
        public AlreadyExistsAuditException(Exception innerException)
            : base(message: "Audit with the same Id already exists.", innerException) { }
    }
}