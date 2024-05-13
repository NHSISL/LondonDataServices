using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSets.Exceptions
{
    public class DataSetValidationException : Xeption
    {
        public DataSetValidationException(string message, Xeption? innerException)
            : base(message,innerException)
        { }
    }
}