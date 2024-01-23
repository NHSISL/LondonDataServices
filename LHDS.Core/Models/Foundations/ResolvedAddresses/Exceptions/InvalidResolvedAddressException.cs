using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions
{
    public class InvalidResolvedAddressException : Xeption
    {
        public InvalidResolvedAddressException(string message)
            : base(message)
        { }
    }
}