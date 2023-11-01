using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyConceptMaps.Exceptions
{
    public class FailedOntologyConceptMapServiceException : Xeption
    {
        public FailedOntologyConceptMapServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}