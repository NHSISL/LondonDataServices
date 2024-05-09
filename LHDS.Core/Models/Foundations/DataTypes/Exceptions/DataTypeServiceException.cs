using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.DataTypes.Exceptions
{
    public class DataTypeServiceException : Xeption
    {
        public DataTypeServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}