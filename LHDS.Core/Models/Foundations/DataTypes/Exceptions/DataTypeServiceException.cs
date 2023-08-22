using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.DataTypes.Exceptions
{
    public class DataTypeServiceException : Xeption
    {
        public DataTypeServiceException(Exception innerException)
            : base(message: "DataType service error occurred, contact support.", innerException)
        { }
    }
}