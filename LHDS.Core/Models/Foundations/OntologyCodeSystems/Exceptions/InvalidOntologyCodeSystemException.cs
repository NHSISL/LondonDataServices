using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyCodeSystems.Exceptions
{
    public class InvalidOntologyCodeSystemException : Xeption
    {
        public InvalidOntologyCodeSystemException(string message)
            : base(message)
        { }
    }
}