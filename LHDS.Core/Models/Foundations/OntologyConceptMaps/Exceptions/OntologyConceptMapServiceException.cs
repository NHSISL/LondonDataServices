using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyConceptMaps.Exceptions
{
    public class OntologyConceptMapServiceException : Xeption
    {
        public OntologyConceptMapServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}