using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyValueSets.Exceptions
{
    public class OntologyValueSetDependencyException : Xeption
    {
        public OntologyValueSetDependencyException(string message, Xeption innerException) 
            : base(message, innerException)
        { }
    }
}