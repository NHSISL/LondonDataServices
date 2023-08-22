using Xeptions;

namespace LHDS.Core.Models.Foundations.DataTypes.Exceptions
{
    public class DataTypeDependencyException : Xeption
    {
        public DataTypeDependencyException(Xeption innerException) :
            base(message: "DataType dependency error occurred, contact support.", innerException)
        { }
    }
}