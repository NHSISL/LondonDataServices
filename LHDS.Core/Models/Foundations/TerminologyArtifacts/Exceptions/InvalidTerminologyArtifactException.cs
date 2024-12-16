// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions
{
    public class InvalidTerminologyArtifactException : Xeption
    {
        public InvalidTerminologyArtifactException(string message)
            : base(message)
        { }
    }
}