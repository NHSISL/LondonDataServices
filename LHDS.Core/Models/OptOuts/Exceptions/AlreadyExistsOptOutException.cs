using System;
using Xeptions;

namespace LHDS.Core.Models.OptOuts.Exceptions
{
    public class AlreadyExistsOptOutException : Xeption
    {
        public AlreadyExistsOptOutException(Exception innerException)
            : base(message: "OptOut with the same Id already exists.", innerException)
        { }
    }
}