using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.SpecificationObjects.Exceptions
{
    public class AlreadyExistsDataSetObjectException : Xeption
    {
        public AlreadyExistsDataSetObjectException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}