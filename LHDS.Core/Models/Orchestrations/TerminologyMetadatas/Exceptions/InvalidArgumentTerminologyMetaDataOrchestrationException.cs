// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.TerminologyMetadatas.Exceptions
{
    public class InvalidArgumentTerminologyMetaDataOrchestrationException : Xeption
    {
        public InvalidArgumentTerminologyMetaDataOrchestrationException(string message)
            : base(message)
        { }
    }
}
