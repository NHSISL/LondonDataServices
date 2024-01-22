// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.OptOuts.Exceptions
{
    public class InvalidMeshMessageOrchestrationException : Xeption
    {
        public InvalidMeshMessageOrchestrationException(string message)
            : base(message)
        { }
    }
}