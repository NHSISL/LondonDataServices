using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyCodeSystems.Exceptions
{
    public class OntologyCodeSystemDependencyException : Xeption
    {
        public OntologyCodeSystemDependencyException(string message, Xeption innerException) 
            : base(message, innerException)
        { }
    }
}