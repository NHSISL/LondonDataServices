using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.DataTypes.Exceptions
{
    public class FailedDataTypeStorageException : Xeption
    {
        public FailedDataTypeStorageException(Exception innerException)
            : base(message: "Failed dataType storage error occurred, contact support.", innerException)
        { }
    }
}