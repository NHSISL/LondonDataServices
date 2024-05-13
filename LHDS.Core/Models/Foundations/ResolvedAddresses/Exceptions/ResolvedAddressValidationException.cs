using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions
{
    public class ResolvedAddressValidationException : Xeption
    {
        public ResolvedAddressValidationException(string message, Xeption? innerException)
            : base(message,innerException)
        { }
    }
}