using Xeptions;

namespace LHDS.Core.Models.Foundations.Ontologies.Exceptions
{
    public class OntologyDependencyException : Xeption
    {
        public OntologyDependencyException(string message, Xeption? innerException) :
            base(message, innerException)
        { }
    }
}