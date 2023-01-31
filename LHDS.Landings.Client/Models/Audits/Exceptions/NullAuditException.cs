using Xeptions;

namespace LHDS.Landings.Client.Models.Audits.Exceptions
{
    public class NullAuditException : Xeption
    {
        public NullAuditException()
            : base(message: "Audit is null.")
        { }
    }
}