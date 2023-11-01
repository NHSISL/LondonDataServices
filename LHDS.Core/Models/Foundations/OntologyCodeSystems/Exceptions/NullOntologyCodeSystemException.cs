using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyCodeSystems.Exceptions
{
    public class NullOntologyCodeSystemException : Xeption
    {
        public NullOntologyCodeSystemException(string message)
            : base(message)
        { }
    }
}