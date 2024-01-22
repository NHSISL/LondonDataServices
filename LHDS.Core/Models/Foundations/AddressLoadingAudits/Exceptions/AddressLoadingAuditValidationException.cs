using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressLoadingAudits.Exceptions
{
    public class AddressLoadingAuditValidationException : Xeption
    {
        public AddressLoadingAuditValidationException(string message, Xeption innerException)
            : base(message,innerException)
        { }
    }
}