using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSetObjects.Exceptions
{
    public class LockedDataSetObjectException : Xeption
    {
        public LockedDataSetObjectException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}