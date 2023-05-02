using Xeptions;

namespace LHDS.Core.Models.PdsAudits.Exceptions
{
    public class NullPdsAuditException : Xeption
    {
        public NullPdsAuditException()
            : base(message: "PdsAudit is null.")
        { }
    }
}