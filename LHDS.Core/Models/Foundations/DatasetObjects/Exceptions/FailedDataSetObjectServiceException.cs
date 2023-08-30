using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSetObjects.Exceptions
{
    public class FailedDataSetObjectServiceException : Xeption
    {
        public FailedDataSetObjectServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}