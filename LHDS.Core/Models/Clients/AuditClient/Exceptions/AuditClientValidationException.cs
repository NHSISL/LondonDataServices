// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Clients.AuditClient.Exceptions
{
    public class AuditClientValidationException : Xeption
    {
        public AuditClientValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
