using Xeptions;

namespace LHDS.Core.Models.Foundations.DataTypes.Exceptions
{
    public class DataTypeDependencyException : Xeption
    {
        public DataTypeDependencyException(string message, Xeption? innerException) :
            base(message, innerException)
        { }
    }
}