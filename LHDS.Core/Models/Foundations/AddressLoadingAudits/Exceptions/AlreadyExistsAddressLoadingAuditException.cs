// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressLoadingAudits.Exceptions
{
    public class AlreadyExistsAddressLoadingAuditException : Xeption
    {
        public AlreadyExistsAddressLoadingAuditException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}