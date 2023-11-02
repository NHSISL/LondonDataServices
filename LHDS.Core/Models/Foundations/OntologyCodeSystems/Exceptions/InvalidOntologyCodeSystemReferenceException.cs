using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyCodeSystems.Exceptions
{
    public class InvalidOntologyCodeSystemReferenceException : Xeption
    {
        public InvalidOntologyCodeSystemReferenceException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}