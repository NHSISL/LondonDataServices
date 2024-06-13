// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.AddressNormalisations.Exceptions
{
    public class AddressNormalisationOrchestrationDependencyValidationException : Xeption
    {
        public AddressNormalisationOrchestrationDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
