using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions
{
    public class LockedDataSetSpecificationException : Xeption
    {
        public LockedDataSetSpecificationException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}