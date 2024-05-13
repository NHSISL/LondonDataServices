using Xeptions;

namespace LHDS.Core.Models.Foundations.DataTypes.Exceptions
{
    public class DataTypeValidationException : Xeption
    {
        public DataTypeValidationException(string message, Xeption? innerException)
            : base(message,innerException)
        { }
    }
}