using Xeptions;

namespace LHDS.Core.Models.Foundations.Students.Exceptions
{
    public class StudentDependencyValidationException : Xeption
    {
        public StudentDependencyValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}