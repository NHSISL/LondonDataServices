using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyCodeSystems.Exceptions
{
    public class FailedOntologyCodeSystemStorageException : Xeption
    {
        public FailedOntologyCodeSystemStorageException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}