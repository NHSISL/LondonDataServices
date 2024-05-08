using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions
{
    public class InvalidDataSetSpecificationReferenceException : Xeption
    {
        public InvalidDataSetSpecificationReferenceException(string message, Exception? innerException)
            : base(message, innerException) { }
    }
}