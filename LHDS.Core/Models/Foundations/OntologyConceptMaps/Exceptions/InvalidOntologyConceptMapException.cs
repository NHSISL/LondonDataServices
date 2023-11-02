using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyConceptMaps.Exceptions
{
    public class InvalidOntologyConceptMapException : Xeption
    {
        public InvalidOntologyConceptMapException(string message)
            : base(message)
        { }
    }
}