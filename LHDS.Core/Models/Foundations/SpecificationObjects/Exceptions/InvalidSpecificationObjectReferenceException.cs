using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.SpecificationObjects.Exceptions
{
    public class InvalidSpecificationObjectReferenceException : Xeption
    {
        public InvalidSpecificationObjectReferenceException(string message, Exception? innerException)
            : base(message, innerException) { }
    }
}