// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions
{
    public class TerminologyArtifactDependencyException : Xeption
    {
        public TerminologyArtifactDependencyException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}