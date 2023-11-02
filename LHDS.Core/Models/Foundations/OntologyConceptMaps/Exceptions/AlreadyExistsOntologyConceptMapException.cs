using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyConceptMaps.Exceptions
{
    public class AlreadyExistsOntologyConceptMapException : Xeption
    {
        public AlreadyExistsOntologyConceptMapException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}