// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Pds.Exceptions
{
    public class NullBlobContainersPdsOrchestrationException : Xeption
    {
        public NullBlobContainersPdsOrchestrationException(string message)
            : base(message)
        { }
    }
}
