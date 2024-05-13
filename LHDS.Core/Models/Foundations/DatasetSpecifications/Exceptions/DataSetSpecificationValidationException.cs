using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions
{
    public class DataSetSpecificationValidationException : Xeption
    {
        public DataSetSpecificationValidationException(string message, Xeption? innerException)
            : base(message,innerException)
        { }
    }
}