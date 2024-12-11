// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Pds.Exceptions
{
    public class PdsOrchestrationDependencyException : Xeption
    {
        public PdsOrchestrationDependencyException(string message, Xeption? innerException)
         : base(message, innerException)
        { }
    }
}
