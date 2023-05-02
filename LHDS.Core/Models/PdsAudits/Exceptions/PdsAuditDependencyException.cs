using Xeptions;

namespace LHDS.Core.Models.PdsAudits.Exceptions
{
    public class PdsAuditDependencyException : Xeption
    {
        public PdsAuditDependencyException(Xeption innerException) :
            base(message: "PdsAudit dependency error occurred, contact support.", innerException)
        { }
    }
}