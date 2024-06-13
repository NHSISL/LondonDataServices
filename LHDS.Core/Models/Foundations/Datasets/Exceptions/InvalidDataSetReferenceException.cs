using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSets.Exceptions
{
    public class InvalidDataSetReferenceException : Xeption
    {
        public InvalidDataSetReferenceException(string message, Exception? innerException)
            : base(message, innerException) { }
    }
}