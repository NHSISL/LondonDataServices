// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.OptOuts.Exceptions
{
    public class InvalidMeshMessageOrchestrationException : Xeption
    {
        public InvalidMeshMessageOrchestrationException()
            : base(message: "Invalid mesh message orchestration error, please correct the errors and try again.") { }
    }
}