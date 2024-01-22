// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.TerminologyArtifacts.Exceptions
{
    public class NullTerminologyArtifactProcessingException : Xeption
    {
        public NullTerminologyArtifactProcessingException(string message)
            : base(message)
        { }
    }
}
