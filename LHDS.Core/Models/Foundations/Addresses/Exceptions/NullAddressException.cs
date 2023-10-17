using Xeptions;

namespace LHDS.Core.Models.Foundations.Addresses.Exceptions
{
    public class NullAddressException : Xeption
    {
        public NullAddressException(string message)
            : base(message)
        { }
    }
}