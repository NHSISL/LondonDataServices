using Xeptions;

namespace LHDS.Core.Models.Foundations.SpecificationObjects.Exceptions
{
    public class DataSetObjectDependencyException : Xeption
    {
        public DataSetObjectDependencyException(string message, Xeption innerException) :
            base(message, innerException)
        { }
    }
}