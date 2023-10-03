using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Students.Exceptions
{
    public class FailedStudentStorageException : Xeption
    {
        public FailedStudentStorageException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}