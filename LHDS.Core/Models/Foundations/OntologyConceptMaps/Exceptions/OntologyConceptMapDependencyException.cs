using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyConceptMaps.Exceptions
{
    public class OntologyConceptMapDependencyException : Xeption
    {
        public OntologyConceptMapDependencyException(string message, Xeption innerException) 
            : base(message, innerException)
        { }
    }
}