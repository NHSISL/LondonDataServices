using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyConceptMaps.Exceptions
{
    public class LockedOntologyConceptMapException : Xeption
    {
        public LockedOntologyConceptMapException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}