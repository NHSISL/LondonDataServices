using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSets.Exceptions
{
    public class FailedDataSetServiceException : Xeption
    {
        public FailedDataSetServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}