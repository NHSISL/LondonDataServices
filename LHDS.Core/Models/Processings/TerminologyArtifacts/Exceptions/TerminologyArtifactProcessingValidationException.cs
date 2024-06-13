// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.TerminologyArtifacts.Exceptions
{
    public class TerminologyArtifactProcessingValidationException : Xeption
    {
        public TerminologyArtifactProcessingValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}