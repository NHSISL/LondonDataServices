using System;
using Xeptions;

namespace LHDS.Core.Models.OptOuts.Exceptions
{
    public class FailedOptOutStorageException : Xeption
    {
        public FailedOptOutStorageException(Exception innerException)
            : base(message: "Failed optOut storage error occurred, contact support.", innerException)
        { }
    }
}