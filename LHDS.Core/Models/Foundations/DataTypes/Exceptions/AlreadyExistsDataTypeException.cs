using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.DataTypes.Exceptions
{
    public class AlreadyExistsDataTypeException : Xeption
    {
        public AlreadyExistsDataTypeException(Exception innerException)
            : base(message: "DataType with the same Id already exists.", innerException)
        { }
    }
}