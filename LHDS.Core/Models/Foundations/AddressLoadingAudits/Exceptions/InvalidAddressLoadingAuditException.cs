using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressLoadingAudits.Exceptions
{
    public class InvalidAddressLoadingAuditException : Xeption
    {
        public InvalidAddressLoadingAuditException(string message)
            : base(message)
        { }
    }
}