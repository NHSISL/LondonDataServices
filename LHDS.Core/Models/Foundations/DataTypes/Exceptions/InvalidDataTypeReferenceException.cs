using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.DataTypes.Exceptions
{
    public class InvalidDataTypeReferenceException : Xeption
    {
        public InvalidDataTypeReferenceException(string message, Exception? innerException)
            : base(message, innerException) { }
    }
}