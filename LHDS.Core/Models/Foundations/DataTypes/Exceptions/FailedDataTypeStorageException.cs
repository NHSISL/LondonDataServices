using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.DataTypes.Exceptions
{
    public class FailedDataTypeStorageException : Xeption
    {
        public FailedDataTypeStorageException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}