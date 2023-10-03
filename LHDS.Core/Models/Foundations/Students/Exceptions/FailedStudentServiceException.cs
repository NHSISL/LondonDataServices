using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Students.Exceptions
{
    public class FailedStudentServiceException : Xeption
    {
        public FailedStudentServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}