using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyConceptMaps.Exceptions
{
    public class NullOntologyConceptMapException : Xeption
    {
        public NullOntologyConceptMapException(string message)
            : base(message)
        { }
    }
}