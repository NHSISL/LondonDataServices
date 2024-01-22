using Xeptions;

namespace LHDS.Core.Models.Foundations.ObjectColumns.Exceptions
{
    public class InvalidObjectColumnException : Xeption
    {
        public InvalidObjectColumnException(string message)
            : base(message)
        { }
    }
}