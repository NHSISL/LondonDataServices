using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions
{
    public class DataSetSpecificationDependencyValidationException : Xeption
    {
        public DataSetSpecificationDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}