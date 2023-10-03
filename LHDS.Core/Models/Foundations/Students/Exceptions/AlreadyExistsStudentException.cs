using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Students.Exceptions
{
    public class AlreadyExistsStudentException : Xeption
    {
        public AlreadyExistsStudentException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}