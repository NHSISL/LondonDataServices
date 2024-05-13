using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions
{
    public class ResolvedAddressDependencyException : Xeption
    {
        public ResolvedAddressDependencyException(string message, Xeption? innerException) 
            : base(message, innerException)
        { }
    }
}