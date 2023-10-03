using Xeptions;

namespace LHDS.Core.Models.Foundations.Students.Exceptions
{
    public class NullStudentException : Xeption
    {
        public NullStudentException(string message)
            : base(message)
        { }
    }
}