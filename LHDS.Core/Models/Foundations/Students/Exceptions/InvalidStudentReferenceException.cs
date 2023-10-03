using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Students.Exceptions
{
    public class InvalidStudentReferenceException : Xeption
    {
        public InvalidStudentReferenceException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}