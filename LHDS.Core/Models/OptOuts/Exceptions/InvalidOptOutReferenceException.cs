using System;
using Xeptions;

namespace LHDS.Core.Models.OptOuts.Exceptions
{
    public class InvalidOptOutReferenceException : Xeption
    {
        public InvalidOptOutReferenceException(Exception innerException)
            : base(message: "Invalid optOut reference error occurred.", innerException) { }
    }
}