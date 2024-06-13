// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Clients.TerminologyClient.Exceptions
{
    public class TerminologyClientValidationException : Xeption
    {
        public TerminologyClientValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
