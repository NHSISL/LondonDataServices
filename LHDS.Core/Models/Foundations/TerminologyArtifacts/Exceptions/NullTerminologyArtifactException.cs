// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions
{
    public class NullTerminologyArtifactException : Xeption
    {
        public NullTerminologyArtifactException(string message)
            : base(message)
        { }
    }
}