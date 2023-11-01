using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyValueSets.Exceptions
{
    public class InvalidOntologyValueSetException : Xeption
    {
        public InvalidOntologyValueSetException(string message)
            : base(message)
        { }
    }
}