using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions
{
    public class NullResolvedAddressException : Xeption
    {
        public NullResolvedAddressException(string message)
            : base(message)
        { }
    }
}