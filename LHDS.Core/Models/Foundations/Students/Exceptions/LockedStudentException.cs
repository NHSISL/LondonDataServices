using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Students.Exceptions
{
    public class LockedStudentException : Xeption
    {
        public LockedStudentException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}