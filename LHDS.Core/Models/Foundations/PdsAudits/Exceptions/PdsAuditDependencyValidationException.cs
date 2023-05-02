using Xeptions;

namespace LHDS.Core.Models.Foundations.PdsAudits.Exceptions
{
    public class PdsAuditDependencyValidationException : Xeption
    {
        public PdsAuditDependencyValidationException(Xeption innerException)
            : base(message: "PdsAudit dependency validation occurred, please try again.", innerException)
        { }
    }
}