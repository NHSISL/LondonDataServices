using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSetObjects.Exceptions
{
    public class DataSetObjectServiceException : Xeption
    {
        public DataSetObjectServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}