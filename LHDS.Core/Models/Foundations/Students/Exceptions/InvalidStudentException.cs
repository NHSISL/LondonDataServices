using Xeptions;

namespace LHDS.Core.Models.Foundations.Students.Exceptions
{
    public class InvalidStudentException : Xeption
    {
        public InvalidStudentException(string message)
            : base(message)
        { }
    }
}