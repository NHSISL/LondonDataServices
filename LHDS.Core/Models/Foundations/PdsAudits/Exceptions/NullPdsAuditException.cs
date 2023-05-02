using Xeptions;

namespace LHDS.Core.Models.Foundations.PdsAudits.Exceptions
{
    public class NullPdsAuditException : Xeption
    {
        public NullPdsAuditException()
            : base(message: "PdsAudit is null.")
        { }
    }
}