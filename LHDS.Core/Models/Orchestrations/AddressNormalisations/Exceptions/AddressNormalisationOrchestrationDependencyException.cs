// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.AddressNormalisations.Exceptions
{
    public class AddressNormalisationOrchestrationDependencyException : Xeption
    {
        public AddressNormalisationOrchestrationDependencyException(string message, Xeption? innerException) 
            : base(message, innerException)
        { }
    }
}
