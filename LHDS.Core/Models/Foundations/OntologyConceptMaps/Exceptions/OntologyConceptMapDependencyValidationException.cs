using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyConceptMaps.Exceptions
{
    public class OntologyConceptMapDependencyValidationException : Xeption
    {
        public OntologyConceptMapDependencyValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}