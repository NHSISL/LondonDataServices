// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressLoadingAudits.Exceptions
{
    public class AddressLoadingAuditDependencyValidationException : Xeption
    {
        public AddressLoadingAuditDependencyValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}