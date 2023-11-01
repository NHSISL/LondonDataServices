using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyConceptMaps.Exceptions
{
    public class FailedOntologyConceptMapStorageException : Xeption
    {
        public FailedOntologyConceptMapStorageException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}