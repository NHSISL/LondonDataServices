using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyValueSets.Exceptions
{
    public class OntologyValueSetValidationException : Xeption
    {
        public OntologyValueSetValidationException(string message, Xeption innerException)
            : base(message,innerException)
        { }
    }
}