// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressLoadingAudits.Exceptions
{
    public class NullAddressLoadingAuditException : Xeption
    {
        public NullAddressLoadingAuditException(string message)
            : base(message)
        { }
    }
}