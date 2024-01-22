// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressLoadingAudits.Exceptions
{
    public class AddressLoadingAuditValidationException : Xeption
    {
        public AddressLoadingAuditValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}