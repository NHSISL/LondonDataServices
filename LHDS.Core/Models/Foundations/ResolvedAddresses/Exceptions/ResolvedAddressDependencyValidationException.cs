using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions
{
    public class ResolvedAddressDependencyValidationException : Xeption
    {
        public ResolvedAddressDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}