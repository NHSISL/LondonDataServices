using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.SpecificationObjects.Exceptions
{
    public class FailedDataSetObjectStorageException : Xeption
    {
        public FailedDataSetObjectStorageException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}