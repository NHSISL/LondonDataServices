using Xeptions;

namespace LHDS.Core.Models.Foundations.ObjectColumns.Exceptions
{
    public class ObjectColumnDependencyValidationException : Xeption
    {
        public ObjectColumnDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}