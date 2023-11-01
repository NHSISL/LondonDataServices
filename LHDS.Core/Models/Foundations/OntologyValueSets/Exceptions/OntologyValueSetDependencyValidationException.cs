using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyValueSets.Exceptions
{
    public class OntologyValueSetDependencyValidationException : Xeption
    {
        public OntologyValueSetDependencyValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}