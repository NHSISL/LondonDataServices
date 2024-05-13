using Xeptions;

namespace LHDS.Core.Models.Foundations.DataTypes.Exceptions
{
    public class DataTypeDependencyValidationException : Xeption
    {
        public DataTypeDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}