using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions
{
    public class InvalidTerminologyArtifactReferenceException : Xeption
    {
        public InvalidTerminologyArtifactReferenceException(string message, Exception? innerException)
            : base(message, innerException) { }
    }
}