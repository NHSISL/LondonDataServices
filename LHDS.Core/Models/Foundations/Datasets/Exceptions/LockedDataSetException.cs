using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSets.Exceptions
{
    public class LockedDataSetException : Xeption
    {
        public LockedDataSetException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}