using Xeptions;

namespace LHDS.Core.Models.Foundations.DataTypes.Exceptions
{
    public class DataTypeValidationException : Xeption
    {
        public DataTypeValidationException(Xeption innerException)
            : base(message: "DataType validation errors occurred, please try again.",
                  innerException)
        { }
    }
}