using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSetObjects.Exceptions
{
    public class InvalidDataSetObjectReferenceException : Xeption
    {
        public InvalidDataSetObjectReferenceException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}