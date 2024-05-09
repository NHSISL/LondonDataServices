using Xeptions;

namespace LHDS.Core.Models.Foundations.Ontologies.Exceptions
{
    public class OntologyDependencyValidationException : Xeption
    {
        public OntologyDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}