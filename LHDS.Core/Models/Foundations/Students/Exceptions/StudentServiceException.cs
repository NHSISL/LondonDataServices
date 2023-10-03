using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Students.Exceptions
{
    public class StudentServiceException : Xeption
    {
        public StudentServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}