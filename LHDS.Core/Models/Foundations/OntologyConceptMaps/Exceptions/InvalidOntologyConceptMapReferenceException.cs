using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyConceptMaps.Exceptions
{
    public class InvalidOntologyConceptMapReferenceException : Xeption
    {
        public InvalidOntologyConceptMapReferenceException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}