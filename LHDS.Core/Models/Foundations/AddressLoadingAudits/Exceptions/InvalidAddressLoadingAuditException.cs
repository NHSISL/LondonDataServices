// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressLoadingAudits.Exceptions
{
    public class InvalidAddressLoadingAuditException : Xeption
    {
        public InvalidAddressLoadingAuditException(string message)
            : base(message)
        { }
    }
}