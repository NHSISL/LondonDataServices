using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressExtractionAudits.Exceptions
{
    public class InvalidAddressExtractionAuditException : Xeption
    {
        public InvalidAddressExtractionAuditException(string message)
            : base(message)
        { }
    }
}