// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.AddressPersistances.Exceptions
{
    public class AddressPersistanceOrchestrationDependencyException : Xeption
    {
        public AddressPersistanceOrchestrationDependencyException(string message, Xeption innerException)
         : base(message, innerException)
        { }
    }
}
