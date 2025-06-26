// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddressAudits.Exceptions
{
    public class NullResolvedAddressAuditException : Xeption
    {
        public NullResolvedAddressAuditException(string message)
            : base(message)
        { }
    }
}