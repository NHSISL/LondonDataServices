using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions
{
    public class NotFoundTerminologyArtifactException : Xeption
    {
        public NotFoundTerminologyArtifactException(Guid terminologyArtifactId)
            : base(message: $"Couldn't find terminologyArtifact with terminologyArtifactId: {terminologyArtifactId}.")
        { }
    }
}