using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions
{
    public class LockedTerminologyArtifactException : Xeption
    {
        public LockedTerminologyArtifactException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}