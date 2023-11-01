using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyValueSets.Exceptions
{
    public class NullOntologyValueSetException : Xeption
    {
        public NullOntologyValueSetException(string message)
            : base(message)
        { }
    }
}