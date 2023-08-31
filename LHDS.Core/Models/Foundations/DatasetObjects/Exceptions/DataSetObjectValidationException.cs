using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSetObjects.Exceptions
{
    public class DataSetObjectValidationException : Xeption
    {
        public DataSetObjectValidationException(string message, Xeption innerException)
            : base(message,innerException)
        { }
    }
}