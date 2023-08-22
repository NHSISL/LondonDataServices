using Xeptions;

namespace LHDS.Core.Models.Foundations.DataTypes.Exceptions
{
    public class InvalidDataTypeException : Xeption
    {
        public InvalidDataTypeException()
            : base(message: "Invalid dataType. Please correct the errors and try again.")
        { }
    }
}