using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.DataTypes.Exceptions
{
    public class InvalidDataTypeReferenceException : Xeption
    {
        public InvalidDataTypeReferenceException(Exception innerException)
            : base(message: "Invalid dataType reference error occurred.", innerException) { }
    }
}