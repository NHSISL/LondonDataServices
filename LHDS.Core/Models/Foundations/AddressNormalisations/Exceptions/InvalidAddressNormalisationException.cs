using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressNormalisations.Exceptions
{
    public class InvalidAddressNormalisationException : Xeption
    {
        public InvalidAddressNormalisationException(string message)
            : base(message)
        { }
    }
}