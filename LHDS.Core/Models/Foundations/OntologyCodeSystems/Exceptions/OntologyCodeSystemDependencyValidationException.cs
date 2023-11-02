using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyCodeSystems.Exceptions
{
    public class OntologyCodeSystemDependencyValidationException : Xeption
    {
        public OntologyCodeSystemDependencyValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}