using Xeptions;

namespace LHDS.Core.Models.Foundations.DataTypes.Exceptions
{
    public class InvalidDataTypeException : Xeption
    {
        public InvalidDataTypeException(string message)
            : base(message)
        { }
    }
}