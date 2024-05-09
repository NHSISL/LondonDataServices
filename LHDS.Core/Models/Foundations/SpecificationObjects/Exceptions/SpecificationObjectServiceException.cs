using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.SpecificationObjects.Exceptions
{
    public class SpecificationObjectServiceException : Xeption
    {
        public SpecificationObjectServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}