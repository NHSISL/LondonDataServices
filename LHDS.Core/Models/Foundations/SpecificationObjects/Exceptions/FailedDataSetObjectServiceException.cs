using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.SpecificationObjects.Exceptions
{
    public class FailedDataSetObjectServiceException : Xeption
    {
        public FailedDataSetObjectServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}