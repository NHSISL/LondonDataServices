using Xeptions;

namespace LHDS.Core.Models.Foundations.SpecificationObjects.Exceptions
{
    public class DataSetObjectValidationException : Xeption
    {
        public DataSetObjectValidationException(string message, Xeption innerException)
            : base(message,innerException)
        { }
    }
}