using Xeptions;

namespace LHDS.Core.Models.Foundations.Assigns.Exceptions
{
    public class AssignValidationException : Xeption
    {
        public AssignValidationException(string message, Xeption? innerException)
            : base(message,innerException)
        { }
    }
}