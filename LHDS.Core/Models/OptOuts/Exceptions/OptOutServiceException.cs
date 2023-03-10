using System;
using Xeptions;

namespace LHDS.Core.Models.OptOuts.Exceptions
{
    public class OptOutServiceException : Xeption
    {
        public OptOutServiceException(Exception innerException)
            : base(message: "OptOut service error occurred, contact support.", innerException)
        { }
    }
}