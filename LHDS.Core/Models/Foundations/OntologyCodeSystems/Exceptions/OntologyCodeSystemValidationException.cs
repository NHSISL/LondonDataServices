using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyCodeSystems.Exceptions
{
    public class OntologyCodeSystemValidationException : Xeption
    {
        public OntologyCodeSystemValidationException(string message, Xeption innerException)
            : base(message,innerException)
        { }
    }
}