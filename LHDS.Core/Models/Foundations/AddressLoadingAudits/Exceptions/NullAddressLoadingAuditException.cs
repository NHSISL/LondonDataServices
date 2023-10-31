using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressLoadingAudits.Exceptions
{
    public class NullAddressLoadingAuditException : Xeption
    {
        public NullAddressLoadingAuditException(string message)
            : base(message)
        { }
    }
}