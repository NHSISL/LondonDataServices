// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.PdsAudits.Exceptions
{
    public class AlreadyExistsPdsAuditException : Xeption
    {
        public AlreadyExistsPdsAuditException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}