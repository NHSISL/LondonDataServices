using Xeptions;

namespace LHDS.Core.Models.Foundations.DataTypes.Exceptions
{
    public class DataTypeDependencyValidationException : Xeption
    {
        public DataTypeDependencyValidationException(Xeption innerException)
            : base(message: "DataType dependency validation occurred, please try again.", innerException)
        { }
    }
}