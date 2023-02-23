using Xeptions;

namespace LHDS.Core.Models.Audits.Exceptions
{
    public class NullAuditException : Xeption
    {
        public NullAuditException()
            : base(message: "Audit is null.")
        { }
    }
}