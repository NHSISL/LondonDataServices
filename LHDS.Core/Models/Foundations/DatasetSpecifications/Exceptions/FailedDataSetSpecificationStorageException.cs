using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions
{
    public class FailedDataSetSpecificationStorageException : Xeption
    {
        public FailedDataSetSpecificationStorageException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}