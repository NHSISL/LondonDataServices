// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.TerminologyArtifacts.Exceptions
{
    public class InvalidArgumentTerminologyArtifactProcessingException : Xeption
    {
        public InvalidArgumentTerminologyArtifactProcessingException(string message)
           : base(message)
        { }
    }
}
