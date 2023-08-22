using Xeptions;

namespace LHDS.Core.Models.Foundations.DataTypes.Exceptions
{
    public class NullDataTypeException : Xeption
    {
        public NullDataTypeException()
            : base(message: "DataType is null.")
        { }
    }
}