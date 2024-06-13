// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.AddressPersistances.Exceptions
{
    public class AddressPersistenceOrchestrationValidationException : Xeption
    {
        public AddressPersistenceOrchestrationValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
