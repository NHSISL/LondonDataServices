using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressExtractionAudits.Exceptions
{
    public class NullAddressExtractionAuditException : Xeption
    {
        public NullAddressExtractionAuditException(string message)
            : base(message)
        { }
    }
}