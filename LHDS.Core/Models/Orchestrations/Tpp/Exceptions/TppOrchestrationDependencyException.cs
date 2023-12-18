// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Tpp.Exceptions
{
    public class TppOrchestrationDependencyException : Xeption
    {
        public TppOrchestrationDependencyException(string message, Xeption innerException)
         : base(message, innerException)
        { }
    }
}
