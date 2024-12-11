// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.TerminologyArtifacts.Exceptions
{
    public class TerminologyArtifactProcessingDependencyException : Xeption
    {
        public TerminologyArtifactProcessingDependencyException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
