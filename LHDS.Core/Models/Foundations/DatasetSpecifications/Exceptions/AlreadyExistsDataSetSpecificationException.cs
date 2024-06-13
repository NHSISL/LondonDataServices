using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions
{
    public class AlreadyExistsDataSetSpecificationException : Xeption
    {
        public AlreadyExistsDataSetSpecificationException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}