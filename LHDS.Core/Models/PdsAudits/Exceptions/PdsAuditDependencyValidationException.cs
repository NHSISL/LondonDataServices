using Xeptions;

namespace LHDS.Core.Models.PdsAudits.Exceptions
{
    public class PdsAuditDependencyValidationException : Xeption
    {
        public PdsAuditDependencyValidationException(Xeption innerException)
            : base(message: "PdsAudit dependency validation occurred, please try again.", innerException)
        { }
    }
}