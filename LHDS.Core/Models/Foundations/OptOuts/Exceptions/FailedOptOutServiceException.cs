using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.OptOuts.Exceptions
{
    public class FailedOptOutServiceException : Xeption
    {
        public FailedOptOutServiceException(Exception innerException)
            : base(message: "Failed optOut service occurred, please contact support", innerException)
        { }
    }
}