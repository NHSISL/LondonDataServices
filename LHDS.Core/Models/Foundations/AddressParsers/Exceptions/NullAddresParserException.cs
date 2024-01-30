using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressParsers.Exceptions
{
    public class NullAddressParserException : Xeption
    {
        public NullAddressParserException(string message)
            : base(message)
        { }
    }
}