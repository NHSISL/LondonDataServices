using Xeptions;

namespace LHDS.Core.Models.Foundations.SecureData.Exceptions
{
    public class NullSecureDataException : Xeption
    {
        public NullSecureDataException(string message)
            : base(message)
        { }
    }
}