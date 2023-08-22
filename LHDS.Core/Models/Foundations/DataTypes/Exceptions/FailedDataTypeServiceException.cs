using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.DataTypes.Exceptions
{
    public class FailedDataTypeServiceException : Xeption
    {
        public FailedDataTypeServiceException(Exception innerException)
            : base(message: "Failed dataType service occurred, please contact support", innerException)
        { }
    }
}