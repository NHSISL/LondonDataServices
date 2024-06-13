// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Clients.TerminologyClient.Exceptions
{
    public class TerminologyClientServiceException : Xeption
    {
        public TerminologyClientServiceException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
