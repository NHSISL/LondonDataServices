using Xeptions;

namespace LHDS.Core.Models.Foundations.SecureData.Exceptions
{
    public class SecureDataValidationException : Xeption
    {
        public SecureDataValidationException(string message, Xeption innerException)
            : base(message,innerException)
        { }
    }
}