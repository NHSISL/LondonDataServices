using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSets.Exceptions
{
    public class DataSetServiceException : Xeption
    {
        public DataSetServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}