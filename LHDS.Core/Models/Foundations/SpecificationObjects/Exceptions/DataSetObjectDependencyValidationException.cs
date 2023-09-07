using Xeptions;

namespace LHDS.Core.Models.Foundations.SpecificationObjects.Exceptions
{
    public class DataSetObjectDependencyValidationException : Xeption
    {
        public DataSetObjectDependencyValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}