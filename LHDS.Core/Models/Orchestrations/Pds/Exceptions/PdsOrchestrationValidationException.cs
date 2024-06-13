// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Pds.Exceptions
{
    public class PdsOrchestrationValidationException : Xeption
    {
        public PdsOrchestrationValidationException(string message, Xeption? innerException)
            : base(message,innerException)
        { }
    }
}
