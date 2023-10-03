using Xeptions;

namespace LHDS.Core.Models.Foundations.Students.Exceptions
{
    public class StudentDependencyException : Xeption
    {
        public StudentDependencyException(string message, Xeption innerException) 
            : base(message, innerException)
        { }
    }
}