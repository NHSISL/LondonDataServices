// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddressAudits.Exceptions
{
    public class InvalidResolvedAddressAuditException : Xeption
    {
        public InvalidResolvedAddressAuditException(string message)
            : base(message)
        { }
    }
}