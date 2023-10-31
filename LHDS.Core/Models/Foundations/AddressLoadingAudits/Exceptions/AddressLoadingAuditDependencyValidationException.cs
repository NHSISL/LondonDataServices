using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressLoadingAudits.Exceptions
{
    public class AddressLoadingAuditDependencyValidationException : Xeption
    {
        public AddressLoadingAuditDependencyValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}