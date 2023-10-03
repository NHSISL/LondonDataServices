using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Students.Exceptions
{
    public class NotFoundStudentException : Xeption
    {
        public NotFoundStudentException(Guid studentId)
            : base(message: $"Couldn't find student with studentId: {studentId}.")
        { }
    }
}