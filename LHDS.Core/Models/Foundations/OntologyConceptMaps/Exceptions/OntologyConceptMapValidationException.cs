using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyConceptMaps.Exceptions
{
    public class OntologyConceptMapValidationException : Xeption
    {
        public OntologyConceptMapValidationException(string message, Xeption innerException)
            : base(message,innerException)
        { }
    }
}